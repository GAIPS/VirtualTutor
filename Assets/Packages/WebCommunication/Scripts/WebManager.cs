using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using System.Text;

public class WebManager : MonoBehaviour
{
    [Serializable]
    public class Values
    {
        public List<jsonValues.phrases> phrases;
        public List<jsonValues.tutor> tutor;
        public List<jsonValues.modulesViewed> modules;
    }

    private Values v;

    //DB
    public Boolean success;
    public string tutor = null;
    public int tutorchosen = 0;
    private databaseConnections dbCons;
    private WebserviceLogin login;

    public DataManager manager;

    public static WebManager Instance;
    public String assid, aprov;
    private String phrase;

    private List<String> requestsMade = new List<string>();

    // Use this for initialization
    void Start()
    {
        GameObject moodleLogin = gameObject; //GameObject.Find("moodleLogin");
        login = new WebserviceLogin(manager);
        dbCons = moodleLogin.AddComponent(typeof(databaseConnections)) as databaseConnections;
        Instance = this;
        //startConnectionWithId(3, 5);

        //Debug.Log(dbCons.databaseConnection());
        //conID();
        //makeConnection();
    }

    int i = 0;

    DateTime last = DateTime.UtcNow;
    private bool _onlyConnect = true;

    // Update is called once per frame
    void Update()
    {
        if (_onlyConnect)
            return;
        
        if (manager.getUser().readyForRead && i == 0 && login.UserVerified
        ) // a escrita do aluno foi completa e foi autenticado
        {
#if UNITY_WEBGL
            getTutor();
#endif
            getParameters();
            checkLoginsDB();
            getPerformance();

            seeModulesViewed();
            i++;
        }

        if (manager.getUser().readyForRead && login.UserVerified &&
            manager.getCourseById(courseId).parameters != null && manager.getCourseById(courseId).logins.Count != 0 &&
            i == 1)
        {
            // fazer calculos para obter frases
            double loginI = manager.getCourseById(courseId).averageLoginSpace / 86400.0;
            //Debug.Log(manager.getCourseById(courseId).averageLoginSpace);
            if (loginI < manager.getCourseById(courseId).parameters.login_low)
            {
                //Debug.Log("ASSID HIGH");
                assid = "high";
            }
            else
                assid = (loginI < manager.getCourseById(courseId).parameters.login_high) ? "middle" : "low";

            //Debug.Log(assid);
            //Debug.Log(manager.getCourseById(courseId).currentAprov + "/" + manager.getCourseById(courseId).maxCurrentAprov);
            int maxAprov = manager.getCourseById(courseId).maxCurrentAprov;
            maxAprov = Math.Max(maxAprov, 1);
            int aprovV = manager.getCourseById(courseId).currentAprov * 100 / maxAprov;
            //Debug.Log(aprovV);
            if (aprovV > manager.getCourseById(courseId).parameters.aprov_high)
                aprov = "high";
            else
                aprov = (aprovV > manager.getCourseById(courseId).parameters.aprov_low) ? "middle" : "low";
            //Debug.Log(aprov);
            getPhrases(aprov, assid);
            putPerformance();
            InsertLogin();
            i++;
        }

        if (requestsMade.Count > 0)
        {
            TimeSpan sp = DateTime.UtcNow - last;
            if (sp.TotalSeconds > 1)
            {
                last = DateTime.UtcNow;
                String[] copyR = new String[requestsMade.Count];
                requestsMade.CopyTo(copyR);
                foreach (String s in copyR)
                {
                    if (dbCons.hashtable.ContainsKey(s))
                    {
                        if (s.Contains("getLogins"))
                        {
                            List<jsonValues.logins> logins = dbCons.hashtable[s] as List<jsonValues.logins>;
                            foreach (jsonValues.logins l in logins)
                            {
                                manager.getCourseById(l.course).logins.Add(l.login);
                            }
                        }
                        else if (s.Equals("getphrases")) // check error
                        {
                            Values v = JsonUtility.FromJson<Values>("{\"phrases\":" + dbCons.hashtable[s] + "}");

                            phrase = filterText(v.phrases);

                            Debug.Log(phrase);
                        }

                        if (s.Contains("getparameters"))
                        {
                            string text = dbCons.hashtable[s] as string;
                            text = Regex.Replace(text, @"^\[|\]$", string.Empty);
                            UserInfo.Course.dbValues temp =
                                UnityEngine.JsonUtility.FromJson<UserInfo.Course.dbValues>(text);
                            manager.getCourseById(temp.courseId).parameters = temp;
                        }

                        if (s.Contains("gettutor")) // DEDICADO AO WEBGL
                        {
                            // ver o valor escolhido, se nao houve fazer fase de escolha de tutor
                            try
                            {
                                Values v = JsonUtility.FromJson<Values>(dbCons.hashtable[s].ToString());

                                if (v.tutor[0].tutorid == 0)
                                {
                                    tutor = "";
                                    // Proceder para a escolha do tutor
                                }
                                else
                                {
                                    //load TUTOR
                                    Debug.Log("Carregar o tutor " + ((v.tutor[0].tutorid == 1) ? "João" : "Maria"));
                                    tutor = ((v.tutor[0].tutorid == 1) ? "joao" : "Maria");
                                }
                            }
                            catch
                            {
                                Debug.Log("NÃO Há ESCOLHA");
                                // Proceder para a escolha do tutor
                                tutor = "";
                            }

                            tutorchosen++;
                            Debug.Log("Final tutor: " + tutor);
                        }

                        if (s.Contains("getmodulesviewed"))
                        {
                            StringBuilder por = new StringBuilder();
                            por.Append("{\"modules\":");
                            por.Append(dbCons.hashtable[s] + "}");

                            List<jsonValues.modulesViewed> m = JsonUtility.FromJson<Values>(por.ToString()).modules;
                            DateTime time;
                            foreach (jsonValues.modulesViewed mod in m)
                            {
                                time = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Local);

                                time = time.AddSeconds(mod.timecreated).ToLocalTime();
                                //if (mod.objecttable.Equals("forum"))
                                //{
                                //    Debug.Log("As "+time.ToLocalTime()+" Foi visto o Forum: " + manager.getCourseById(mod.courseid).getForum(mod.contextinstanceid).name);
                                //}
                                //else if (mod.objecttable.Equals("resource"))
                                //{
                                //    Debug.Log("As " + time.ToLocalTime() + " Foi visto o recurso: " + manager.getCourseById(mod.courseid).getModuleById(mod.contextinstanceid).name);
                                //}
                                //else if (mod.objecttable.Equals("page"))
                                //{
                                //    Debug.Log("As " + time.ToLocalTime() + " Foi visto a pagina: " + manager.getCourseById(mod.courseid).getModuleById(mod.contextinstanceid).name);
                                //}
                                //else if (mod.objecttable.Equals("book"))
                                //{
                                //    Debug.Log("As " + time.ToLocalTime() + " Foi visto o livro: " + manager.getCourseById(mod.courseid).getModuleById(mod.contextinstanceid).name);
                                //}
                                //else if (mod.component.Equals("mod_assign"))
                                //{
                                //    Debug.Log("As " + time.ToLocalTime() + " Foi visto o fólio: " + manager.getCourseById(mod.courseid).getFolio(mod.contextinstanceid).name);
                                //}

                                manager.getCourseById(mod.courseid).compareUpdates(mod);
                            }

                            foreach (UserInfo.Course c in manager.getCourses())
                            {
                                Debug.Log("COURSE: " + c.fullName);
                                foreach (UserInfo.Course.newsUpdate n in c.news)
                                {
                                    Debug.Log(n.cmid + " " + n.news);
                                }
                            }
                        }

                        requestsMade.Remove(s);
                        dbCons.hashtable.Remove(s);
                    }
                }
            }
        }
    }

    // COMUNICACAO COM O MOODLE
    public void MakeConnection()
    {
        Action callback = () =>
        {
            if (manager.getUser().readyForRead && login.UserVerified)
            {
                Debug.Log("Logging Successful!");
                login.compareTime();
            }
            else
            {
                Debug.Log("Logging failed...");
            }
        };
        StartCoroutine(LoginCoroutine(callback));
    }

    private IEnumerator LoginCoroutine(Action callback)
    {
        yield return login.BeginConnection(userName, password);
        if (callback != null)
        {
            callback();
        }
    }


    // COMUNICACAO COM A BD
    /**
     * Metodo que insere o Login em cada cadeira presente, sendo uma no WebGL e X no Android
     * 
     * */
    public void InsertLogin()
    {
        //DateTime time = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Local);

        //time = time.AddSeconds(mod.timecreated).ToLocalTime();
        TimeSpan s = DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc));
        int seconds = Convert.ToInt32(s.TotalSeconds);
        foreach (UserInfo.Course c in manager.getCourses())
        {
            if ((new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)
                    .AddSeconds(manager.getCourseById(c.id).logins[manager.getCourseById(c.id).logins.Count - 1])
                    .Day) != (new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(seconds).Day))
            {
                // TEMPORARIO LIMITA A UM POR DIA
                StartCoroutine(dbCons.insertLogin(c.id));
                manager.getCourseById(c.id).logins.Add(seconds);
            }


            manager.getCourseById(c.id).logins.Sort();

            StartCoroutine(login.StartUpdateCheck(manager.getCourseById(c.id).logins[manager.getCourseById(c.id).logins.Count - 1]));
            manager.getCourseById(c.id).getAverageLoginSpace();
            Debug.Log("CADEIRA: " + manager.getCourseById(c.id).fullName + " nlogins: " +
                      manager.getCourseById(c.id).logins.Count);
        }

        requestsMade.Add("insertLogin");
    }

    /**
     * Metodo que vai buscar os Logins, Ainda precisa de ajustes
     **/
    public void checkLoginsDB()
    {
        List<int> courseidList = new List<int>();
        foreach (UserInfo.Course c in manager.getCourses())
        {
            courseidList.Add(c.id);
        }

        dbCons.getLogins(courseidList);
        requestsMade.Add("getLogins");
    }

    /**
     * Busca as frases de feedback com base no aproveitamento e assiduidade do aluno
     * 
     * */
    public void getPhrases(String aproveitamento, String assiduidade)
    {
        String filename = "phrases.php";
        Hashtable parameters = new Hashtable();
        parameters.Add("function", "getphrases");
        requestsMade.Add(parameters["function"].ToString());
        parameters.Add("iden", new String[] {"f"});
        parameters.Add("aprov", aproveitamento);
        parameters.Add("assid", assiduidade);
        dbCons.prepareRequest(filename, parameters);
    }

    /**
     * Busca o Tutor selecionado pelo aluno no WebGl
     * */
    public void getTutor()
    {
        String filename = "tutor.php";
        Hashtable parameters = new Hashtable();
        parameters.Add("function", "gettutor");
        requestsMade.Add(parameters["function"].ToString());

        parameters.Add("courseid", courseId);
        parameters.Add("userid", userId);

        dbCons.prepareRequest(filename, parameters);
    }

    /**
     * Insere na BD a escolha do aluno para o tutor
     * tutorid -> Id associado ao tutor 1 para joao, 2 para maria
     * */
    public void chooseTutor(int tutorid)
    {
        String filename = "tutor.php";
        Hashtable parameters = new Hashtable();
        parameters.Add("function", "selecttutor");
        requestsMade.Add(parameters["function"].ToString());

        parameters.Add("courseid", courseId);
        parameters.Add("userid", userId);
        parameters.Add("tutorid", tutorid);
        dbCons.prepareRequest(filename, parameters);
    }

    public void seeModulesViewed()
    {
        String filename = "logs.php";
        Hashtable parameters = new Hashtable();
        parameters.Add("function", "getmodulesviewed");
        requestsMade.Add(parameters["function"].ToString());
        parameters.Add("secret", true);
        parameters.Add("courseid", courseId);
        parameters.Add("userid", userId);
        parameters.Add("timecreated", manager.getUser().datelast);

        dbCons.prepareRequest(filename, parameters);
    }

    public void getParameters()
    {
        String filename = "parameters.php";
        Hashtable parameters = new Hashtable();
        parameters.Add("function", "getparameters");
        requestsMade.Add(parameters["function"].ToString());

        parameters.Add("courseid", courseId);

        dbCons.prepareRequest(filename, parameters);
    }

    public void getPerformance()
    {
        String filename = "student_performance.php";
        Hashtable parameters = new Hashtable();
        parameters.Add("function", "getperformance");
        requestsMade.Add(parameters["function"].ToString());

        parameters.Add("courseid", courseId);
        parameters.Add("studentid", userId);


        dbCons.prepareRequest(filename, parameters);
    }

    public void putPerformance()
    {
#if !UNITY_WEBGL
        TimeSpan s = DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc));
        int seconds = Convert.ToInt32(s.TotalSeconds);
        List<Hashtable> param = new List<Hashtable>();
        String filename = "student_performance.php";
        foreach (UserInfo.Course c in manager.getCourses())
        {
            manager.getCourseById(courseId).getAverageLoginSpace();
            if ((new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)
                    .AddSeconds(manager.getCourseById(c.id).logins[manager.getCourseById(c.id).logins.Count - 1])
                    .Day) != (new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(seconds).Day))
            {
                // TEMPORARIO LIMITA A UM POR DIA

                Hashtable parameters = new Hashtable();
                parameters.Add("function", "putperformance");
                //requestsMade.Add(parameters["function"].ToString());

                parameters.Add("courseid", c.id);
                parameters.Add("studentid", userId);

                int maxAprov = manager.getCourseById(c.id).maxCurrentAprov;
                maxAprov = Math.Max(maxAprov, 1);
                parameters.Add("aproveitamento", manager.getCourseById(c.id).currentAprov * 100 / maxAprov);
                parameters.Add("assiduidade", manager.getCourseById(courseId).averageLoginSpace);
                parameters.Add("time", seconds);
                param.Add(parameters);
            }
        }

        if (param.Count > 0)
            dbCons.prepareRequests(filename, param);
#else
        TimeSpan s = DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc));
        int seconds = Convert.ToInt32(s.TotalSeconds);
        foreach (UserInfo.Course c in manager.getCourses())
        {
            manager.getCourseById(courseId).getAverageLoginSpace();
            if ((new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(manager.getCourseById(c.id).logins[manager.getCourseById(c.id).logins.Count - 1]).Day) != (new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(seconds).Day))
            { // TEMPORARIO LIMITA A UM POR DIA
                String filename = "student_performance.php";
                Hashtable parameters = new Hashtable();
                parameters.Add("function", "putperformance");
                //requestsMade.Add(parameters["function"].ToString());

                parameters.Add("courseid", c.id);
                parameters.Add("studentid", userId);
                parameters.Add("aproveitamento", ((manager.getCourseById(c.id).currentAprov * 100) / manager.getCourseById(c.id).maxCurrentAprov));
                parameters.Add("assiduidade", manager.getCourseById(courseId).averageLoginSpace);
                parameters.Add("time", seconds);

                dbCons.prepareRequest(filename, parameters);
            }
        }

#endif
    }

    /**
     * Metodo generico para inicializar um pedido a base de dados, permite fazer pedidos adicionais sem estar limitado aos metodos existentes
     * 
     **/
    public void requestDB(String filename, Hashtable parameters)
    {
        requestsMade.Add(parameters["function"].ToString());
        dbCons.prepareRequest(filename, parameters);
    }

    /**
     * Filtra os textos recebidos
     * Atualmente filtra o feedback de acordo com o aproveitamento e assiduidade do aluno
     * p-> ver o ficheiro jsonValues
     * */
    public String filterText(List<jsonValues.phrases> p)
    {
        System.Random n = new System.Random();
        ArrayList feedbackE = new ArrayList();
        ArrayList feedbackf = new ArrayList();
        foreach (jsonValues.phrases i in p)
        {
            if (i.identifier.Contains("fe"))
                feedbackE.Add(i.frase);
            if (i.identifier.Contains("ff"))
                feedbackf.Add(i.frase);
        }

        StringBuilder sb = new StringBuilder();
        if (feedbackE.Count > 0)
            sb.Append(feedbackE[n.Next(0, feedbackE.Count)]);
        sb.Append(" ");
        if (feedbackf.Count > 0)
            sb.Append(feedbackf[n.Next(0, feedbackf.Count)]);


        String si = Regex.Replace(sb.ToString(), "@username", manager.getUser().userName);

        // adicionar filtros adicionais se necessários

        return si;
    }

    public int userId = 0;
    public String userName = "";
    public int courseId = 0;
    public String password = "";

    // METODOS PARA IR BUSCAR VALORES DADOS PELO USER
    public void getUserName(String name)
    {
        userName = name;
    }


    public void Get_userId(int id)
    {
        userId = id;
    }

    public void Get_courseId(int id)
    {
        courseId = id;
    }

    public String GetPhrase()
    {
        return phrase;
    }

    public void getPassword(String Password)
    {
        password = Password;
    }
}