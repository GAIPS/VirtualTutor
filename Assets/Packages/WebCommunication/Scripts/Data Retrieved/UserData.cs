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
        public int datelast;
        //current localtime
        DateTime localTime = DateTime.Now;
        TimeSpan diff;
        public Boolean readyForRead = false;

        //informacao relativa as cadeiras do user
        //public List<Course> courses = new List<Course>();

        //public Course course = new Course();

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        //public String giveData()
        //{
        //    StringBuilder filtered = new StringBuilder();
        //    filtered.Append("TIME: " + localTime.ToLocalTime() + "\n");
        //    diff = DateTime.UtcNow.Subtract(datelast);
        //    // dados do user
        //    if (lang.Equals("en"))
        //    {
        //        if (localTime.ToLocalTime().Hour < 12)
        //            filtered.Append("Good Morning " + fullName + "\n");
        //        else if (localTime.ToLocalTime().Hour >= 12 && localTime.ToLocalTime().Hour < 20)
        //            filtered.Append("Good Afternoon " + fullName + "\n");
        //        else
        //            filtered.Append("Good Night " + fullName + "\n");
        //        //filtered.Append("First time you entered in Moodle was: " + datefirst.ToLocalTime() + "\n");
        //        filtered.Append("Last time you entered in Moodle was: " + datelast.ToLocalTime() + "\n");
                
        //        filtered.Append("You haven't gone on Moodle in: " + diff.Hours + " hours " + diff.Minutes + " minutes and " + diff.Seconds + " seconds"  + "\n");

        //        foreach (Course c in courses)
        //        {
        //            foreach (Course.Forum f in c.forums)
        //            {
        //                foreach (Course.Discussions d in f.discussions)
        //                {
        //                    foreach (Course.Posts p in d.posts)
        //                    {

        //                        if (p.userid == id)
        //                        {
        //                            DateTime date = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
        //                            date = date.AddSeconds(Convert.ToInt32(p.created));
        //                            filtered.Append("\nOn Forum " + f.name + " There's the Discussion " + d.name + " on which you posted: " + p.message + " at " + date.ToLocalTime());
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    else
        //    {
        //        if (localTime.ToLocalTime().Hour < 12)
        //            filtered.Append("Bom dia " + fullName + "\n");
        //        else if (localTime.ToLocalTime().Hour >= 12 && localTime.ToLocalTime().Hour < 20)
        //            filtered.Append("Boa tarde " + fullName + "\n");
        //        else
        //            filtered.Append("Boa noite " + fullName + "\n");

        //        filtered.Append("Informação de cursos:\n");

                
        //    }

        //    filtered.Append("efolios:\n");
        //    foreach (Course c in courses)
        //    {
        //        filtered.Append(c.fullName + "\n");
        //        filtered.Append(c.foliosToString());
        //        filtered.Append("---------------\n");
        //    }

        //    return filtered.ToString();
        //}

        // METODOS PARA CONVERTER EM FORMATO LEGIVEL O CONTEUDO

        //public String courseDisplay(int courseId)
        //{
        //    Course c = getCourse(courseId);

        //    StringBuilder s = new StringBuilder();

        //    if(c.visible == 1)
        //    {
        //        if (lang.Equals("en"))
        //        {
        //            s.Append(c.fullName + "\n");
        //            s.Append("ID:" + c.id + "\n");
        //            s.Append("Sumary: " + c.summary + "\n");
        //            if (c.grade == -1)
        //                s.Append("No grade was yet given\n");
        //            else
        //                s.Append("Grade: " + c.grade + "\n");
        //            //s.Append(c.toString());
                   
        //        }


        //        else
        //        {
        //            s.Append("ID da cadeira:" + c.id + "\n");
        //            s.Append("Nome da cadeira: " + c.fullName + "\n");
        //            s.Append("Nomenclatura: " + c.shortName + "\n");
        //            s.Append("Descricao: " + c.summary + "\n");
        //            if (c.grade == -1)
        //                s.Append("Avaliacao ainda nao foi feita\n");
        //            else
        //                s.Append("Nota: " + c.grade + "\n");
        //            //s.Append(c.toString());
        //        }
                
        //        s.Append("NOTAS:\n");
                
        //        foreach (Course.Folio f in c.folios) {
                    
        //            if(f.duedate<DateTime.UtcNow && c.getHighestGradeFolio(f.id) ==-2)
        //            {
        //                s.AppendLine(f.name + " ja passou do prazo e naõ foi entregue nada :_(");
        //            }
        //            else if(f.duedate < DateTime.UtcNow && c.getHighestGradeFolio(f.id) ==-1)
        //            {
        //                s.AppendLine(f.name + " ja passou do prazo mas ainda não foi avaliada");
        //            }
        //            else if (f.duedate < DateTime.UtcNow)
        //            s.AppendLine(f.name + " " + c.getHighestGradeFolio(f.id) + " em " + f.grademax);
        //            else if (f.duedate > DateTime.UtcNow && f.attempgrade.Count>0)
        //            {
        //                s.AppendLine(f.name + " ainda tem tempo antes da entrega, reve a tua entrega com calma");
        //            }
        //            else if(f.duedate> DateTime.UtcNow)
        //            {
        //                s.AppendLine(f.name + " ainda tem tempo antes da entrega, toma cuidado");
        //            }

        //        }
        //        List<Course.gradeStruct> grades = c.getGradeValues();
                
        //        // TODO FAZER VECTORES E ANGULOS AQUI
        //        Vector2 v1 = new Vector2(0,0),v2 = new Vector2(0,0),vFirst = new Vector2(0,0), vLast = new Vector2(0, 0);
        //        LinkedList<float> declives = new LinkedList<float>();

        //        float angle;
        //        double bas = 0, superior = 0;
        //        foreach (Course.gradeStruct g in grades)
        //        {
        //            if (g.dueDate<DateTime.UtcNow && g.isGraded) {
        //                //Debug.Log("FOLIO: " + g.name + " grade:" + g.grade + " out of " + g.gradeMax + " duedate:" + g.dueDate.ToLocalTime() + " POSITION:" + g.position);
        //                if (g.position == 0)
        //                {
        //                    v1 = new Vector2(g.position++,(float)(g.grade * (20 / g.gradeMax)));
                           
        //                    vFirst = new Vector2(v1.x,v1.y);
        //                }
        //                else
        //                {
        //                    if (g.position % 2 != 0)
        //                    {
        //                        v2 = new Vector2(g.position++, (float)(g.grade * (20 / g.gradeMax)));

                                
        //                        //sign = (v2.y < v1.y) ? -1.0f : 1.0f;
        //                        angle = Mathf.Atan2(v2.y - v1.y, v2.x - v1.x) * 180 / Mathf.PI;
        //                        declives.AddLast(angle);
        //                        vLast = new Vector2(v2.x, v2.y);
        //                    }
        //                    else
        //                    {
        //                        v1 = new Vector2(g.position++, (float)(g.grade * (20 / g.gradeMax)));
        //                        //sign= (v1.y < v2.y) ? -1.0f : 1.0f;
                                
        //                        angle = Mathf.Atan2(v1.y - v2.y, v1.x - v2.x) * 180 / Mathf.PI;
        //                        declives.AddLast(angle);
        //                        vLast = new Vector2(v1.x, v1.y);
        //                    }

        //                }
        //                bas += g.gradeMax;
        //                superior += g.grade;
        //            }
                    
        //        }

        //        foreach (float d in declives)
        //        {
        //            s.Append("DECLIVE: " + d + "\n");
        //        }
        //        //sign = (vLast.y < vFirst.y) ? -1.0f : 1.0f;
        //        angle = Mathf.Atan2(vLast.y - vFirst.y, vLast.x - vFirst.x) * 180 / Mathf.PI;
        //        s.AppendLine("Performance geral: " + angle);
        //        s.AppendLine(superior + "/" + bas + "=" + (superior / bas));

        //    }
            
        //    return s.ToString();
        //}

        //public String TopicDisplay(int courseId, int topidID)
        //{

        //    Course c = getCourse(courseId);
        //    if (c == null)
        //        return null;

        //    StringBuilder s = new StringBuilder();

        //    s.Append(c.TopicDisplay(topidID,lang));
            
        //    return s.ToString();
        //}

        //public String ModuleDisplay(int courseId, int topicId, int moduleId)
        //{
        //    Course c = getCourse(courseId);
        //    if (c == null)
        //        return null;

        //    StringBuilder s = new StringBuilder();

        //    s.Append(c.ModuleDisplay(topicId,moduleId, lang));

        //    return s.ToString();
        //}

        //public String FolioDisplay(int courseId, int folioID)
        //{

        //    Course c = getCourse(courseId);
        //    if (c == null)
        //        return null;

        //    StringBuilder s = new StringBuilder();

        //    s.Append(c.FolioDisplay(folioID));

        //    return s.ToString();
        //}


        //public Course getCourse(int id)
        //{

        //    foreach (Course co in courses)
        //    {
        //        if (co.id == id)
        //            return co;
        //    }
        //    return null;

        //}



        // METODOS PARA FILTRAR WEBSERVICE RESPONSES

        
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
            
            datelast = u.lastaccess;
            
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
