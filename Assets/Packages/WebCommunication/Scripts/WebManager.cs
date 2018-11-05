using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UserInfo;
using Random = System.Random;

public class WebManager : MonoBehaviour
{
    public TextAsset Tokens;

    //DB
    private WebserviceLogin _login;

    public DataManager Manager;

//    private DateTime _last = DateTime.UtcNow;

    // Use this for initialization
    void Start()
    {
        _login = new WebserviceLogin(Manager, Tokens.text);
    }

#if UNITY_EDITOR
    // TODO Remove username and password storage.
    public string Username = "";
    public string Password = "";

    public int CourseId;

    // COMUNICACAO COM O MOODLE
    public void MakeConnection()
    {
        Login(Username, Password);
    }
#endif

    public void Login(string username, string password)
    {
        StartCoroutine(LoginCoroutine(username, password, success => { }));
    }

    public delegate void LoginCallback(bool success);

    public void Login(string username, string password, LoginCallback callback)
    {
        StartCoroutine(LoginCoroutine(username, password, callback));
    }

    private IEnumerator LoginCoroutine(string username, string password, LoginCallback callback)
    {
        yield return _login.Login(username, password);
        if (_login.UserVerified)
        {
            _login.UpdateTime();
        }

        callback(_login.UserVerified);
    }

    public void RetrieveData(string username)
    {
        StartCoroutine(RetrieveDataCoroutine(username, (percentage, message) => { }));
    }

    public delegate void ProgressCallback(float percentage, string message);

    public void RetrieveData(string username, ProgressCallback callback)
    {
        StartCoroutine(RetrieveDataCoroutine(username, callback));
    }

    private IEnumerator RetrieveDataCoroutine(string username, ProgressCallback callback)
    {
        callback(0f, "Retrieving User Informations...");
        yield return _login.RetrieveUser(username);
        callback(1.0f / 7.0f, "Retrieving Courses...");
        yield return
            _login.RetrieveCourses(); // busca as cadeiras que o aluno esta inscrito, tem em conta se são varias cadeiras

        callback(2.0f / 7.0f, "Retrieving Course Groups...");
        yield return _login.RetrieveCourseGroups();
        callback(3.0f / 7.0f, "Retrieving Course Topics...");
        yield return _login.RetrieveCourseTopics();

        callback(4.0f / 7.0f, "Retrieving Course Grades...");
        yield return _login.RetrieveCourseGrades();
        callback(5.0f / 7.0f, "Retrieving User Grades...");
        yield return _login.RetrieveUserGrades();
        callback(6.0f / 7.0f, "Retrieving Forum Data...");
        yield return _login.RetrieveForumData();
        _login.UpdateTime();
        Manager.GetUser().doneWriting();
        callback(1f, "Retrieved All Data.");
    }


    // COMUNICACAO COM A BD
    /**
     * Metodo que insere o Login em cada cadeira presente, sendo uma no WebGL e X no Android
     * 
     */
    public void InsertLogin()
    {
        //DateTime time = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Local);

        //time = time.AddSeconds(mod.timecreated).ToLocalTime();
        TimeSpan s = DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc));
        int seconds = Convert.ToInt32(s.TotalSeconds);
        foreach (Course c in Manager.GetCourses())
        {
            if ((new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)
                    .AddSeconds(Manager.GetCourseById(c.id).logins[Manager.GetCourseById(c.id).logins.Count - 1])
                    .Day) != (new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(seconds).Day))
            {
                // TEMPORARIO LIMITA A UM POR DIA
//                StartCoroutine(_dbCons.InsertLogin(c.id));
                Manager.GetCourseById(c.id).logins.Add(seconds);
            }


            Manager.GetCourseById(c.id).logins.Sort();

            StartCoroutine(_login.StartUpdateCheck(Manager.GetCourseById(c.id)
                .logins[Manager.GetCourseById(c.id).logins.Count - 1]));
            Manager.GetCourseById(c.id).getAverageLoginSpace();
            Debug.Log("CADEIRA: " + Manager.GetCourseById(c.id).fullName + " nlogins: " +
                      Manager.GetCourseById(c.id).logins.Count);
        }
    }

    // Update is called once per frame
//    private int _i;
//    private List<string> requestsMade = new List<string>();
//    void Update()
//    {
//        if (Manager.GetUser().readyForRead && _i == 0 && _login.UserVerified
//        ) // a escrita do aluno foi completa e foi autenticado
//        {
//            getParameters();
//            checkLoginsDB();
//            getPerformance();
//
//            seeModulesViewed();
//            _i++;
//        }
//
//        if (Manager.GetUser().readyForRead && _login.UserVerified &&
//            Manager.GetCourseById(courseId).parameters != null && Manager.GetCourseById(courseId).logins.Count != 0 &&
//            _i == 1)
//        {
//            // fazer calculos para obter frases
//            double loginI = Manager.GetCourseById(courseId).averageLoginSpace / 86400.0;
//            //Debug.Log(manager.getCourseById(courseId).averageLoginSpace);
//            if (loginI < Manager.GetCourseById(courseId).parameters.login_low)
//            {
//                //Debug.Log("ASSID HIGH");
//                Assid = "high";
//            }
//            else
//                Assid = (loginI < Manager.GetCourseById(courseId).parameters.login_high) ? "middle" : "low";
//
//            //Debug.Log(assid);
//            //Debug.Log(manager.getCourseById(courseId).currentAprov + "/" + manager.getCourseById(courseId).maxCurrentAprov);
//            int maxAprov = Manager.GetCourseById(courseId).maxCurrentAprov;
//            maxAprov = Math.Max(maxAprov, 1);
//            int aprovV = Manager.GetCourseById(courseId).currentAprov * 100 / maxAprov;
//            //Debug.Log(aprovV);
//            if (aprovV > Manager.GetCourseById(courseId).parameters.aprov_high)
//                Aprov = "high";
//            else
//                Aprov = (aprovV > Manager.GetCourseById(courseId).parameters.aprov_low) ? "middle" : "low";
//            //Debug.Log(aprov);
//            getPhrases(Aprov, Assid);
//            putPerformance();
//            InsertLogin();
//            _i++;
//        }
//
//        if (requestsMade.Count > 0)
//        {
//            TimeSpan sp = DateTime.UtcNow - _last;
//            if (sp.TotalSeconds > 1)
//            {
//                _last = DateTime.UtcNow;
//                String[] copyR = new String[requestsMade.Count];
//                requestsMade.CopyTo(copyR);
//                foreach (String s in copyR)
//                {
//                    if (_dbCons.hashtable.ContainsKey(s))
//                    {
//                        if (s.Contains("getLogins"))
//                        {
//                            List<jsonValues.logins> logins = _dbCons.hashtable[s] as List<jsonValues.logins>;
//                            foreach (jsonValues.logins l in logins)
//                            {
//                                Manager.GetCourseById(l.course).logins.Add(l.login);
//                            }
//                        }
//                        else if (s.Equals("getphrases")) // check error
//                        {
//                            Values v = JsonUtility.FromJson<Values>("{\"phrases\":" + _dbCons.hashtable[s] + "}");
//
//                            _phrase = filterText(v.phrases);
//
//                            Debug.Log(_phrase);
//                        }
//
//                        if (s.Contains("getparameters"))
//                        {
//                            string text = _dbCons.hashtable[s] as string;
//                            text = Regex.Replace(text, @"^\[|\]$", string.Empty);
//                            Course.dbValues temp =
//                                JsonUtility.FromJson<Course.dbValues>(text);
//                            Manager.GetCourseById(temp.courseId).parameters = temp;
//                        }
//
//                        if (s.Contains("getmodulesviewed"))
//                        {
//                            StringBuilder por = new StringBuilder();
//                            por.Append("{\"modules\":");
//                            por.Append(_dbCons.hashtable[s] + "}");
//
//                            List<jsonValues.modulesViewed> m = JsonUtility.FromJson<Values>(por.ToString()).modules;
//                            DateTime time;
//                            foreach (jsonValues.modulesViewed mod in m)
//                            {
//                                time = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Local);
//
//                                time = time.AddSeconds(mod.timecreated).ToLocalTime();
//                                //if (mod.objecttable.Equals("forum"))
//                                //{
//                                //    Debug.Log("As "+time.ToLocalTime()+" Foi visto o Forum: " + manager.getCourseById(mod.courseid).getForum(mod.contextinstanceid).name);
//                                //}
//                                //else if (mod.objecttable.Equals("resource"))
//                                //{
//                                //    Debug.Log("As " + time.ToLocalTime() + " Foi visto o recurso: " + manager.getCourseById(mod.courseid).getModuleById(mod.contextinstanceid).name);
//                                //}
//                                //else if (mod.objecttable.Equals("page"))
//                                //{
//                                //    Debug.Log("As " + time.ToLocalTime() + " Foi visto a pagina: " + manager.getCourseById(mod.courseid).getModuleById(mod.contextinstanceid).name);
//                                //}
//                                //else if (mod.objecttable.Equals("book"))
//                                //{
//                                //    Debug.Log("As " + time.ToLocalTime() + " Foi visto o livro: " + manager.getCourseById(mod.courseid).getModuleById(mod.contextinstanceid).name);
//                                //}
//                                //else if (mod.component.Equals("mod_assign"))
//                                //{
//                                //    Debug.Log("As " + time.ToLocalTime() + " Foi visto o fólio: " + manager.getCourseById(mod.courseid).getFolio(mod.contextinstanceid).name);
//                                //}
//
//                                Manager.GetCourseById(mod.courseid).compareUpdates(mod);
//                            }
//
//                            foreach (Course c in Manager.GetCourses())
//                            {
//                                Debug.Log("COURSE: " + c.fullName);
//                                foreach (Course.newsUpdate n in c.news)
//                                {
//                                    Debug.Log(n.cmid + " " + n.news);
//                                }
//                            }
//                        }
//
//                        requestsMade.Remove(s);
//                        _dbCons.hashtable.Remove(s);
//                    }
//                }
//            }
//        }
//    }
}