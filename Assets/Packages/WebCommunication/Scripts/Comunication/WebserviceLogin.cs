using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text;
using System;
using UserInfo;


public class WebserviceLogin : MonoBehaviour
{
    // https://moodle.org/mod/forum/discuss.php?d=227169
    [Serializable]
    public class Values
    {
        public List<jsonValues.users> users;
        //public List<warnings> warnings;

        // course
        public List<jsonValues.Courses> courses;
        public List<jsonValues.Grades> grades;
        
        public List<jsonValues.Topics> topics;
        public List<jsonValues.assignments> assignments;
        public List<jsonValues.usergrades> usergrades;
        public List<jsonValues.groups> groups;
        //quizzes
        public List<jsonValues.quizzes> quizzes; // TODO IMPLEMENT
        //update related
        public List<jsonValues.instances> instances;
        // forum related
        public List<jsonValues.forums> forums;
        public List<jsonValues.discussions> discussions;
        public List<jsonValues.posts> posts;
        // notes related
        public List<jsonValues.notes> notes;
    }
    

    DateTime lastupdate;

    //String userName = "ricardo";
    //String password ="WS_TestUser1";

    //String token//MoodleSession = "d814dcb92eff420d14bd3574de0a3b3a";
    String tutorToken = "14ab94c8af25f6b426fc61cde1ed090b";
    int tutorID;
    public Boolean userVerified = false;
    String userToken ="14ab94c8af25f6b426fc61cde1ed090b";

    DataManager dataM;

    

    //HUDSize hud; ONDE VAI BUSCAR OS VALORES INSERIDOS PELO USER


    String moodleUrl = "http://ec2-52-211-160-228.eu-west-1.compute.amazonaws.com/moodleFCUL";

    int cycle = 0; // saber o nº de vezes que tentou ver actualizações

    // usado para separar multiplas cadeiras de so uma
    public Boolean multi = false;
    int courseId=0;

    public void selectionMulti()
    {
        multi = !multi;
    }

    // Use this for initialization
    void Start()
    {
        //hud = gameObject.AddComponent(typeof(HUDSize)) as HUDSize;
        //StartCoroutine("webGlRequests");
        //StartCoroutine("RetrieveTutorID");
        
    }

    DateTime begining;
    public void compareTime()
    {

        TimeSpan span = DateTime.UtcNow.Subtract(begining);
        StringBuilder timeString = new StringBuilder();
        if (span.Minutes > 0)
        {
            timeString.Append(span.Minutes + "minutos ");
        }
        timeString.Append(span.Seconds + "segundos");
        Debug.Log("Differença de tempo: " + timeString.ToString());

    }

    public void findDataManager()
    {
        GameObject moodleLogin = GameObject.Find("moodleLogin");
        dataM = moodleLogin.GetComponent(typeof(DataManager)) as DataManager;
    }
    

    int npoints = 0;
    DateTime last = DateTime.UtcNow, current, lastCheck;
    TimeSpan s;
    Boolean wasPressed = false;
    // Update is called once per frame
    void Update()
    {
        if(dataM != null)
        if (dataM.getUser().readyForRead && npoints !=1)
        {
            
            compareTime();
            npoints=1;
        }

        //if (scrollview.giveText().Contains("A carregar informação") && dataM.getUser().readyForRead)
        //{
        //    scrollview.changeText(dataM.getUser().giveData());
        //    StartCoroutine("checkNewInfo");

        //}


        //TimeSpan span = DateTime.UtcNow.Subtract(lastCheck);

        //if (span.TotalSeconds > 60 && dataM.getUser().readyForRead)
        //{
        //    StartCoroutine("hasUpdates");
        //    lastCheck = DateTime.UtcNow;
        //}

    }

    

    public void press()
    {
        if (!wasPressed)
        {
            wasPressed = true;
            StartCoroutine("StartUp");
            
            lastupdate = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc).AddSeconds(dataM.getUser().datelast);
            // consequentes updates
            lastupdate = DateTime.UtcNow;
            lastCheck = DateTime.UtcNow;
        }
    }

    /**
     * Devido ao webgl nao suportar acesso directo as sockets que torna o System.Net invalido, foi necessario criar uma alternativa a solucao corrente
     * */

    // METODOS PARA FAZER RETRIEVE DE INFO DO MOODLE SEM O USO DE SOCKETS
    // CHAMADAS AS ESTES METODOS SAO DA FORMA: StartCoroutine("webGlRequests");

    public Boolean beginConnection()
    {
        
        StartCoroutine("StartUp");
        lastupdate = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc).AddSeconds(dataM.getUser().datelast);

        // consequentes updates
        lastupdate = DateTime.UtcNow;
        lastCheck = DateTime.UtcNow;
        return userVerified;
    }


    //public Boolean beginConnectionWithMobile(String username, string password)
    //{
       
    //    begining = DateTime.UtcNow;
        
    //    StartCoroutine(getUserDataMobile(username, password));
        
    //    return userVerified;
    //}

    //public Boolean beginConnectionWithId(int id, int courseId)
    //{

    //    dataM.getUser().id = id;
    //    if (!multi)
    //    {
    //        this.courseId = courseId;
    //    }
    //    begining = DateTime.UtcNow;
    //    StartCoroutine("getUserData");

    //    return userVerified;
    //}

    public Boolean beginConnection(Dictionary<String,object> values)
    {
        findDataManager();
        begining = DateTime.UtcNow;
#if UNITY_ANDROID
         StartCoroutine(getUserDataMobile(Convert.ToString(values["username"]), Convert.ToString(values["password"])));
#else
        dataM.getUser().id = Convert.ToInt32(values["userId"]);
        if (!multi)
        {
            this.courseId = Convert.ToInt32(values["courseId"]);
        }
        StartCoroutine(getUserData());
        
#endif
        return userVerified;
    }


    IEnumerator getUserDataMobile(String username, string password) // Metodo que organiza a busca de informação feita na aplicação mobile
    {
        
        yield return testLogin(username, password);
        if (userVerified)
        {

            //yield return RetrieveToken(username, password);
            yield return retrieveUser(username);
            yield return RetrieveCourses(); // busca as cadeiras que o aluno esta inscrito, tem em conta se são varias cadeiras
            
            retrieveCourseGroups();
            yield return RetrieveCourseTopics();

            //yield return getCourseNotes();
            yield return RetrieveCourseGrades();
            RetrieveUserGrades();
            RetrieveForumData();
            lastupdate = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc).AddSeconds(dataM.getUser().datelast);

            dataM.getUser().doneWriting(); // marca o final da captacao de dados do user
        }
    }

    IEnumerator getUserData() // Metodo que organiza a busca de informação feita na aplicação WebGl
    {
       
        StartCoroutine(retrieveUser("")); // não eh preciso autenticar ou verificar o aluno, assumindo que este metodo eh soh chamado no WebGl dentro do moodle
        
        yield return RetrieveCourses(); // busca as cadeiras que o aluno esta inscrito, tem em conta se são varias cadeiras

        retrieveCourseGroups();
        yield return RetrieveCourseTopics();

        getCourseNotes();
        yield return RetrieveCourseGrades();
        RetrieveUserGrades();
        yield return RetrieveForumData();
        
        lastupdate = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc).AddSeconds(dataM.getUser().datelast);
        dataM.getUser().doneWriting(); // marca o final da captacao de dados do user

        
        
    }

    /**
     * Metodo que Comunica com o servidor para ir buscar o token necessario, nao utiliza sockets
     */
    IEnumerator RetrieveToken(String username, String password)
    {
        WWW www = new WWW(moodleUrl + "/login/token.php" + "?service=TVService&username=" + username + "&password=" + password);
        yield return www;
        String content = www.text;
        String[] variable = content.Split(new[] { "\"token\":\"" }, StringSplitOptions.None);
        if (variable.Length > 1)
        {
            variable = variable[1].Split(new[] { "\"" }, StringSplitOptions.None);
            if (variable.Length > 0)
            {
                userVerified = true;
                userToken = variable[0];

            }
        }
    }

    /*
     * Metodo que comunica com o servidor e vai buscar a informacao do user, nao utiliza sockets
     */
    IEnumerator retrieveUser(String username)
    {

#if UNITY_ANDROID
        WWW www = new WWW(moodleUrl + "/webservice/rest/server.php" + "?wstoken=" + userToken + "&wsfunction=core_user_get_users&criteria[0][key]=username&criteria[0][value]=" + username + "&moodlewsrestformat=json");
#else
        //Debug.Log(moodleUrl + "/webservice/rest/server.php" + "?wstoken=" + tutorToken + "&wsfunction=core_user_get_users&criteria[0][key]=id&criteria[0][value]=" + dataM.getUser().id + "&moodlewsrestformat=json");
        WWW www = new WWW(moodleUrl + "/webservice/rest/server.php" + "?wstoken=" + tutorToken + "&wsfunction=core_user_get_users&criteria[0][key]=id&criteria[0][value]=" + dataM.getUser().id + "&moodlewsrestformat=json");
#endif
        yield return www;
        String content = www.text;
        
        Values v = UnityEngine.JsonUtility.FromJson<Values>(content);

        if (v.users.Count>0)
        {
            dataM.getUser().receiveUsers(v.users[0]);
             
            userVerified = true;
        }
        else
            userVerified = false;
        
    }

    public IEnumerator testLogin(String Username, string Password)
    {
        string formUrl = "http://ec2-52-211-160-228.eu-west-1.compute.amazonaws.com/moodleFCUL/login/index.php";

        WWWForm loginFields = new WWWForm();
        
        loginFields.AddField("username", Username);
        loginFields.AddField("password", Password);

        WWW www = new WWW(formUrl, loginFields.data);

        yield return www;

        String pageSource = www.text;
        
        if (pageSource.Contains("<title>Dashboard</title>"))
            userVerified = true;
        else
        {
            userVerified = false;
        }
    }

    /*
     * Metodo para obter o ID to tutor, usado para as notes 
     */
    IEnumerator RetrieveTutorID()
    {
#if UNITY_ANDROID
        String token = userToken;
#else
        String token = tutorToken;
#endif
        WWW www = new WWW(moodleUrl + "/webservice/rest/server.php" + "?wstoken=" + token + "&wsfunction=core_user_get_users&criteria[0][key]=username&criteria[0][value]=" + "tutor_virtual" + "&moodlewsrestformat=json");
        yield return www;
        String content = www.text;

        Values v = UnityEngine.JsonUtility.FromJson<Values>(content);
        tutorID = v.users[0].id;
    }
    /**
     *  Metodo que comunica com o servidor e busca a informacao dos cursos a que o utilizador esta inscrito, nao utiliza sockets
     */
    IEnumerator RetrieveCourses()
    {
#if UNITY_ANDROID
        String token = userToken;
#else
        String token = tutorToken;
#endif

        WWW www = new WWW(moodleUrl + "/webservice/rest/server.php" + "?wstoken=" + token + "&wsfunction=core_enrol_get_users_courses&userid=" + dataM.getUser().id + "&moodlewsrestformat=json");
        yield return www;
        String content = www.text;

        StringBuilder por = new StringBuilder();
        por.Append("{\"courses\":");
        por.Append(content + "}");

        Values v = JsonUtility.FromJson<Values>(por.ToString());
        dataM.receiveCourses(v.courses,multi,courseId);
        
        foreach (Course c in dataM.getCourses())
        {

            StartCoroutine(RetrieveUsersInCourse(c.id));
        }
    }

    IEnumerator RetrieveUsersInCourse(int courseId)
    {

#if UNITY_ANDROID
        String token = userToken;
#else
        String token = tutorToken;
#endif
        WWW www = new WWW(moodleUrl + "/webservice/rest/server.php" + "?wstoken=" + token + "&wsfunction=core_enrol_get_enrolled_users&courseid="+courseId+"&moodlewsrestformat=json");
        yield return www;
        String content = www.text;
        
        StringBuilder por = new StringBuilder();
        por.Append("{\"users\":");
        por.Append(content + "}");
        Values v = UnityEngine.JsonUtility.FromJson<Values>(por.ToString());
        //tutorID = v.users[0].id;
        //foreach(jsonValues.users u in v.users)
        //{
        //    Debug.Log(u.id);
        //} 
    }

    IEnumerator RetrieveUserGrades() // gradereport_overview_get_course_grades, vai buscar as notas do aluno relativamente a cadeira no geral
    {
#if UNITY_ANDROID
        String token = userToken;
#else
        String token = tutorToken;
#endif
        WWW www = new WWW(moodleUrl + "/webservice/rest/server.php" + "?wstoken=" + token + "&wsfunction=gradereport_overview_get_course_grades&userid=" + dataM.getUser().id + "&moodlewsrestformat=json");
        yield return www;
        String content = www.text;

        Values v = UnityEngine.JsonUtility.FromJson<Values>(content);
        dataM.receiveGrades(v.grades);
    }

    IEnumerator RetrieveCourseTopics()
    {
#if UNITY_ANDROID
        String token = userToken;
#else
        String token = tutorToken;
#endif
        WWW www;
        String content;
        StringBuilder por;
        Values v;

        foreach (Course c in dataM.getCourses())
        {
            www = new WWW(moodleUrl + "/webservice/rest/server.php" + "?wstoken=" + token + "&wsfunction=core_course_get_contents&courseid=" + c.id + "&moodlewsrestformat=json"); // NAO sera utilizado de forma primaria para identificar conteudo na cadeira, eh necessario avaliar o aluno de acordo com o grupo a que pertence (avaliacao continua)
            yield return www;
            content = www.text;
            por = new StringBuilder();
            por.Append("{\"topics\":");
            por.Append(content + "}");
            
            v = JsonUtility.FromJson<Values>(por.ToString());
            c.receiveCourseTopics(v.topics);
            
        }
      
    }

    IEnumerator RetrieveCourseGrades() // mod_assign_get_assignments, busca as tentativas (attemptgrades)
    {
#if UNITY_ANDROID
        String token = userToken;
#else
        String token = tutorToken;
#endif
        WWW www;
        String content;
        StringBuilder sb;
        int count = 0;
        Values v;
        
        foreach (Course c in dataM.getCourses())
        {
            www = new WWW(moodleUrl + "/webservice/rest/server.php" + "?wstoken=" + token + "&wsfunction=mod_assign_get_assignments&courseids[0]=" + c.id + "&moodlewsrestformat=json");
            yield return www;
            content = www.text;

            v = JsonUtility.FromJson<Values>(content);
            c.receiveAssignments(v.courses[0].assignments);

            sb = new StringBuilder();
            count = 0;
            foreach (UserInfo.Course.Folio f in c.folios)
            {
                sb.Append("&assignmentids[" + count + "]=" + f.id);
                count++;
            }
            www = new WWW(moodleUrl + "/webservice/rest/server.php" + "?wstoken=" + token + "&wsfunction=mod_assign_get_grades" + sb.ToString() + "&moodlewsrestformat=json");
            yield return www;
            content = www.text;

            v = JsonUtility.FromJson<Values>(content);
            c.receiveAssignmentsGrade(v.assignments, dataM.getUser().id);


            www = new WWW(moodleUrl + "/webservice/rest/server.php" + "?wstoken=" + tutorToken + "&wsfunction=gradereport_user_get_grade_items&courseid=" + c.id + "&userid=" + dataM.getUser().id + "&groupid=0&moodlewsrestformat=json");
            yield return www;
            content = www.text;
            
            v = JsonUtility.FromJson<Values>(content);
            foreach (jsonValues.usergrades ug in v.usergrades)
            {

                c.receiveGrades(ug);
            }
            c.currentAprov = c.currentAproveitamento();
            c.maxCurrentAprov = c.maxCurrentAproveitamento();
            Debug.Log(c.fullName + " Aproveitamento: " +c.currentAprov + " out of " + c.maxCurrentAprov + " " + (Convert.ToDouble(c.currentAprov) / Convert.ToDouble(c.maxCurrentAprov))*100 + "%");
        }
    }

    IEnumerator RetrieveForumData()
    {
#if UNITY_ANDROID
        String token = userToken;
#else
        String token = tutorToken;
#endif
        String content;
        StringBuilder por;
        Values v;
        WWW www;
        // report_competency_data_for_report
        foreach (Course c in dataM.getCourses())
        {
            www = new WWW(moodleUrl + "/webservice/rest/server.php" + "?wstoken=" + token + "&wsfunction=mod_forum_get_forums_by_courses&courseids[0]=" + c.id + "&moodlewsrestformat=json");
            yield return www;
            content = www.text;
            por = new StringBuilder();
            por.Append("{\"forums\":");
            por.Append(content + "}");

            v = JsonUtility.FromJson<Values>(por.ToString());

            dataM.getCourseById(c.id).receiveForums(v.forums);

            foreach (UserInfo.Course.Forum f in dataM.getCourseById(c.id).forums)
            {

                www = new WWW(moodleUrl + "/webservice/rest/server.php" + "?wstoken=" + token + "&wsfunction=mod_forum_get_forum_discussions_paginated&forumid=" + f.id + "&moodlewsrestformat=json"); // vem organizados pelo mais recentemente alterado
                yield return www;
                content = www.text;
                v = JsonUtility.FromJson<Values>(content);
                dataM.getCourseById(c.id).receiveDiscussions(v.discussions, f.cmid);


                foreach (UserInfo.Course.Discussions d in f.discussions)
                {
                    www = new WWW(moodleUrl + "/webservice/rest/server.php" + "?wstoken=" + token + "&wsfunction=mod_forum_get_forum_discussion_posts&discussionid=" + d.id + "&moodlewsrestformat=json"); // vem organizados pelo mais recentemente alterado
                    yield return www;
                    content = www.text;
                    v = JsonUtility.FromJson<Values>(content);
                    dataM.getCourseById(c.id).receivePosts(v.posts, f.id, d.id);

                }
               
            }

        }
    }

        public void startUpdateCheck(int timestamp)
    {
        StartCoroutine(checkNewInfo(timestamp));
    }

    /**
     * Metodo para verificar se houve actualizações desde o ultimo login, só serve para por em texto as novidades
     * TODO core_course_check_updates pode ter que ser utilizado para validar informação que afecte o user -> Check if there is updates affecting the user for the given course and contexts.
     * */
    IEnumerator checkNewInfo(int timestamp)
    {

#if UNITY_ANDROID
        String token = userToken;
#else
        String token = tutorToken;
#endif
        TimeSpan s = lastupdate - new DateTime(1970, 1, 1);
        UnityEngine.Debug.Log("Verificar updates: " + cycle);
        cycle++;
        StringBuilder debrief = new StringBuilder();
        WWW www; String content; Values v;


            foreach (UserInfo.Course c in dataM.getCourses())
            {
            //UnityEngine.Debug.Log(moodleUrl + "/webservice/rest/server.php" + "?wstoken=" + token + "&wsfunction=core_course_get_updates_since&courseid=" + c.id + "&since=" + (int)s.TotalSeconds + "&moodlewsrestformat=json");
                www = new WWW(moodleUrl + "/webservice/rest/server.php" + "?wstoken=" + token + "&wsfunction=core_course_get_updates_since&courseid=" + c.id + "&since=" + (int)s.TotalSeconds + "&moodlewsrestformat=json");
                yield return www;
                content = www.text;
                v = JsonUtility.FromJson<Values>(content);

                if (v.instances.Count > 0) // Ocurreu algo de novo
                {
                //debrief.Append("For Course " + c.fullName + " :\n");
                //foreach (jsonValues.instances i in v.instances)
                //{
                //    if (i.contextlevel.ToLower().Equals("module"))
                //    {
                //        Course.modules m = dataM.getCourseById(c.id).getSpecificModule(i.id);
                //        if (m == null)
                //        {
                //            debrief.Append("A module with the id " + i.id + " was removed\n");
                //        }
                //        else
                //        {
                //            debrief.Append("On Module " + m.name + " something happened\n");
                //        }
                //    }
                //    if (i.contextlevel.ToLower().Equals("topic"))
                //    {
                //        UserInfo.Course.Topic t = dataM.getCourseById(c.id).GetTopic(i.id);
                //        if (t == null)
                //        {
                //            debrief.Append("A Topic with the id " + i.id + " was removed\n");
                //        }
                //        else
                //        {
                //            debrief.Append("On Topic " + t.name + " something happened\n");
                //        }

                //    }
                //}
                c.receiveUpdates(v.instances,dataM.getUser().datelast);
                }
                else if (v.instances.Count == 0)
                {
                    debrief.Append("Course " + c.fullName + " has nothing to report\n\n");
                }
                debrief.Append("\n");
            }

    }

    IEnumerator hasUpdates()
    {
#if UNITY_ANDROID
        String token = userToken;
#else
        String token = tutorToken;
#endif
        TimeSpan s = lastupdate - new DateTime(1970, 1, 1);
        UnityEngine.Debug.Log("Verificar updates: " + cycle);
        cycle++;
        Boolean happened = false;
        WWW www; String content; Values v;

        foreach (Course c in dataM.getCourses())
        {
            www = new WWW(moodleUrl + "/webservice/rest/server.php" + "?wstoken=" + token + "&wsfunction=core_course_get_updates_since&courseid=" + c.id + "&since=" + (int)s.TotalSeconds + "&moodlewsrestformat=json");
            yield return www;
            content = www.text;
            v = JsonUtility.FromJson<Values>(content);


            if (v.instances != null)
            {
                if (v.instances.Count > 0)
                {
                    StartCoroutine(executeUpdates(v.instances, c.id));
                    happened = true;
                }
            }
        }
    }

    IEnumerator executeUpdates(List<jsonValues.instances> instances, int id)
    {
        dataM.getUser().needsWriting();
        
        StringBuilder sb = new StringBuilder(), modu = new StringBuilder();
        object m;

        foreach (jsonValues.instances i in instances)
        {
            // fazer updates aqui de acordo com o que pede, por agora manter assim
            yield return StartCoroutine("RetrieveCourseTopics");

            foreach (jsonValues.updates u in i.updates)
            {
                
                sb.Append(u.name + ",");
                if (u.name.ToLower().Equals("configuration"))
                {
                    m = dataM.getCourseById(id).getUndefinedModule(i.id);

                    if (m != null)
                    {
                        if (m.GetType().Equals(typeof(UserInfo.Course.modules)))
                        {
                            UserInfo.Course.modules mod = (UserInfo.Course.modules)m;
                            modu.Append("TEM O NOME: " + mod.name);
                        }


                    }
                    else
                        modu.Append("ERRO ID: " + i.id);
                    
                }

            }
            Debug.Log("Um(a) " + i.contextlevel + " com o id: " + i.id + " sofreu uma alteracao na " + sb.Remove(sb.Length - 1, 1).ToString() + "\n" + modu.ToString());
            
            
            sb = new StringBuilder();
            modu = new StringBuilder();
        }
        dataM.getUser().doneWriting();
    }

    IEnumerator getCourseNotes() // USADO NUM AMBIENTE DE TESTE
    {
#if UNITY_ANDROID
        String token = userToken;
#else
        String token = tutorToken;
#endif
        StringBuilder por;
        String content;
        Values v;
        
        foreach (Course c in dataM.getCourses())
        {
            WWW www = new WWW(moodleUrl + "/webservice/rest/server.php" + "?wstoken=" + token + "&wsfunction=core_notes_get_course_notes&userid=" + tutorID +
                "&courseid=" + c.id + "&moodlewsrestformat=json");
            yield return www;
            content = www.text;

            por = new StringBuilder();
            por.Append("{\"notes\":[");
            por.Append(content + "]}");
            
            v = JsonUtility.FromJson<Values>(por.ToString());
            c.notes.receiveNotes(v.notes,dataM.getUser().id);

        }
    }

    IEnumerator retrieveCourseGroups()
    {
#if UNITY_ANDROID
        String token = userToken;
#else
        String token = tutorToken;
#endif
        WWW www;
        String content;
        Values v;
        foreach (UserInfo.Course c in dataM.getCourses())
        {
            www = new WWW(moodleUrl + "/webservice/rest/server.php" + "?wstoken=" + token + "&wsfunction=core_group_get_course_user_groups&courseid=" +c.id+ "&userid=" + dataM.getUser().id + "&moodlewsrestformat=json");
            yield return www;
            content = www.text;

            v = JsonUtility.FromJson<Values>(content);
            c.receiveGroups(v.groups);
        }
    }

    /**
    * Metodo usado para testar novos webservices a parte
    * http://ec2-52-215-90-45.eu-west-1.compute.amazonaws.com/moodleFCUL/webservice/rest/server.php?wstoken=14ab94c8af25f6b426fc61cde1ed090b&wsfunction=core_group_get_course_user_groups&courseid=6&userid=3&moodlewsrestformat=json
    * http://ec2-52-215-90-45.eu-west-1.compute.amazonaws.com/moodleFCUL/webservice/rest/server.php?wstoken=14ab94c8af25f6b426fc61cde1ed090b&wsfunction=core_notes_get_course_notes&courseid=5&userid=6&moodlewsrestformat=json
    * */
    IEnumerator testNewFunction()
    {
#if UNITY_ANDROID
        String token = userToken;
#else
        String token = tutorToken;
#endif
        UnityEngine.Debug.Log("NEW FUNCTION");

        WWW www = new WWW(moodleUrl + "/webservice/rest/server.php" + "?wstoken=" + token + "&wsfunction=core_user_get_course_user_profiles&userlist[0][userid]=" + dataM.getUser().id + "&userlist[0][courseid]=" + courseId);
        yield return www;
        String content = www.text;
        UnityEngine.Debug.Log(content);

    }

    public void EnviarNota() // USADO NUM AMBIENTE DE TESTE
    {
        Boolean duvida=false, resolvido=false, existe;
        Toggle[] infs = GUIElement.FindObjectsOfType<Toggle>();
        String texto=null;
        foreach (Toggle inf in infs)
        {
            if (inf.name.Equals("Duvida/Questao"))
            {
                if (inf.isOn)
                    duvida = true;
            }

            if (inf.name.Equals("Resolvido"))
            {
                if (inf.isOn)
                    resolvido = true;
            }

        }
        InputField[] textos = GUIElement.FindObjectsOfType<InputField>();
        foreach (InputField inf in textos)
        {
            if (inf.name.Equals("DadosPergunta"))
                texto = inf.text;
        }

        
        existe = dataM.getCourses()[0].notes.existNote(dataM.getUser().id);// TODO TEMPORARIO

        StartCoroutine(newQuestion(duvida,texto,resolvido,existe));
        
    }

    IEnumerator newQuestion(Boolean duvida,String textoDuvida, Boolean resolvido, Boolean existe) // USADO NUM AMBIENTE DE TESTE
    {
        StringBuilder por = new StringBuilder();
#if UNITY_ANDROID
        String token = userToken;
#else
        String token = tutorToken;
#endif

        if (!existe)
        {
            por.AppendLine(dataM.getUser().id + " ");
            por.AppendLine("QUESTOES:");
            por.AppendLine(textoDuvida + " Duvida:" + duvida + " Resolvido:" + resolvido);
            UnityEngine.Debug.Log("a criar nota");
            UnityEngine.Debug.Log(por.ToString());
            UnityEngine.Debug.Log(tutorToken);
            UnityEngine.Debug.Log(tutorID);
            WWW www = new WWW(moodleUrl + "/webservice/rest/server.php" + "?wstoken=" + token + "&wsfunction=core_notes_create_notes&notes[0][userid]=" + tutorID + "&notes[0][publishstate]=personal"
                        + "&notes[0][courseid]=5" + "&notes[0][text]=" + por.ToString() + " " + "&notes[0][format]=2" + "&moodlewsrestformat=json");
            yield return www;
            
            String content = www.text;
           
        }

        else
        {
            UnityEngine.Debug.Log("Actualizar nota");
            UserInfo.Notes.Note n = dataM.getCourses()[0].notes.getNote(dataM.getUser().id); // TODO TEMPORARIO
            por.Append(Course.HtmlDecode(n.content));
            por.AppendLine(textoDuvida + " Duvida:" + duvida + " Resolvido:" + resolvido);
            WWW www = new WWW(moodleUrl + "/webservice/rest/server.php" + "?wstoken=" + tutorToken + "&wsfunction=core_notes_update_notes" + "&notes[0][publishstate]=personal"
                       + "&notes[0][id]=" + n.id + "&notes[0][text]=" + por.ToString() + " " + "&notes[0][format]=2" + "&moodlewsrestformat=json");
            yield return www;
            String content = www.text;
            UnityEngine.Debug.Log(content);
        }

        StartCoroutine(getCourseNotes());
    }

    IEnumerator getGrades()
    {
        StringBuilder por = new StringBuilder();
#if UNITY_ANDROID
        String token = userToken;
#else
        String token = tutorToken;
#endif
        
        WWW www = new WWW(moodleUrl + "/webservice/rest/server.php" + "?wstoken=" + token + "&wsfunction=core_notes_create_notes&notes[0][userid]=" + tutorID + "&notes[0][publishstate]=personal"
                        + "&notes[0][courseid]=5" + "&notes[0][text]=" + por.ToString() + " " + "&notes[0][format]=2" + "&moodlewsrestformat=json");
        yield return www;

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
