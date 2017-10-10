using System.Collections;
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

public class webserviceLogin : MonoBehaviour
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
    }
    
    


    String userName = "wstestuser";
    String password = "WS_TestUser1";
    String ID;
    String tokenMoodleSession = null;

    public UserData user;

    //HUDSize hud; ONDE VAI BUSCAR OS VALORES INSERIDOS PELO USER

    //dataflow scrollview; // ONDE VAI FAZER DISPLAY DA INFO

    String moodleUrl = "http://ec2-52-215-90-45.eu-west-1.compute.amazonaws.com/moodleFCUL";

    // Use this for initialization
    void Start()
    {
        //scrollview = gameObject.AddComponent(typeof(dataflow)) as dataflow;
        //hud = gameObject.AddComponent(typeof(HUDSize)) as HUDSize;
        StartCoroutine("webGlRequests");

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void press()
    {
        StartCoroutine("webGlRequests");
    }

    /**
     * Devido ao webgl nao suportar acesso directo as sockets que torna o System.Net invalido, foi necessario criar uma alternativa a solucao corrente
     * */
    IEnumerator webGlRequests()
    {
        
        WWW www = new WWW(moodleUrl + "/login/token.php" + "?service=tvservice&username=" + userName + "&password=" + password);
        yield return www;
        String content = www.text;
        
        String []variable = content.Split(new[] { "\"token\":\"" }, StringSplitOptions.None);
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

                //StringBuilder s = new StringBuilder();
                //int n = 0;
                //foreach (Course c in user.courses)
                //{
                //    s.Append("&courseids[" + n + "]=");
                //    s.Append(c.id);
                //    n++;
                //}
                //www = new WWW(moodleUrl+ "/webservice/rest/server.php" + "?wstoken=" + tokenMoodleSession + "&wsfunction=mod_assign_get_assignments" + s.ToString()); // usar o 5 como exemplo
                //yield return www;
                //content = www.text;
                ////System.IO.File.WriteAllText(@"C:\Users\***\Desktop\result.txt",Course.HtmlDecode(content));
                


                //scrollview.changeText(user.giveData());
            }
        }
        else
        {
            //scrollview.changeText("Moodle indisponivel");
        }

    }


    public void getToken()
    {
        CookieContainer cookie = new CookieContainer();
        CookieCollection rCookie = new CookieCollection();
        
        Uri address = new Uri(moodleUrl + "/login/token.php" + "?service=tvservice&username=" + userName + "&password="+password);
        
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
        }catch(Exception e)
        {
            Debug.Log("Ocurreu um erro com a busca do token\n" +
                "Por favor, tente de novo. Visto que ocasionalmente a primeira não dá\n Obrigado\n" + e.Message);
        }
        
        if (tokenMoodleSession != null)
            getUserData();
    }



    public void getUserData()
    {
        //   Dictionary<string, string> myarray =
        //new Dictionary<string, string>();

        //   myarray.Add("0", "Number1");
        //   myarray.Add("1", "Hello World");

        // test bed token: ffcad039f3f36de486a755bd88505e3f
        //moodle test token : e89a8bebb9cf153e669f2544ce8d995c

        // Usado para obter dados do user
        Uri address = new Uri(moodleUrl + "/webservice/rest/server.php" + "?wstoken="+ tokenMoodleSession + "&wsfunction=core_user_get_users&criteria[0][key]=username&criteria[0][value]="+ userName);

       
       
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
            Course chosen=null;
            foreach (Course c in user.courses)
                if (c.id == 4)
                {
                    c.filterCourse(content);
                    chosen = c;
                }
            


        }
       //testNewFunction();
        //scrollview.changeText(user.giveData());
        //scrollview.changeListHeader("Nº courses:" + user.courses.Count);
    }

    public void testNewFunction()
    {
        Uri address;
        HttpWebRequest request;
        StringBuilder data;
        byte[] byteData;



        address = new Uri(moodleUrl + "/webservice/rest/server.php" + "?wstoken=" + tokenMoodleSession + "&wsfunction=gradereport_user_get_grade_items&courseid=" + "5" + "&userid="+ user.id+"&groupid=0");


      
        // Create the web request  
        request= WebRequest.Create(address) as HttpWebRequest;

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
            

        }

    }

    
    // METODOS PARA IR BUSCAR VALORES DADOS PELO USER
    //public void getUserName()
    //{
    //    InputField[] infs= GUIElement.FindObjectsOfType<InputField>();   
    //    foreach ( InputField inf in infs)
    //    {
    //        if ((inf.name.Equals("InputUser") && hud.landscape.isActiveAndEnabled) || (inf.name.Equals("InputUser P") && hud.portrait.isActiveAndEnabled))
    //            userName = inf.text;
    //    }
    //}

    //public void getPassword()
    //{
    //    InputField[] infs = GUIElement.FindObjectsOfType<InputField>();
    //    foreach (InputField inf in infs)
    //    {
    //        if ((inf.name.Equals("InputPassword") && hud.landscape.isActiveAndEnabled) || (inf.name.Equals("InputPassword P") && hud.portrait.isActiveAndEnabled))
    //            password = inf.text;
    //    }
    //}

    //public void getID()
    //{
        
    //    InputField[] infs = GUIElement.FindObjectsOfType<InputField>();
       
    //    foreach (InputField inf in infs)
    //    {
           
    //        if ((inf.name.Equals("ID") && hud.landscape.isActiveAndEnabled) || (inf.name.Equals("ID P") && hud.portrait.isActiveAndEnabled))
    //           ID = inf.text;
    //    }
    //}

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
        }
    }



    /*
     * Metodo usado para pedir informacao ao servico sobre um certo id expecificado no inputfield
     * */
    public void connectWithId()
    {
        //Debug.Log("Button pressed");
        //   Dictionary<string, string> myarray =
        //new Dictionary<string, string>();

        //   myarray.Add("0", "Number1");
        //   myarray.Add("1", "Hello World");


        Uri address = new Uri("http://moodle.lead.uab.pt/projetos/webservice/rest/server.php" + "?wstoken=e89a8bebb9cf153e669f2544ce8d995c" + "&wsfunction=core_user_get_users_by_id&userids[0]="+ID);

        // Create the web request  
        HttpWebRequest request = WebRequest.Create(address) as HttpWebRequest;

        // Set type to POST  
        request.Method = "POST";
        request.ContentType = "application/x-www-form-urlencoded";
        // request.Credentials = new NetworkCredential("admin", "#1AdminUser");
        // Create the data we want to send  
        // string appId = "YahooDemo";
        //  string context = "Italian sculptors and painters of the renaissance"
        //                     + "favored the Virgin Mary for inspiration";
        // string query = "madonna";
        // HttpContext.Current.Server.UrlEncode();


        StringBuilder data = new StringBuilder();
        //data.Append("username=admin");
        // data.Append("&password=#1AdminUser");
        //data.Append("&moodlewsrestformat=json");
        // data.Append(urlParameters);

        //  data.Append("&query=" + HttpUtility.UrlEncode(query));

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
            // Console application output  
            //GUIElement.FindObjectsOfType<>();

            //filtrar dados
            //System.IO.File.WriteAllText(@"C:\Users\****\Desktop\response.txt", content); // clears the file and writes
            user.filterUserData(content);
            //scrollview.changeText(user.giveData());
            
            //Debug.Log("Writing done");
        }

    }





    // METODO SEM PROPOSITO ATUAL
    public bool MyRemoteCertificateValidationCallback(System.Object sender,X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
    {
        bool isOk = true;
        // If there are errors in the certificate chain,
        // look at each error to determine the cause.
        if (sslPolicyErrors != SslPolicyErrors.None)
        {
            for (int i = 0; i < chain.ChainStatus.Length; i++)
            {
                if (chain.ChainStatus[i].Status == X509ChainStatusFlags.RevocationStatusUnknown)
                {
                    continue;
                }
                chain.ChainPolicy.RevocationFlag = X509RevocationFlag.EntireChain;
                chain.ChainPolicy.RevocationMode = X509RevocationMode.Online;
                chain.ChainPolicy.UrlRetrievalTimeout = new TimeSpan(0, 1, 0);
                chain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllFlags;
                bool chainIsValid = chain.Build((X509Certificate2)certificate);
                if (!chainIsValid)
                {
                    isOk = false;
                    break;
                }
            }
        }
        return isOk;
    }



}
