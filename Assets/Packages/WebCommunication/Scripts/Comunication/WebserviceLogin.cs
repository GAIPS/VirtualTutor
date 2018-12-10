using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text;
using System;
using SimpleJSON;
using UserInfo;


public class WebserviceLogin
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

    [Serializable]
    public class Tokens
    {
        public string url;
        public string userToken;
        public string adminToken;
        public string mobileToken;
    }

    private Tokens _tokens;

    private DateTime _lastUpdate;

    public bool UserVerified;
    // http://ec2-34-240-43-90.eu-west-1.compute.amazonaws.com/moodleFCUL/webservice/rest/server.php?wstoken=fff2866b7aa75ca34556714df5e2c173&wsfunction=get_data&moodlewsrestformat=json&courseid=16
    // http://ec2-34-240-43-90.eu-west-1.compute.amazonaws.com/moodleFCUL/course/view.php?id=17 <--- ID 17

    private DataManager _dataM;

    private int _cycle; // saber o nº de vezes que tentou ver actualizações

    // usado para separar multiplas cadeiras de so uma
    public bool MultiCourses = true;
    int courseId = 0;

    public WebserviceLogin(DataManager manager, string jsonTokens)
    {
        _dataM = manager;
        _tokens = JsonUtility.FromJson<Tokens>(jsonTokens);
    }

    // Metodo que organiza a busca de informação feita na aplicação mobile
    IEnumerator GetUserDataMobile(String username, string password)
    {
        yield return Login(username, password);
        if (UserVerified)
        {
            yield return RetrieveUser(username);
            yield return
                RetrieveCourses(); // busca as cadeiras que o aluno esta inscrito, tem em conta se são varias cadeiras

            yield return RetrieveCourseGroups();
            yield return RetrieveCourseTopics();

            //yield return getCourseNotes();
            yield return RetrieveCourseGrades();
            yield return RetrieveUserGrades();
            yield return RetrieveForumData();
            UpdateTime();

            _dataM.GetUser().doneWriting(); // marca o final da captacao de dados do user
        }
    }

    public void UpdateTime()
    {
        _lastUpdate =
            new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(_dataM.GetUser().datelast);
    }

    public IEnumerator Login(String username, string password)
    {
        string formUrl = _tokens.url + "/login/index.php";

        WWWForm loginFields = new WWWForm();

        loginFields.AddField("username", username);
        loginFields.AddField("password", password);

        WWW www = new WWW(formUrl, loginFields.data);

        yield return www;

        String pageSource = www.text;

        if (pageSource.Contains("<title>Dashboard</title>"))
            UserVerified = true;
        else
        {
            UserVerified = false;
        }
    }

    /*
     * Metodo que comunica com o servidor e vai buscar a informacao do user, nao utiliza sockets
     */
    public IEnumerator RetrieveUser(String username)
    {
        string url = _tokens.url + "/webservice/rest/server.php" + "?wstoken=" + _tokens.userToken +
                     "&wsfunction=core_user_get_users&criteria[0][key]=username&criteria[0][value]=" + username +
                     "&moodlewsrestformat=json";
        WWW www = new WWW(url);
        yield return www;
        String content = www.text;

//        var storage = PersistentDataStorage.Instance;
//        var state = storage.GetState();
//        state["Moodle"].AsObject["Users"] = JSONNode.Parse(content);
//        JSONNode users = JSONNode.Parse(content)["users"];
//        if (users != null && users.Count > 0)
//        {
//            state["Moodle"].AsObject["users"].AsObject.Add(users["users"][0]["id"],users["users"][0]);
//        }

        Values v = JsonUtility.FromJson<Values>(content);

        if (v.users.Count > 0)
        {
            _dataM.GetUser().receiveUsers(v.users[0]);

            UserVerified = true;
        }
        else
            UserVerified = false;
    }

    /**
     *  Metodo que comunica com o servidor e busca a informacao dos cursos a que o utilizador esta inscrito, nao utiliza sockets
     */
    public IEnumerator RetrieveCourses()
    {
        String token = _tokens.userToken;

        WWW www = new WWW(_tokens.url + "/webservice/rest/server.php" + "?wstoken=" + token +
                          "&wsfunction=core_enrol_get_users_courses&userid=" + _dataM.GetUser().id +
                          "&moodlewsrestformat=json");
        yield return www;
        String content = www.text;

        StringBuilder por = new StringBuilder();
        por.Append("{\"courses\":");
        por.Append(content + "}");

        Values v = JsonUtility.FromJson<Values>(por.ToString());
        _dataM.ReceiveCourses(v.courses, MultiCourses, courseId);

        foreach (Course c in _dataM.GetCourses())
        {
            yield return RetrieveUsersInCourse(c.id);
        }
    }

    public IEnumerator RetrieveUsersInCourse(int courseId)
    {
        String token = _tokens.userToken;
        WWW www = new WWW(_tokens.url + "/webservice/rest/server.php" + "?wstoken=" + token +
                          "&wsfunction=core_enrol_get_enrolled_users&courseid=" + courseId +
                          "&moodlewsrestformat=json");
        yield return www;
        String content = www.text;

        StringBuilder por = new StringBuilder();
        por.Append("{\"users\":");
        por.Append(content + "}");
    }

    public IEnumerator
        RetrieveUserGrades() // gradereport_overview_get_course_grades, vai buscar as notas do aluno relativamente a cadeira no geral
    {
        String token = _tokens.userToken;
        WWW www = new WWW(_tokens.url + "/webservice/rest/server.php" + "?wstoken=" + token +
                          "&wsfunction=gradereport_overview_get_course_grades&userid=" + _dataM.GetUser().id +
                          "&moodlewsrestformat=json");
        yield return www;
        String content = www.text;

        Values v = JsonUtility.FromJson<Values>(content);
        _dataM.ReceiveGrades(v.grades);
    }

    public IEnumerator RetrieveCourseTopics()
    {
        String token = _tokens.userToken;
        WWW www;
        String content;
        StringBuilder por;
        Values v;

        foreach (Course c in _dataM.GetCourses())
        {
            www = new WWW(_tokens.url + "/webservice/rest/server.php" + "?wstoken=" + token +
                          "&wsfunction=core_course_get_contents&courseid=" + c.id +
                          "&moodlewsrestformat=json"); // NAO sera utilizado de forma primaria para identificar conteudo na cadeira, eh necessario avaliar o aluno de acordo com o grupo a que pertence (avaliacao continua)
            yield return www;
            content = www.text;
            por = new StringBuilder();
            por.Append("{\"topics\":");
            por.Append(content + "}");

            v = JsonUtility.FromJson<Values>(por.ToString());
            c.receiveCourseTopics(v.topics);
        }
    }

    public IEnumerator RetrieveCourseGrades() // mod_assign_get_assignments, busca as tentativas (attemptgrades)
    {
        String token = _tokens.userToken;
        WWW www;
        String content;
        StringBuilder sb;
        int count;
        Values v;

        foreach (Course c in _dataM.GetCourses())
        {
            www = new WWW(_tokens.url + "/webservice/rest/server.php" + "?wstoken=" + token +
                          "&wsfunction=mod_assign_get_assignments&courseids[0]=" + c.id + "&moodlewsrestformat=json");
            yield return www;
            content = www.text;

            v = JsonUtility.FromJson<Values>(content);
            c.receiveAssignments(v.courses[0].assignments);

            sb = new StringBuilder();
            count = 0;
            foreach (Course.Folio f in c.folios)
            {
                sb.Append("&assignmentids[" + count + "]=" + f.id);
                count++;
            }

            www = new WWW(_tokens.url + "/webservice/rest/server.php" + "?wstoken=" + token +
                          "&wsfunction=mod_assign_get_grades" + sb + "&moodlewsrestformat=json");
            yield return www;
            content = www.text;

            v = JsonUtility.FromJson<Values>(content);
            c.receiveAssignmentsGrade(v.assignments, _dataM.GetUser().id);


            www = new WWW(_tokens.url + "/webservice/rest/server.php" + "?wstoken=" + token +
                          "&wsfunction=gradereport_user_get_grade_items&courseid=" + c.id + "&userid=" +
                          _dataM.GetUser().id + "&groupid=0&moodlewsrestformat=json");
            yield return www;
            content = www.text;

            v = JsonUtility.FromJson<Values>(content);
            foreach (jsonValues.usergrades ug in v.usergrades)
            {
                c.receiveGrades(ug);
            }
        }
    }

    public IEnumerator RetrieveForumData()
    {
        String token = _tokens.userToken;
        String content;
        StringBuilder por;
        Values v;
        WWW www;
        // report_competency_data_for_report
        foreach (Course c in _dataM.GetCourses())
        {
            www = new WWW(_tokens.url + "/webservice/rest/server.php" + "?wstoken=" + token +
                          "&wsfunction=mod_forum_get_forums_by_courses&courseids[0]=" + c.id +
                          "&moodlewsrestformat=json");
            yield return www;
            content = www.text;
            por = new StringBuilder();
            por.Append("{\"forums\":");
            por.Append(content + "}");

            v = JsonUtility.FromJson<Values>(por.ToString());

            _dataM.GetCourseById(c.id).receiveForums(v.forums);

            foreach (var f in _dataM.GetCourseById(c.id).forums)
            {
                www = new WWW(_tokens.url + "/webservice/rest/server.php" + "?wstoken=" + token +
                              "&wsfunction=mod_forum_get_forum_discussions_paginated&forumid=" + f.id +
                              "&moodlewsrestformat=json"); // vem organizados pelo mais recentemente alterado
                yield return www;
                content = www.text;
                v = JsonUtility.FromJson<Values>(content);
                _dataM.GetCourseById(c.id).receiveDiscussions(v.discussions, f.cmid);


                foreach (var d in f.discussions)
                {
                    www = new WWW(_tokens.url + "/webservice/rest/server.php" + "?wstoken=" + token +
                                  "&wsfunction=mod_forum_get_forum_discussion_posts&discussionid=" + d.id +
                                  "&moodlewsrestformat=json"); // vem organizados pelo mais recentemente alterado
                    yield return www;
                    content = www.text;
                    v = JsonUtility.FromJson<Values>(content);
                    _dataM.GetCourseById(c.id).receivePosts(v.posts, f.id, d.id);
                }
            }
        }
    }

    public IEnumerator RetrieveCourseGroups()
    {
        String token = _tokens.userToken;
        WWW www;
        String content;
        Values v;
        foreach (var c in _dataM.GetCourses())
        {
            www = new WWW(_tokens.url + "/webservice/rest/server.php" + "?wstoken=" + token +
                          "&wsfunction=core_group_get_course_user_groups&courseid=" + c.id + "&userid=" +
                          _dataM.GetUser().id + "&moodlewsrestformat=json");
            yield return www;
            content = www.text;

            v = JsonUtility.FromJson<Values>(content);
            c.receiveGroups(v.groups);
        }
    }

    public IEnumerator StartUpdateCheck(int timestamp)
    {
        return CheckNewInfo(timestamp);
    }

    /**
     * Metodo para verificar se houve actualizações desde o ultimo login, só serve para por em texto as novidades
     * TODO core_course_check_updates pode ter que ser utilizado para validar informação que afecte o user -> Check if there is updates affecting the user for the given course and contexts.
     * */
    private IEnumerator CheckNewInfo(int timestamp)
    {
        String token = _tokens.userToken;
        TimeSpan s = _lastUpdate - new DateTime(1970, 1, 1);
        Debug.Log("Verificar updates: " + _cycle);
        _cycle++;
        StringBuilder debrief = new StringBuilder();
        WWW www;
        String content;
        Values v;


        foreach (Course c in _dataM.GetCourses())
        {
            //UnityEngine.Debug.Log(_tokens.url + "/webservice/rest/server.php" + "?wstoken=" + token + "&wsfunction=core_course_get_updates_since&courseid=" + c.id + "&since=" + (int)s.TotalSeconds + "&moodlewsrestformat=json");
            www = new WWW(_tokens.url + "/webservice/rest/server.php" + "?wstoken=" + token +
                          "&wsfunction=core_course_get_updates_since&courseid=" + c.id + "&since=" +
                          (int) s.TotalSeconds + "&moodlewsrestformat=json");
            yield return www;
            content = www.text;
            v = JsonUtility.FromJson<Values>(content);

            if (v.instances.Count > 0) // Ocurreu algo de novo
            {
                c.receiveUpdates(v.instances, _dataM.GetUser().datelast);
            }
            else if (v.instances.Count == 0)
            {
                debrief.Append("Course " + c.fullName + " has nothing to report\n\n");
            }

            debrief.Append("\n");
        }
    }

    private IEnumerator HasUpdates()
    {
        String token = _tokens.userToken;
        TimeSpan s = _lastUpdate - new DateTime(1970, 1, 1);
        Debug.Log("Verificar updates: " + _cycle);
        _cycle++;
        WWW www;
        String content;
        Values v;

        foreach (Course c in _dataM.GetCourses())
        {
            www = new WWW(_tokens.url + "/webservice/rest/server.php" + "?wstoken=" + token +
                          "&wsfunction=core_course_get_updates_since&courseid=" + c.id + "&since=" +
                          (int) s.TotalSeconds + "&moodlewsrestformat=json");
            yield return www;
            content = www.text;
            v = JsonUtility.FromJson<Values>(content);


            if (v.instances != null)
            {
                if (v.instances.Count > 0)
                {
                    yield return ExecuteUpdates(v.instances, c.id);
                }
            }
        }
    }

    private IEnumerator ExecuteUpdates(List<jsonValues.instances> instances, int id)
    {
        _dataM.GetUser().needsWriting();

        StringBuilder sb = new StringBuilder(), modu = new StringBuilder();
        object m;

        foreach (jsonValues.instances i in instances)
        {
            // fazer updates aqui de acordo com o que pede, por agora manter assim
            yield return RetrieveCourseTopics();

            foreach (jsonValues.updates u in i.updates)
            {
                sb.Append(u.name + ",");
                if (u.name.ToLower().Equals("configuration"))
                {
                    m = _dataM.GetCourseById(id).getUndefinedModule(i.id);

                    if (m != null)
                    {
                        if (m.GetType().Equals(typeof(Course.modules)))
                        {
                            Course.modules mod = (Course.modules) m;
                            modu.Append("TEM O NOME: " + mod.name);
                        }
                    }
                    else
                        modu.Append("ERRO ID: " + i.id);
                }
            }

            Debug.Log("Um(a) " + i.contextlevel + " com o id: " + i.id + " sofreu uma alteracao na " +
                      sb.Remove(sb.Length - 1, 1) + "\n" + modu);


            sb = new StringBuilder();
            modu = new StringBuilder();
        }

        _dataM.GetUser().doneWriting();
    }
}