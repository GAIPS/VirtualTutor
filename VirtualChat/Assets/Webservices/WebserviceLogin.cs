﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Net;
using UnityEngine.UI;
using System.IO;
using System.Collections.Specialized;
using System.Text;
using System;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using UserInfo;
using System.Threading;

public class WebserviceLogin : MonoBehaviour
{

    [Serializable]
    public class Values
    {
        public List<jsonValues.users> users;
        //public List<warnings> warnings;

        public List<jsonValues.Courses> courses;
        public List<jsonValues.Grades> grades;

        public List<jsonValues.Topics> topics;
        public List<jsonValues.usergrades> usergrades;

        public List<jsonValues.quizzes> quizzes; // TODO IMPLEMENT
        public List<jsonValues.instances> instances;
    }


    DateTime lastupdate;

    String userName = "wstestuser";
    String password = "WS_TestUser1";

    String tokenMoodleSession = null;

    public UserData user;

    private Thread updateThread;

    //HUDSize hud; ONDE VAI BUSCAR OS VALORES INSERIDOS PELO USER

    //dataflow scrollview; // ONDE VAI FAZER DISPLAY DA INFO

    String moodleUrl = "http://ec2-52-215-90-45.eu-west-1.compute.amazonaws.com/moodleFCUL";

    int cycle = 0; // saber o nº de vezes que tentou ver actualizações

    // Use this for initialization
    void Start()
    {
        //scrollview = gameObject.AddComponent(typeof(dataflow)) as dataflow;
        //hud = gameObject.AddComponent(typeof(HUDSize)) as HUDSize;
        StartCoroutine("StartUp");
    }

    public void compareTime()
    {

        TimeSpan span = DateTime.UtcNow.Subtract(lastupdate);
        StringBuilder timeString = new StringBuilder();
        if (span.Minutes > 0)
        {
            timeString.Append(span.Minutes + "minutos ");
        }
        timeString.Append(span.Seconds + "segundos");
        //scrollview.addText("Differença de tempo: " + timeString.ToString());

    }
    //int npoint = 0;
    //DateTime lastpoint =DateTime.UtcNow;

    int npoints = 0;
    DateTime last = DateTime.UtcNow, current, lastCheck;
    TimeSpan s;
    Boolean wasPressed = false;
    // Update is called once per frame
    void Update()
    {
        if (!user.readyForRead && wasPressed)
        {
            current = DateTime.UtcNow;
            s = current - last;
            if (s.TotalSeconds > 1) // mexe com os pontos
            {
                if (npoints < 3)
                {
                    //scrollview.joinText(".");
                    npoints++;
                }
                else
                {
                    //scrollview.changeText(scrollview.giveText().Replace(".",""));
                    npoints = 0;
                }
                last = current;
            }
        }

        /*if (scrollview.giveText().Contains("A carregar informação") && user.readyForRead)
        {
            scrollview.changeText(user.giveData());
            StartCoroutine("checkNewInfo");
        }*/


        //TimeSpan span = DateTime.UtcNow.Subtract(lastCheck);

        //if (span.TotalSeconds > 60 && user.readyForRead)
        //{
        //    StartCoroutine("hasUpdates");
        //    lastCheck = DateTime.UtcNow;
        //}

    }

    public void press()
    {
        wasPressed = true;
        //scrollview.changeText("A carregar informação");
        StartCoroutine("StartUp");
        lastupdate = user.datelast;
        //StartCoroutine("webGlRequests");

        // consequentes updates
        lastupdate = DateTime.UtcNow;
        lastCheck = DateTime.UtcNow;
       
    }

    /**
     * Devido ao webgl nao suportar acesso directo as sockets que torna o System.Net invalido, foi necessario criar uma alternativa a solucao corrente
     * */
    IEnumerator webGlRequests()
    {

        WWW www = new WWW(moodleUrl + "/login/token.php" + "?service=tvservice&username=" + userName + "&password=" + password);
        yield return www;
        String content = www.text;

        String[] variable = content.Split(new[] { "\"token\":\"" }, StringSplitOptions.None);
        if (variable.Length > 1)
        {
            variable = variable[1].Split(new[] { "\"" }, StringSplitOptions.None);
            if (variable.Length > 0)
            {
                tokenMoodleSession = variable[0];
                // Get user
                www = new WWW(moodleUrl + "/webservice/rest/server.php" + "?wstoken=" + tokenMoodleSession + "&wsfunction=core_user_get_users&criteria[0][key]=username&criteria[0][value]=" + userName + "&moodlewsrestformat=json");
                yield return www;
                content = www.text;
                Values v = UnityEngine.JsonUtility.FromJson<Values>(content);
                user.receiveUsers(v.users[0]);


                // get courses
                www = new WWW(moodleUrl + "/webservice/rest/server.php" + "?wstoken=" + tokenMoodleSession + "&wsfunction=core_enrol_get_users_courses&userid=" + user.id + "&moodlewsrestformat=json");
                yield return www;
                content = www.text;
                StringBuilder por = new StringBuilder();
                por.Append("{\"courses\":");
                por.Append(content + "}");
                v = JsonUtility.FromJson<Values>(por.ToString());
                user.receiveCourses(v.courses);

                // Get course grades
                www = new WWW(moodleUrl + "/webservice/rest/server.php" + "?wstoken=" + tokenMoodleSession + "&wsfunction=gradereport_overview_get_course_grades&userid=" + user.id + "&moodlewsrestformat=json");
                yield return www;
                content = www.text;

                v = UnityEngine.JsonUtility.FromJson<Values>(content);
                user.receiveGrades(v.grades);
                //user.filterGrades(content);

                foreach (Course c in user.courses)
                {
                    www = new WWW(moodleUrl + "/webservice/rest/server.php" + "?wstoken=" + tokenMoodleSession + "&wsfunction=core_course_get_contents&courseid=" + c.id + "&moodlewsrestformat=json");
                    yield return www;
                    content = www.text;
                    por = new StringBuilder();
                    por.Append("{\"topics\":");
                    por.Append(content + "}");

                    v = JsonUtility.FromJson<Values>(por.ToString());
                    c.receiveCourseTopics(v.topics);
                }




                foreach (Course c in user.courses)
                {
                    www = new WWW(moodleUrl + "/webservice/rest/server.php" + "?wstoken=" + tokenMoodleSession + "&wsfunction=gradereport_user_get_grade_items&courseid=" + c.id + "&userid=" + user.id + "&groupid=0&moodlewsrestformat=json");
                    yield return www;
                    content = www.text;


                    v = JsonUtility.FromJson<Values>(content);
                    foreach (jsonValues.usergrades ug in v.usergrades)
                    {

                        c.receiveGrades(ug);
                    }

                }

                //c.filterfolios(content.Split('\n'));


                user.doneWriting(); // marca o final da captacao de dados do user


                www = new WWW(moodleUrl + "/webservice/rest/server.php" + "?wstoken=" + tokenMoodleSession + "&wsfunction=mod_quiz_get_quizzes_by_courses&courseids[0]=" + 5 + "&moodlewsrestformat=json");
                yield return www;
                content = www.text;
                
            }
        }
        else
        {
            //scrollview.changeText("Moodle indisponivel");
        }

    }

    // METODOS PARA FAZER RETRIEVE DE INFO DO MOODLE SEM O USO DE SOCKETS
    // CHAMADAS AS ESTES METODOS SAO DA FORMA: StartCoroutine("webGlRequests");

    IEnumerator StartUp()
    {
        yield return RetrieveToken();
        if(tokenMoodleSession != null)
        {
            // A Ordem eh importante
            yield return RetrieveUser();
            yield return RetrieveCourses();
            yield return RetrieveCourseGrades();
            yield return RetrieveCourseTopics();
            yield return RetrieveUserGrades();
            user.doneWriting(); // marca o final da captacao de dados do user
        }
        else
        {
            //scrollview.changeText("Login incorrecto");
        }
    }

    /**
     * Metodo que Comunica com o servidor para ir buscar o token necessario, nao utiliza sockets
     */
    IEnumerator RetrieveToken()
    {
        WWW www = new WWW(moodleUrl + "/login/token.php" + "?service=tvservice&username=" + userName + "&password=" + password);
        yield return www;
        String content = www.text;
        String[] variable = content.Split(new[] { "\"token\":\"" }, StringSplitOptions.None);
        if (variable.Length > 1)
        {
            variable = variable[1].Split(new[] { "\"" }, StringSplitOptions.None);
            if (variable.Length > 0)
            {
                tokenMoodleSession = variable[0];
            }
        }
    }


    /*
     * Metodo que comunica com o servidor e vai buscar a informacao do user, nao utiliza sockets
     */
    IEnumerator RetrieveUser()
    {
        WWW www = new WWW(moodleUrl + "/webservice/rest/server.php" + "?wstoken=" + tokenMoodleSession + "&wsfunction=core_user_get_users&criteria[0][key]=username&criteria[0][value]=" + userName + "&moodlewsrestformat=json");
        yield return www;
        String content = www.text;

        Values v = UnityEngine.JsonUtility.FromJson<Values>(content);
        user.receiveUsers(v.users[0]);
    }

    /**
     *  Metodo que comunica com o servidor e busca a informacao dos cursos a que o utilizador esta inscrito, nao utiliza sockets
     */
    IEnumerator RetrieveCourses()
    {
        WWW www = new WWW(moodleUrl + "/webservice/rest/server.php" + "?wstoken=" + tokenMoodleSession + "&wsfunction=core_enrol_get_users_courses&userid=" + user.id + "&moodlewsrestformat=json");
        yield return www;
        String content = www.text;

        StringBuilder por = new StringBuilder();
        por.Append("{\"courses\":");
        por.Append(content + "}");

        Values v = JsonUtility.FromJson<Values>(por.ToString());
        user.receiveCourses(v.courses);
    }

    IEnumerator RetrieveUserGrades()
    {
        WWW www = new WWW(moodleUrl + "/webservice/rest/server.php" + "?wstoken=" + tokenMoodleSession + "&wsfunction=gradereport_overview_get_course_grades&userid=" + user.id + "&moodlewsrestformat=json");
        yield return www;
        String content = www.text;

        Values v = UnityEngine.JsonUtility.FromJson<Values>(content);
        user.receiveGrades(v.grades);
    }

    IEnumerator RetrieveCourseTopics()
    {
        
        WWW www;
        String content;
        StringBuilder por;
        Values v;
        foreach (Course c in user.courses)
        {
            www = new WWW(moodleUrl + "/webservice/rest/server.php" + "?wstoken=" + tokenMoodleSession + "&wsfunction=core_course_get_contents&courseid=" + c.id + "&moodlewsrestformat=json");
            yield return www;
            content = www.text;
            por = new StringBuilder();
            por.Append("{\"topics\":");
            por.Append(content + "}");

            v = JsonUtility.FromJson<Values>(por.ToString());
            c.receiveCourseTopics(v.topics);
        }
        user.doneWriting();
    }

    IEnumerator RetrieveCourseGrades()
    {
        WWW www;
        String content;
        
        Values v;
        foreach (Course c in user.courses)
        {
            www = new WWW(moodleUrl + "/webservice/rest/server.php" + "?wstoken=" + tokenMoodleSession + "&wsfunction=gradereport_user_get_grade_items&courseid=" + c.id + "&userid=" + user.id + "&groupid=0&moodlewsrestformat=json");
            yield return www;
            content = www.text;


            v = JsonUtility.FromJson<Values>(content);
            foreach (jsonValues.usergrades ug in v.usergrades)
            {

                c.receiveGrades(ug);
            }

        }
    }

    private void startUpdateCheck()
    {
        StartCoroutine("hasUpdates");
    }

    /**
     * Metodo para verificar se houve actualizações desde o ultimo login, só serve para por em texto as novidades
     * TODO core_course_check_updates pode ter que ser utilizado para validar informação que afecte o user -> Check if there is updates affecting the user for the given course and contexts.
     * */
    IEnumerator checkNewInfo()
    {
        TimeSpan s = lastupdate - new DateTime(1970, 1, 1);
        Debug.Log("Verificar updates: " + cycle);
        cycle++;
        StringBuilder debrief = new StringBuilder();
        foreach (UserInfo.Course c in user.courses)
        {
            WWW www = new WWW(moodleUrl + "/webservice/rest/server.php" + "?wstoken=" + tokenMoodleSession + "&wsfunction=core_course_get_updates_since&courseid=" + c.id + "&since=" + (int)s.TotalSeconds + "&moodlewsrestformat=json");
            yield return www;
            String content = www.text;
            Values v = JsonUtility.FromJson<Values>(content);

            if(v.instances.Count> 0) // Ocurreu algo de novo
            {
                debrief.Append("For Course " + c.fullName + " :\n");
                foreach(jsonValues.instances i in v.instances)
                {
                    if (i.contextlevel.ToLower().Equals("module"))
                    {
                        Course.modules m = user.getCourse(c.id).getSpecificModule(i.id);
                        if(m == null)
                        {
                            debrief.Append("A module with the id " + i.id + " was removed\n");
                        }
                        else
                        {
                            debrief.Append("On Module " + m.name + " something happened\n");
                        }
                    }
                }
            }
            else if(v.instances.Count == 0)
            {
                debrief.Append("Course " + c.fullName + " has nothing to report\n\n");
            }
            debrief.Append("\n");
        }

        //scrollview.addText("\n\n" + debrief.ToString());
    }

    IEnumerator hasUpdates()
    {
        TimeSpan s = lastupdate - new DateTime(1970, 1, 1);
        Debug.Log("Verificar updates: " + cycle);
        cycle++;
        WWW www = new WWW(moodleUrl + "/webservice/rest/server.php" + "?wstoken=" + tokenMoodleSession + "&wsfunction=core_course_get_updates_since&courseid=" + 5 + "&since=" + (int)s.TotalSeconds + "&moodlewsrestformat=json");
        yield return www;
        String content = www.text;
        Values v = JsonUtility.FromJson<Values>(content);

        
        if (v.instances != null)
        {
            if (v.instances.Count > 0)
            {
                StartCoroutine(executeUpdates(v.instances,5));
                lastupdate = DateTime.UtcNow;
            }
        }
    }

    IEnumerator executeUpdates(List<jsonValues.instances> instances, int id)
    {
        user.needsWriting();
        //scrollview.changeText("A fazer updates");
        StringBuilder sb = new StringBuilder(), modu = new StringBuilder();
        foreach (jsonValues.instances i in instances)
        {
            // fazer updates aqui de acordo com o que pede, por agora manter assim
            yield return StartCoroutine("RetrieveCourseTopics");

            foreach (jsonValues.updates u in i.updates)
            {
                
                sb.Append(u.name + ",");
                if (u.name.ToLower().Equals("configuration"))
                {
                    Course.modules m = user.getCourse(id).getSpecificModule(i.id);
                    if (m != null)
                        modu.Append("TEM O NOME: " + m.name);
                    else
                        modu.Append("ERRO ID: " + i.id);
                    
                }

            }
            //scrollview.addText("Um(a) " + i.contextlevel + " com o id: " + i.id + " sofreu uma alteracao na " + sb.Remove(sb.Length - 1, 1).ToString() + "\n" + modu.ToString());
            
            
            sb = new StringBuilder();
            modu = new StringBuilder();
        }
    }



    /**
     * Metodo que Comunica com o servidor para ir buscar o token necessario a pedir os webservices com o uso de Sockets
     **/
    public void getToken()
    {
        CookieContainer cookie = new CookieContainer();


        Uri address = new Uri(moodleUrl + "/login/token.php" + "?service=tvservice&username=" + userName + "&password=" + password);

        String[] variable;
        try
        {
            HttpWebRequest requestB = WebRequest.Create(address) as HttpWebRequest;

            requestB.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36";
            // Set type to POST  
            requestB.Method = "POST";

            //request.ContentType = "application/x-www-form-urlencoded";
            requestB.Timeout = 700;
            //requestB.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";


            requestB.Headers.Add("Accept-Encoding", "gzip, deflate, sdch");
            requestB.Headers.Add("Accept-Language", "en-US,en;q=0.8,pt-PT;q=0.6,pt;q=0.4");

            requestB.AllowAutoRedirect = true;
            requestB.CookieContainer = cookie;


            StringBuilder data = new StringBuilder();



            // Create a byte array of the data we want to send  
            byte[] byteData = UTF8Encoding.UTF8.GetBytes(data.ToString()); //data.ToString()

            // Set the content length in the request headers  
            requestB.ContentLength = byteData.Length;
            //request.CookieContainer = new CookieContainer();

            // Write data  
            using (Stream postStream = requestB.GetRequestStream())
            {
                postStream.Write(byteData, 0, byteData.Length);
                //postStream.Flush();
                postStream.Close();

            }



            // Get response  
            using (HttpWebResponse responseB = requestB.GetResponse() as HttpWebResponse)
            {
                //rCookie = responseB.Cookies;
                //WebHeaderCollection header = responseB.Headers;


                // Get the response stream  
                StreamReader reader = new StreamReader(responseB.GetResponseStream());
                String content = reader.ReadToEnd();

                // GET Token from response
                variable = content.Split(new[] { "\"token\":\"" }, StringSplitOptions.None);
                variable = variable[1].Split(new[] { "\"" }, StringSplitOptions.None);
                tokenMoodleSession = variable[0];

                // Console application output  
                //Debug.Log(tokenMoodleSession);


                // To Write on pc DEBUG PURPOSES
                //System.IO.File.WriteAllText(@"C:\Users\***\Desktop\response.txt", content); // clears the file and writes
                //System.IO.File.WriteAllText(@"C:\Users\***\Desktop\response.html", content); // clears the file and writes
                //if (System.IO.File.Exists(@"C:\Users\***\Desktop\accounts.txt")) {
                //    System.IO.File.AppendAllText(@"C:\Users\***\Desktop\accounts.txt", "\n\nUserName: " + userName + "\n");
                //    System.IO.File.AppendAllText(@"C:\Users\***\Desktop\accounts.txt", "Password: " + password + "\n");
                //}
                //else {

                //    System.IO.File.Create(@"C:\Users\***\Desktop\accounts.txt");
                //    System.IO.File.AppendAllText(@"C:\Users\***\Desktop\accounts.txt", "UserName: " + userName + "\n");
                //    System.IO.File.AppendAllText(@"C:\Users\***\Desktop\accounts.txt", "Password: " + password + "\n");
                //} 



            }
        }
        catch (Exception e)
        {
            Debug.Log("Ocurreu um erro com a busca do token\n" +
                "Por favor, tente de novo. Visto que ocasionalmente a primeira não dá\n Obrigado\n" + e.Message);
        }

        if (tokenMoodleSession != null)
            getUserData();
    }



    public void getUserData()
    {
       
        // Usado para obter dados do user
        Uri address = new Uri(moodleUrl + "/webservice/rest/server.php" + "?wstoken=" + tokenMoodleSession + "&wsfunction=core_user_get_users&criteria[0][key]=username&criteria[0][value]=" + userName);



        // Create the web request  
        HttpWebRequest request = WebRequest.Create(address) as HttpWebRequest;

        // Set type to POST  
        request.Method = "POST";
        request.ContentType = "application/x-www-form-urlencoded";

        StringBuilder data = new StringBuilder();

        // Create a byte array of the data we want to send  
        byte[] byteData = UTF8Encoding.UTF8.GetBytes(data.ToString()); //data.ToString()

        // Set the content length in the request headers  
        request.ContentLength = byteData.Length;

        // Write data  
        using (Stream postStream = request.GetRequestStream())
        {
            postStream.Write(byteData, 0, byteData.Length);
        }

        // Get response  
        using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
        {
            // Get the response stream  
            StreamReader reader = new StreamReader(response.GetResponseStream());
            String content = reader.ReadToEnd();
            user.filterUserData(content);
            //Debug.Log(content);



        }
        //Debug.Log(user.id);
        //Usado para verificar os cursos que o user esta inscrito
        Uri address2 = new Uri(moodleUrl + "/webservice/rest/server.php" + "?wstoken=" + tokenMoodleSession + "&wsfunction=core_enrol_get_users_courses&userid=" + user.id);




        request = WebRequest.Create(address2) as HttpWebRequest;

        // Set type to POST  
        request.Method = "POST";
        request.ContentType = "application/x-www-form-urlencoded";

        data = new StringBuilder();

        // Create a byte array of the data we want to send  
        byteData = UTF8Encoding.UTF8.GetBytes(data.ToString()); //data.ToString()

        // Set the content length in the request headers  
        request.ContentLength = byteData.Length;

        // Write data  
        using (Stream postStream = request.GetRequestStream())
        {
            postStream.Write(byteData, 0, byteData.Length);
        }



        // Get response  
        using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
        {
            // Get the response stream  
            StreamReader reader = new StreamReader(response.GetResponseStream());
            String content = reader.ReadToEnd();
            user.filterCourseData(content);
            //Debug.Log(content);

        }

        //Usado para verificar as grades dos cursos que o user esta inscrito
        Uri address3 = new Uri(moodleUrl + "/webservice/rest/server.php" + "?wstoken=" + tokenMoodleSession + "&wsfunction=gradereport_overview_get_course_grades&userid=" + user.id);

        request = WebRequest.Create(address3) as HttpWebRequest;

        // Set type to POST  
        request.Method = "POST";
        request.ContentType = "application/x-www-form-urlencoded";

        data = new StringBuilder();

        // Create a byte array of the data we want to send  
        byteData = UTF8Encoding.UTF8.GetBytes(data.ToString()); //data.ToString()

        // Set the content length in the request headers  
        request.ContentLength = byteData.Length;

        // Write data  
        using (Stream postStream = request.GetRequestStream())
        {
            postStream.Write(byteData, 0, byteData.Length);
        }



        // Get response  
        using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
        {
            // Get the response stream  
            StreamReader reader = new StreamReader(response.GetResponseStream());
            String content = reader.ReadToEnd();
            //Debug.Log(content);
            user.filterGrades(content);

            // To Write on pc

            //System.IO.File.WriteAllText(@"C:\Users\***\Desktop\response.html", content); // clears the file and writes
            //if (System.IO.File.Exists(@"C:\Users\***\Desktop\accounts.txt")) {
            //    System.IO.File.AppendAllText(@"C:\Users\***\Desktop\accounts.txt", "\n\nUserName: " + userName + "\n");
            //    System.IO.File.AppendAllText(@"C:\Users\***\Desktop\accounts.txt", "Password: " + password + "\n");
            //}
            //else {

            //    System.IO.File.Create(@"C:\Users\***\Desktop\accounts.txt");
            //    System.IO.File.AppendAllText(@"C:\Users\***\Desktop\accounts.txt", "UserName: " + userName + "\n");
            //    System.IO.File.AppendAllText(@"C:\Users\***\Desktop\accounts.txt", "Password: " + password + "\n");
            //} 



        }


        address = new Uri(moodleUrl + "/webservice/rest/server.php" + "?wstoken=" + tokenMoodleSession + "&wsfunction=core_course_get_contents&courseid=" + "4");



        // Create the web request  
        request = WebRequest.Create(address) as HttpWebRequest;

        // Set type to POST  
        request.Method = "POST";
        request.ContentType = "application/x-www-form-urlencoded";

        data = new StringBuilder();

        // Create a byte array of the data we want to send  
        byteData = UTF8Encoding.UTF8.GetBytes(data.ToString()); //data.ToString()

        // Set the content length in the request headers  
        request.ContentLength = byteData.Length;

        // Write data  
        using (Stream postStream = request.GetRequestStream())
        {
            postStream.Write(byteData, 0, byteData.Length);
        }

        // Get response  
        using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
        {
            // Get the response stream  
            StreamReader reader = new StreamReader(response.GetResponseStream());
            String content = reader.ReadToEnd();
            //filter data
            //Debug.Log(content);

            foreach (Course c in user.courses)
                if (c.id == 4)
                {
                    c.filterCourse(content);
                   
                }

        }
        //testNewFunction();
        //scrollview.changeText(user.giveData());
        //scrollview.changeListHeader("Nº courses:" + user.courses.Count);
    }

    /**
     * Metodo usado para testar novos webservices a parte
     * */
    public void testNewFunction()
    {
        Uri address;
        HttpWebRequest request;
        StringBuilder data;
        byte[] byteData;



        address = new Uri(moodleUrl + "/webservice/rest/server.php" + "?wstoken=" + tokenMoodleSession + "&wsfunction=gradereport_user_get_grade_items&courseid=" + "5" + "&userid=" + user.id + "&groupid=0");



        // Create the web request  
        request = WebRequest.Create(address) as HttpWebRequest;

        // Set type to POST  
        request.Method = "POST";
        request.ContentType = "application/x-www-form-urlencoded";

        data = new StringBuilder();

        // Create a byte array of the data we want to send  
        byteData = UTF8Encoding.UTF8.GetBytes(data.ToString()); //data.ToString()

        // Set the content length in the request headers  
        request.ContentLength = byteData.Length;

        // Write data  
        using (Stream postStream = request.GetRequestStream())
        {
            postStream.Write(byteData, 0, byteData.Length);
        }

        // Get response  
        using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
        {
            // Get the response stream  
            StreamReader reader = new StreamReader(response.GetResponseStream());
            String content = reader.ReadToEnd();
            //filter data
            content.ToString();

        }

    }


    // METODOS PARA IR BUSCAR VALORES DADOS PELO USER
    public void getUserName()
    {
        InputField[] infs = GUIElement.FindObjectsOfType<InputField>();
        foreach (InputField inf in infs)
        {
            if (inf.name.Equals("InputUser"))
                userName = inf.text;
        }
    }

    public void getPassword()
    {
        InputField[] infs = GUIElement.FindObjectsOfType<InputField>();
        foreach (InputField inf in infs)
        {
            if (inf.name.Equals("InputPassword"))
                password = inf.text;
        }
    }


    public void writeToFileExample()
    {
        string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        string filename = Path.Combine(path, "myfile.txt");

        using (var streamWriter = new StreamWriter(filename, true))
        {
            streamWriter.WriteLine(DateTime.UtcNow);
        }

        using (var streamReader = new StreamReader(filename))
        {
            string content = streamReader.ReadToEnd();
            //System.Diagnostics.Debug.WriteLine(content);
            content.ToString();
        }
    }




}
