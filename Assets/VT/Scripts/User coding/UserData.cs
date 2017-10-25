using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace UserInfo
{
    public class UserData : MonoBehaviour
    {


        public int id; // n de identificacao do user
        public String userName; // username disposto no moodle
        public String fullName; // nome completo do user
        public String email; // email do user
        public String lang; // languagem que o user tem no moodle?

        // informacao potencialmente util
        public String imgSmall; // link para a imagem em tamanho pequeno do user
        public String imgProf; // imagem de perfil do user
                               //Time of first access
        public DateTime datefirst = new DateTime();
        //time of last access
        public DateTime datelast;
        //current localtime
        DateTime localTime = DateTime.Now;
        TimeSpan diff;
        public Boolean readyForRead = false;
        //informacao relativa as cadeiras do user
        //TBD
        public List<Course> courses = new List<Course>();

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public String giveData()
        {
            StringBuilder filtered = new StringBuilder();
            filtered.Append("TIME: " + localTime.ToLocalTime() + "\n");
            diff = DateTime.UtcNow.Subtract(datelast);
            // dados do user
            if (lang.Equals("en"))
            {
                if (localTime.ToLocalTime().Hour < 12)
                    filtered.Append("Good Morning " + fullName + "\n");
                else if (localTime.ToLocalTime().Hour >= 12 && localTime.ToLocalTime().Hour < 20)
                    filtered.Append("Good Afternoon " + fullName + "\n");
                else
                    filtered.Append("Good Night " + fullName + "\n");
                //filtered.Append("First time you entered in Moodle was: " + datefirst.ToLocalTime() + "\n");
                filtered.Append("Last time you entered in Moodle was: " + datelast.ToLocalTime() + "\n");
                
                filtered.Append("You haven't gone on Moodle in: " + diff.Hours + " hours " + diff.Minutes + " minutes and " + diff.Seconds + " seconds"  + "\n");
                
                foreach(Course c in courses)
                {
                    foreach(Course.Forum f in c.forums)
                    {
                        foreach(Course.Discussions d in f.discussions)
                        {
                            foreach(Course.Posts p in d.posts)
                            {
                                
                                if(p.userid == id)
                                {
                                    DateTime date = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                                    date = date.AddSeconds(Convert.ToInt32(p.created));
                                    filtered.Append("\nOn Forum " + f.name + " There's the Discussion " + d.name + " on which you posted: " + p.message + " at " + date.ToLocalTime());
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                if (localTime.ToLocalTime().Hour < 12)
                    filtered.Append("Bom dia " + fullName + "\n");
                else if (localTime.ToLocalTime().Hour >= 12 && localTime.ToLocalTime().Hour < 20)
                    filtered.Append("Boa tarde " + fullName + "\n");
                else
                    filtered.Append("Boa noite " + fullName + "\n");

                filtered.Append("Informação de cursos:\n");

                
            }
            return filtered.ToString();
        }

        // METODOS PARA CONVERTER EM FORMATO LEGIVEL O CONTEUDO

        public String courseDisplay(int courseId)
        {

            Course c = null;

            foreach (Course co in courses)
            {
                if (co.id == courseId)
                    c = co;
            }
            if (c == null)
                return null;

            StringBuilder s = new StringBuilder();

            if(c.visible == 1)
            {
                if (lang.Equals("en"))
                {
                    s.Append(c.fullName + "\n");
                    s.Append("ID:" + c.id + "\n");
                    s.Append("Sumary: " + c.summary + "\n");
                    if (c.grade == -1)
                        s.Append("No grade was yet given\n");
                    else
                        s.Append("Grade: " + c.grade + "\n");
                    //s.Append(c.toString());
                   
                }


                else
                {
                    s.Append("ID da cadeira:" + c.id + "\n");
                    s.Append("Nome da cadeira: " + c.fullName + "\n");
                    s.Append("Nomenclatura: " + c.shortName + "\n");
                    s.Append("Descricao: " + c.summary + "\n");
                    if (c.grade == -1)
                        s.Append("Avaliacao ainda nao foi feita\n");
                    else
                        s.Append("Nota: " + c.grade + "\n");
                    //s.Append(c.toString());
                }

            }
            
            return s.ToString();
        }

        public String TopicDisplay(int courseId, int topidID)
        {

            Course c = getCourse(courseId);
            if (c == null)
                return null;
            
            StringBuilder s = new StringBuilder();

            s.Append(c.TopicDisplay(topidID,lang));
            
            return s.ToString();
        }

        public String ModuleDisplay(int courseId, int topicId, int moduleId)
        {
            Course c = getCourse(courseId);
            if (c == null)
                return null;

            StringBuilder s = new StringBuilder();

            s.Append(c.ModuleDisplay(topicId,moduleId, lang));

            return s.ToString();
        }

        public String FolioDisplay(int courseId, int folioID)
        {

            Course c = getCourse(courseId);
            if (c == null)
                return null;

            StringBuilder s = new StringBuilder();

            s.Append(c.FolioDisplay(folioID));

            return s.ToString();
        }


        public Course getCourse(int id)
        {
            
            foreach (Course co in courses)
            {
                if (co.id == id)
                    return co;
            }
            return null;
        }

        // METODOS PARA FILTRAR WEBSERVICE RESPONSES

        //filtrar dados de core_user_get_users_by_id em XML
        public void filterUserData(String content)
        {
            String[] filter = content.Split('\n');
            String[] variable;
            Boolean isPerson = true;
            //StringBuilder filtered = new StringBuilder();
            //Time of first access
            DateTime datefirst = new DateTime();
            //time of last access
            DateTime datelast;
            //current localtime
            DateTime localTime = DateTime.Now;

            for (int l = 0; l < filter.Length; l++)
            {
                if (isPerson)
                {
                    //filtrar ID
                    if (filter[l].Contains("<KEY name=\"id\">"))
                    {
                        variable = filter[l].Split(new[] { "<KEY name=\"id\"><VALUE>" }, StringSplitOptions.None);
                        variable = variable[1].Split(new[] { "</VALUE>" }, StringSplitOptions.None);
                        //filtered.Append("O ID do utilizador é: " + variable[0] + "\n");
                        id = Int32.Parse(variable[0]);
                    }

                    //filtrar o Nome Completo
                    if (filter[l].Contains("<KEY name=\"fullname\">"))
                    {
                        variable = filter[l].Split(new[] { "<KEY name=\"fullname\"><VALUE>" }, StringSplitOptions.None);
                        variable = variable[1].Split(new[] { "</VALUE>" }, StringSplitOptions.None);
                        //filtered.Append("O Nome do utilizador é: " + variable[0] + "\n");
                        fullName = variable[0];
                    }

                    //filtrar o Nome de utilizador
                    if (filter[l].Contains("<KEY name=\"username\">"))
                    {
                        variable = filter[l].Split(new[] { "<KEY name=\"username\"><VALUE>" }, StringSplitOptions.None);
                        variable = variable[1].Split(new[] { "</VALUE>" }, StringSplitOptions.None);
                        //filtered.Append("O Nome do utilizador é: " + variable[0] + "\n");
                        userName = variable[0];
                    }

                    //filtrar o email
                    if (filter[l].Contains("<KEY name=\"email\">"))
                    {
                        variable = filter[l].Split(new[] { "<KEY name=\"email\"><VALUE>" }, StringSplitOptions.None);
                        variable = variable[1].Split(new[] { "</VALUE>" }, StringSplitOptions.None);
                        //filtered.Append("O email do utilizador é: " + variable[0] + "\n");
                        email = variable[0];
                    }

                    //filtrar a linguagem
                    if (filter[l].Contains("<KEY name=\"lang\">"))
                    {
                        variable = filter[l].Split(new[] { "<KEY name=\"lang\"><VALUE>" }, StringSplitOptions.None);
                        variable = variable[1].Split(new[] { "</VALUE>" }, StringSplitOptions.None);
                        //filtered.Append("O email do utilizador é: " + variable[0] + "\n");
                        lang = variable[0];
                    }

                    //filtrar o firstaccess
                    if (filter[l].Contains("<KEY name=\"firstaccess\">"))
                    {
                        variable = filter[l].Split(new[] { "<KEY name=\"firstaccess\"><VALUE>" }, StringSplitOptions.None);
                        variable = variable[1].Split(new[] { "</VALUE>" }, StringSplitOptions.None);
                        datefirst = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                        datefirst = datefirst.AddSeconds(Convert.ToInt32(variable[0]));
                        //filtered.Append("O primeiro acesso do utilizador foi: " + datefirst.ToLocalTime() + "\n");
                        this.datefirst = datefirst;
                    }

                    //filtrar o lastaccess e calcular o tempo total
                    if (filter[l].Contains("<KEY name=\"lastaccess\">"))
                    {
                        variable = filter[l].Split(new[] { "<KEY name=\"lastaccess\"><VALUE>" }, StringSplitOptions.None);
                        variable = variable[1].Split(new[] { "</VALUE>" }, StringSplitOptions.None);
                        datelast = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                        datelast = datelast.AddSeconds(Convert.ToInt32(variable[0]));
                        this.datelast = datelast;
                        //filtered.Append("O ultimo acesso foi: " + datelast.ToLocalTime() + "\n");

                        //TimeSpan delta = datelast - datefirst;
                        //TODO NEEDS WORK DONE
                        //filtered.Append("Inscrito Há: " + delta.Days + " dias e " + delta.Hours + " horas" + "\n"); 

                    }

                }
                
            }

        }
   
        // JSON BELOW

        /*
       * Recebe informacao basica do utilizador EM JSON
       */
        public void receiveUsers(jsonValues.users u)
        {
            id = u.id;
            userName = u.username;
            fullName = u.fullname;
            email = u.email;
            lang = u.lang;

            
            DateTime date = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            date = date.AddSeconds(Convert.ToInt32(u.firstaccess));
            
            this.datefirst = date;
            date = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            date = date.AddSeconds(Convert.ToInt32(u.lastaccess));
            
            datelast = date;
            
        }

        /*
        * Recebe informacao basica das cadeiras que o utilizador esta inscrito EM JSON
        */
        public void receiveCourses(List<jsonValues.Courses> c)
        {
            Course template;
            courses = new List<Course>();
            foreach(jsonValues.Courses co in c)
            {
                template = new Course();
                template.id = co.id;
                template.idNumber = co.idnumber;
                template.shortName = co.shortname;
                template.fullName = co.fullname;
                template.summary = Course.HtmlDecode(co.summary);
                template.lang = lang;
                template.visible = co.visible;
                courses.Add(template);
                
            }
        }

        /*
         * 
         */
        public void receiveGrades(List<jsonValues.Grades> g)
        {
            Course template;
            double n;
            bool isNumeric;
            foreach (jsonValues.Grades gr in g)
            {
                template = getCourse(gr.courseid);
                isNumeric = double.TryParse(gr.grade, out n);
                if (gr.grade != null && isNumeric)
                {
                    template.grade = double.Parse(gr.grade);
                }

            }

        }
        /**
         * Recebe informacao basica das cadeiras que o utilizador esta inscrito EM XML PS: DEIXOU DE SER USADO
         * */
        public void filterCourseData(String content)
        {
            String[] filter = content.Split('\n');
            String[] variable;
            int p;
            Course c = new Course();
            for (int l = 0; l < filter.Length; l++)
            {
                if (filter[l].Contains("<SINGLE>")) // Foi encontrada uma cadeira
                {
                    p = l + 1;

                    while (p < filter.Length && !filter[p].Contains("</SINGLE>"))
                    {
                        if (c == null)
                            c = new Course();
                        if (filter[p].Contains("<KEY name=\"id\">"))
                        {
                            variable = filter[p].Split(new[] { "<KEY name=\"id\"><VALUE>" }, StringSplitOptions.None);
                            variable = variable[1].Split(new[] { "</VALUE>" }, StringSplitOptions.None);
                            //filtered.Append("O ID do utilizador é: " + variable[0] + "\n");
                            c.id = Int32.Parse(variable[0]);
                        }

                        if (filter[p].Contains("<KEY name=\"idnumber\">"))
                        {
                           
                            variable = filter[p].Split(new[] { "<KEY name=\"idnumber\"><VALUE>" }, StringSplitOptions.None);
                            variable = variable[1].Split(new[] { "</VALUE>" }, StringSplitOptions.None);
                            //if(!variable[0].Equals(""))
                            //filtered.Append("O ID do utilizador é: " + variable[0] + "\n");
                            c.idNumber = Int32.Parse(variable[0]);
                        }

                        if (filter[p].Contains("<KEY name=\"enrolledusercount\">"))
                        {
                            variable = filter[p].Split(new[] { "<KEY name=\"enrolledusercount\"><VALUE>" }, StringSplitOptions.None);
                            variable = variable[1].Split(new[] { "</VALUE>" }, StringSplitOptions.None);
                            //filtered.Append("O ID do utilizador é: " + variable[0] + "\n");
                            c.enrolledusercount = Int32.Parse(variable[0]);
                        }

                        if (filter[p].Contains("<KEY name=\"visible\">"))
                        {
                            variable = filter[p].Split(new[] { "<KEY name=\"visible\"><VALUE>" }, StringSplitOptions.None);
                            variable = variable[1].Split(new[] { "</VALUE>" }, StringSplitOptions.None);
                            //filtered.Append("O ID do utilizador é: " + variable[0] + "\n");
                            c.visible = Int32.Parse(variable[0]);
                        }

                        if (filter[p].Contains("<KEY name=\"shortname\">"))
                        {
                            variable = filter[p].Split(new[] { "<KEY name=\"shortname\"><VALUE>" }, StringSplitOptions.None);
                            variable = variable[1].Split(new[] { "</VALUE>" }, StringSplitOptions.None);
                            //filtered.Append("O ID do utilizador é: " + variable[0] + "\n");
                            c.shortName = variable[0];
                        }

                        if (filter[p].Contains("<KEY name=\"fullname\">"))
                        {
                            variable = filter[p].Split(new[] { "<KEY name=\"fullname\"><VALUE>" }, StringSplitOptions.None);
                            variable = variable[1].Split(new[] { "</VALUE>" }, StringSplitOptions.None);
                            //filtered.Append("O ID do utilizador é: " + variable[0] + "\n");
                            c.fullName = variable[0];
                        }


                        if (filter[p].Contains("<KEY name=\"summary\">"))
                        {
                            variable = filter[p].Split(new[] { "<KEY name=\"summary\"><VALUE>&lt;p&gt;" }, StringSplitOptions.None);
                            if (variable.Length < 1)
                            {
                                variable = variable[1].Split(new[] { "&lt;/p&gt;</VALUE>" }, StringSplitOptions.None);
                                //filtered.Append("O ID do utilizador é: " + variable[0] + "\n");

                                c.summary = variable[0];
                            }
                        }

                        p += 1;
                    }
                    c.lang = lang;
                    courses.Add(c);
                    c = null;
                }
            }

        }

        
        /**
         * Filtra informacao relativa as notas do aluno 
         **/
        public void filterGrades(String content)
        {
            String[] filter = content.Split('\n');
            String[] variable;
            int p, id;
            Course placeHolder = null;
            for (int l = 0; l < filter.Length; l++)
            {
                if (filter[l].Contains("<SINGLE>")) // Foi encontrada uma cadeira
                {
                    p = l + 1;

                    while (p < filter.Length && !filter[p].Contains("</SINGLE>"))
                    {

                        if (filter[p].Contains("<KEY name=\"courseid\">"))
                        {
                            variable = filter[p].Split(new[] { "<KEY name=\"courseid\"><VALUE>" }, StringSplitOptions.None);
                            variable = variable[1].Split(new[] { "</VALUE>" }, StringSplitOptions.None);
                            //filtered.Append("O ID do utilizador é: " + variable[0] + "\n");
                            id = Int32.Parse(variable[0]);
                            foreach (Course c in courses)
                                if (c.id == id)
                                {
                                    placeHolder = c;
                                }
                        }

                        if (filter[p].Contains("<KEY name=\"grade\"><VALUE>"))
                        {
                            variable = filter[p].Split(new[] { "<KEY name=\"grade\"><VALUE>" }, StringSplitOptions.None);
                            variable = variable[1].Split(new[] { "</VALUE>" }, StringSplitOptions.None);
                            //filtered.Append("O ID do utilizador é: " + variable[0] + "\n");
                            double n;
                            bool isNumeric = double.TryParse(variable[0], out n);

                            if (placeHolder != null && isNumeric)
                                placeHolder.grade = double.Parse(variable[0]);

                        }



                        p += 1;
                    }
                    placeHolder = null;
                }
            }
        }

        public void doneWriting()
        {
            readyForRead = true;
        }

        public void needsWriting()
        {
            readyForRead = false;
        }
    }
}
