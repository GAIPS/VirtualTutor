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
    [Serializable]
    public class Values
    {
        public List<jsonValues.phrases> phrases;
        public List<jsonValues.tutor> tutor;
        public List<jsonValues.modulesViewed> modules;
    }

    private Values _v;

    //DB
    private WebserviceLogin _login;
    private DatabaseConnections _dbCons;
    public Boolean Success;
    public string Tutor;
    public int Tutorchosen;

    public string Assid, Aprov;
    private string _phrase;

    private List<string> requestsMade = new List<string>();

    public DataManager Manager;

    private int _i;

    private DateTime _last = DateTime.UtcNow;
    private bool _onlyConnect = true;

    // Use this for initialization
    void Start()
    {
        _login = new WebserviceLogin(Manager); // TODO read config file
        _dbCons = new DatabaseConnections(Manager.GetUser());
    }

    // Update is called once per frame
    void Update()
    {
        if (_onlyConnect)
            return;
        
        if (Manager.GetUser().readyForRead && _i == 0 && _login.UserVerified
        ) // a escrita do aluno foi completa e foi autenticado
        {
            getParameters();
            checkLoginsDB();
            getPerformance();

            seeModulesViewed();
            _i++;
        }

        if (Manager.GetUser().readyForRead && _login.UserVerified &&
            Manager.GetCourseById(courseId).parameters != null && Manager.GetCourseById(courseId).logins.Count != 0 &&
            _i == 1)
        {
            // fazer calculos para obter frases
            double loginI = Manager.GetCourseById(courseId).averageLoginSpace / 86400.0;
            //Debug.Log(manager.getCourseById(courseId).averageLoginSpace);
            if (loginI < Manager.GetCourseById(courseId).parameters.login_low)
            {
                //Debug.Log("ASSID HIGH");
                Assid = "high";
            }
            else
                Assid = (loginI < Manager.GetCourseById(courseId).parameters.login_high) ? "middle" : "low";

            //Debug.Log(assid);
            //Debug.Log(manager.getCourseById(courseId).currentAprov + "/" + manager.getCourseById(courseId).maxCurrentAprov);
            int maxAprov = Manager.GetCourseById(courseId).maxCurrentAprov;
            maxAprov = Math.Max(maxAprov, 1);
            int aprovV = Manager.GetCourseById(courseId).currentAprov * 100 / maxAprov;
            //Debug.Log(aprovV);
            if (aprovV > Manager.GetCourseById(courseId).parameters.aprov_high)
                Aprov = "high";
            else
                Aprov = (aprovV > Manager.GetCourseById(courseId).parameters.aprov_low) ? "middle" : "low";
            //Debug.Log(aprov);
            getPhrases(Aprov, Assid);
            putPerformance();
            InsertLogin();
            _i++;
        }

        if (requestsMade.Count > 0)
        {
            TimeSpan sp = DateTime.UtcNow - _last;
            if (sp.TotalSeconds > 1)
            {
                _last = DateTime.UtcNow;
                String[] copyR = new String[requestsMade.Count];
                requestsMade.CopyTo(copyR);
                foreach (String s in copyR)
                {
                    if (_dbCons.hashtable.ContainsKey(s))
                    {
                        if (s.Contains("getLogins"))
                        {
                            List<jsonValues.logins> logins = _dbCons.hashtable[s] as List<jsonValues.logins>;
                            foreach (jsonValues.logins l in logins)
                            {
                                Manager.GetCourseById(l.course).logins.Add(l.login);
                            }
                        }
                        else if (s.Equals("getphrases")) // check error
                        {
                            Values v = JsonUtility.FromJson<Values>("{\"phrases\":" + _dbCons.hashtable[s] + "}");

                            _phrase = filterText(v.phrases);

                            Debug.Log(_phrase);
                        }

                        if (s.Contains("getparameters"))
                        {
                            string text = _dbCons.hashtable[s] as string;
                            text = Regex.Replace(text, @"^\[|\]$", string.Empty);
                            Course.dbValues temp =
                                JsonUtility.FromJson<Course.dbValues>(text);
                            Manager.GetCourseById(temp.courseId).parameters = temp;
                        }

                        if (s.Contains("gettutor")) // DEDICADO AO WEBGL
                        {
                            // ver o valor escolhido, se nao houve fazer fase de escolha de tutor
                            try
                            {
                                Values v = JsonUtility.FromJson<Values>(_dbCons.hashtable[s].ToString());

                                if (v.tutor[0].tutorid == 0)
                                {
                                    Tutor = "";
                                    // Proceder para a escolha do tutor
                                }
                                else
                                {
                                    //load TUTOR
                                    Debug.Log("Carregar o tutor " + ((v.tutor[0].tutorid == 1) ? "João" : "Maria"));
                                    Tutor = ((v.tutor[0].tutorid == 1) ? "joao" : "Maria");
                                }
                            }
                            catch
                            {
                                Debug.Log("NÃO Há ESCOLHA");
                                // Proceder para a escolha do tutor
                                Tutor = "";
                            }

                            Tutorchosen++;
                            Debug.Log("Final tutor: " + Tutor);
                        }

                        if (s.Contains("getmodulesviewed"))
                        {
                            StringBuilder por = new StringBuilder();
                            por.Append("{\"modules\":");
                            por.Append(_dbCons.hashtable[s] + "}");

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

                                Manager.GetCourseById(mod.courseid).compareUpdates(mod);
                            }

                            foreach (Course c in Manager.GetCourses())
                            {
                                Debug.Log("COURSE: " + c.fullName);
                                foreach (Course.newsUpdate n in c.news)
                                {
                                    Debug.Log(n.cmid + " " + n.news);
                                }
                            }
                        }

                        requestsMade.Remove(s);
                        _dbCons.hashtable.Remove(s);
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
            if (Manager.GetUser().readyForRead && _login.UserVerified)
            {
                Debug.Log("Logging Successful!");
                _login.compareTime();
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
        yield return _login.BeginConnection(userName, password);
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
        foreach (Course c in Manager.GetCourses())
        {
            if ((new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)
                    .AddSeconds(Manager.GetCourseById(c.id).logins[Manager.GetCourseById(c.id).logins.Count - 1])
                    .Day) != (new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(seconds).Day))
            {
                // TEMPORARIO LIMITA A UM POR DIA
                StartCoroutine(_dbCons.InsertLogin(c.id));
                Manager.GetCourseById(c.id).logins.Add(seconds);
            }


            Manager.GetCourseById(c.id).logins.Sort();

            StartCoroutine(_login.StartUpdateCheck(Manager.GetCourseById(c.id).logins[Manager.GetCourseById(c.id).logins.Count - 1]));
            Manager.GetCourseById(c.id).getAverageLoginSpace();
            Debug.Log("CADEIRA: " + Manager.GetCourseById(c.id).fullName + " nlogins: " +
                      Manager.GetCourseById(c.id).logins.Count);
        }

        requestsMade.Add("insertLogin");
    }

    /**
     * Metodo que vai buscar os Logins, Ainda precisa de ajustes
     **/
    public void checkLoginsDB()
    {
        List<int> courseidList = new List<int>();
        foreach (Course c in Manager.GetCourses())
        {
            courseidList.Add(c.id);
        }

        StartCoroutine(_dbCons.GetLogins(courseidList));
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
        parameters.Add("iden", new[] {"f"});
        parameters.Add("aprov", aproveitamento);
        parameters.Add("assid", assiduidade);
        StartCoroutine(_dbCons.PrepareRequest(filename, parameters));
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

        StartCoroutine(_dbCons.PrepareRequest(filename, parameters));
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
        StartCoroutine(_dbCons.PrepareRequest(filename, parameters));
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
        parameters.Add("timecreated", Manager.GetUser().datelast);

        StartCoroutine(_dbCons.PrepareRequest(filename, parameters));
    }

    public void getParameters()
    {
        String filename = "parameters.php";
        Hashtable parameters = new Hashtable();
        parameters.Add("function", "getparameters");
        requestsMade.Add(parameters["function"].ToString());

        parameters.Add("courseid", courseId);

        StartCoroutine(_dbCons.PrepareRequest(filename, parameters));
    }

    public void getPerformance()
    {
        String filename = "student_performance.php";
        Hashtable parameters = new Hashtable();
        parameters.Add("function", "getperformance");
        requestsMade.Add(parameters["function"].ToString());

        parameters.Add("courseid", courseId);
        parameters.Add("studentid", userId);


        StartCoroutine(_dbCons.PrepareRequest(filename, parameters));
    }

    public void putPerformance()
    {
        TimeSpan s = DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc));
        int seconds = Convert.ToInt32(s.TotalSeconds);
        List<Hashtable> param = new List<Hashtable>();
        String filename = "student_performance.php";
        foreach (Course c in Manager.GetCourses())
        {
            Manager.GetCourseById(courseId).getAverageLoginSpace();
            if ((new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)
                    .AddSeconds(Manager.GetCourseById(c.id).logins[Manager.GetCourseById(c.id).logins.Count - 1])
                    .Day) != (new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(seconds).Day))
            {
                // TEMPORARIO LIMITA A UM POR DIA

                Hashtable parameters = new Hashtable();
                parameters.Add("function", "putperformance");
                //requestsMade.Add(parameters["function"].ToString());

                parameters.Add("courseid", c.id);
                parameters.Add("studentid", userId);

                int maxAprov = Manager.GetCourseById(c.id).maxCurrentAprov;
                maxAprov = Math.Max(maxAprov, 1);
                parameters.Add("aproveitamento", Manager.GetCourseById(c.id).currentAprov * 100 / maxAprov);
                parameters.Add("assiduidade", Manager.GetCourseById(courseId).averageLoginSpace);
                parameters.Add("time", seconds);
                param.Add(parameters);
            }
        }

        if (param.Count > 0)
            StartCoroutine(_dbCons.PrepareRequests(filename, param));
    }

    /**
     * Metodo generico para inicializar um pedido a base de dados, permite fazer pedidos adicionais sem estar limitado aos metodos existentes
     * 
     **/
    public void requestDB(String filename, Hashtable parameters)
    {
        requestsMade.Add(parameters["function"].ToString());
        StartCoroutine(_dbCons.PrepareRequest(filename, parameters));
    }

    /**
     * Filtra os textos recebidos
     * Atualmente filtra o feedback de acordo com o aproveitamento e assiduidade do aluno
     * p-> ver o ficheiro jsonValues
     * */
    public String filterText(List<jsonValues.phrases> p)
    {
        Random n = new Random();
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


        String si = Regex.Replace(sb.ToString(), "@username", Manager.GetUser().userName);

        // adicionar filtros adicionais se necessários

        return si;
    }

    public int userId;
    public String userName = "";
    public int courseId;
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
        return _phrase;
    }

    public void getPassword(String Password)
    {
        password = Password;
    }
}