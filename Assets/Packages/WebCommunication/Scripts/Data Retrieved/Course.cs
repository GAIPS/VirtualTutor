using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
//using VT;

namespace UserInfo
{
    public class Course
    {
        
//        int position = 0;

        public Boolean avaliacaoContinua = false;
        //class que define tudo da cadeira

        public int visible; // se a cadeira eh visivel ao user
        public int id; // verdadeiro ID
        public int idNumber; // ID dado para identificacao
        public String shortName; // nome curto
        public String fullName; // nome completo
        public int enrolledusercount; // numero de alunos inscritos
        public String summary; // descricao da cadeira
        public double grade = -1.00; // nota dada ao user
        public String lang;

        public LinkedList<group> groups = new LinkedList<group>();
        public LinkedList<Topic> topics = new LinkedList<Topic>();
        public LinkedList<Folio> folios = new LinkedList<Folio>();
        public LinkedList<Forum> forums = new LinkedList<Forum>();
        public Notes notes = new Notes();
        //valores relativos a cadeira
        [Serializable]
        public class dbValues
        {
            public int courseId;
            public int post_high;
            public int post_low;
            public int login_high;
            public int login_low;
            public int aprov_high;
            public int aprov_low;
            public int startCourseDate;
            public int login_importance;
            public int post_importance;
            public int assid_high;
            public int assid_low;
        }
        public dbValues parameters = null;
        // valores relativos ao aluno
        public int averageLoginSpace = 0;
        public int currentAprov;
        public int maxCurrentAprov;

        // Dados iniciados para corresponder aos sliders
        public float like;
        public float know;
        public float importance;

        //private List<Checkpoint> checkpoints = new List<Checkpoint>();
        //public List<Checkpoint> Checkpoints
        //{
        //    get
        //    {
        //        return this.checkpoints;
        //    }
        //    set
        //    {
        //        checkpoints = value;
        //    }
        //}

        public List<int> logins = new List<int>();
        public Boolean newsAttempt = false;
        public List<newsUpdate> news = new List<newsUpdate>();
        public List<newsUpdate> moduleNews = new List<newsUpdate>();
        public List<newsUpdate> forumNews = new List<newsUpdate>();
        public List<newsUpdate> folioNews = new List<newsUpdate>();

        public class newsUpdate
        {
            public object component;
            public int cmid;
            public int time;
            public String news;
        }

        public class Folio
        {

            public int id;
            public int cmid;
            public int course;
            public String name;
            public int nosubmissions; // se houve submissoes
            public DateTime duedate;
            public DateTime allowsubmissionsfromdate; // data que o aluno pode fazer submissoes
            public double grademax; // grademax
            public DateTime cutoffdate; // prazo limite

            // dados do modulo
            public int visible;
            public int uservisible;
            public int visibleOnCoursePage;

            // grade data
            public List<attemptgrade> attempgrade = new List<attemptgrade>();

            // Dados usados para notificar o aluno de entregas
            public Boolean isDue = true; // se o aluno entregou algo
            public String dueInformation;

            internal int grademin;
            internal double graderaw;
            internal string gradeformatted;
            internal string weightformatted;
            internal string percentageformatted;
            internal DateTime gradedategraded;
            internal DateTime gradedatesubmitted;
        }

        public class attemptgrade
        {
            public int id;
            public int attemptnumber; // a tentativa nº
            public Double grade; // if -1 entao nao foi dada
            public DateTime timecreated; // submissao do aluno?
            public DateTime timemodified; // avaliacao do professor?

        }

        public class contents //conteudos de um modulo
        {
            // caso seja um ficheiro
            public String type;
            public String fileName;
            public String summary;
            public int fileSize;
            public String filePath; // Location in Moodle
            public String fileUrl; // Download Link
            public int isExternalFile;

            public DateTime timeCreated;
            public DateTime timeModified;

            public int userId;
            public String author;
        }

       public class modules // modulos de uma cadeira
        {
            public int id;
            public String url;
            public String name;
            public String description;
            public int visible;
            public int uservisible;
            public int visibleOnCoursePage;
            public String modname; // maybe useful

            public LinkedList<contents> contents = new LinkedList<contents>();
        }


        public class Topic // topico de uma cadeira
        {
            public int visible; // check
            public int userVisible; // check
            public int id; // check
            public String name; // CHECK
            public String summary; // check
            public int hiddenByNumSections; // maybe useful
            public String url;
            public String availabilityinfo;
            public LinkedList<modules> modules = new LinkedList<modules>();
        }

        public class group
        {
            public int id;
            public String name;
        }

        public class Posts
        {
            public int id;//Post id
            public int discussion;//Discussion id
            public int parent;//Parent id
            public int userid;//User id
            public int created;//Creation time
            public int modified;//Time modified
            public String subject;//The post subject
            public String message;//The post message
        }

        public class Discussions
        {
            public int id;
            public string name; //Discussion name
            public int timemodified;  //Time modified X
            public String subject; //subject of message
            public String message; //message posted
            public String userfullname; //Post author full name
            public String usermodifiedfullname; //Post modifier full name
            public int created;//Creation time
            public int modified;//Time modified X
            public int userid; // id of user who created the discussion
            public List<Posts> posts;
        }
        public class Forum
        {
            public int id; //Forum id
            public int cmid;
            public String type; //The forum type
            public String name; //Forum name
            public String intro;  //The forum intro
            //public List<contents> introfiles;
            public List<Discussions> discussions;
        }

        /**
         * Devolve toda a info relevante ao curso
         * */
        public String toString()
        {
            StringBuilder response = new StringBuilder();
            foreach( Topic t in topics)
            {
               
                response.Append(TopicDisplay(t.id, lang));
                    foreach (modules m in t.modules)
                    {

                            response.Append(ModuleDisplay(t.id, m.id, lang));

                            foreach (contents c in m.contents)
                            {
                                response.Append("Content\n");
                                if (c != null)
                                {
                                    //response.Append(c.userId + "\n");
                                    response.Append("Author: " +c.author + "\n");
                                    //response.Append(c.type + "\n");
                                    response.Append("Name " +c.fileName + "\n");
                                    response.Append("URL: " +c.fileUrl + "\n");
                                    //response.Append(c.timeCreated + " \n");
                                }
                            }
                        
                    }
                
            }
            response.Append("FOLIOS\n");
            response.Append(foliosToString());
            
            return response.ToString();
        }

        

        // METODOS PARA CONVERTER EM FORMATO LEGIVEL O CONTEUDO
        public String TopicDisplay(int id, String lang)
        {
            Topic t = GetTopic(id);
            
            if (t == null)
                return null;
            
            StringBuilder s = new StringBuilder();
            if(t.userVisible==1)
            {
               
                if (lang.Equals("en"))
                {
                    s.Append("Topic ID:" + t.id + "\n");
                    s.Append("Topic Name:" + t.name + "\n");
                    s.Append("Brief summary :" + t.summary + "\n");
                    s.Append("URL:" + t.url + "\n");
                }
                else
                {
                    s.Append("ID do tópico:" + t.id + "\n");
                    s.Append("nome:" + t.name + "\n");
                    s.Append("Breve sumário :" + t.summary + "\n");
                    s.Append("URL:" + t.url + "\n");
                }
                
            }
            
            return s.ToString();
        }

        public String ModuleDisplay(int topicId, int id, String lang)
        {
            Topic t = GetTopic(topicId);
            if (t == null)
                return null;
            modules m = getModule(topicId, id);
            if (m == null)
                return null;
           
            StringBuilder s = new StringBuilder();
            if (m.uservisible == 1)
            {
                if (lang.Equals("en"))
                {
                    s.Append("Name " + m.name + "\n");
                    s.Append("ID:" +m.id + "\n");
                    s.Append("URL: " + m.url + "\n");
                    if (m.description != null)
                        s.Append("Description: " + m.description + "\n");
                    else
                        s.Append("No description was given.\n");
                    foreach (contents c in m.contents)
                    {
                        s.Append("\nContent:\n");
                        if (c != null)
                        {
                            //response.Append(c.userId + "\n");
                            s.Append("Author: " + c.author + "\n");
                            //response.Append(c.type + "\n");
                            s.Append("Name " + c.fileName + "\n");
                            s.Append("URL: " + c.fileUrl + "\n");
                            //response.Append(c.timeCreated + " \n");
                        }
                    }

                }
                else
                {
                    s.Append("Nome" + m.name + "\n");
                    s.Append("ID:" + m.id + "\n");
                    s.Append("URL: " + m.url + "\n");
                    s.Append("Descrição: " + m.description + "\n");
                    foreach (contents c in m.contents)
                    {
                        s.Append("Conteudo:\n");
                        if (c != null)
                        {
                            //response.Append(c.userId + "\n");
                            s.Append("Autor: " + c.author + "\n");
                            //response.Append(c.type + "\n");
                            s.Append("Nome " + c.fileName + "\n");
                            s.Append("URL: " + c.fileUrl + "\n");
                            //response.Append(c.timeCreated + " \n");
                        }
                    }
                }

            }
            
            return s.ToString();
        }

        //info de um folio em string
        public String FolioDisplay(int folioId)
        {
            TimeSpan diff;
            StringBuilder s = new StringBuilder();
            Folio f = getFolio(folioId);
            
            s.Append("Name: " + f.name + "\n");
            s.AppendLine("id " + f.id);
            s.AppendLine("cmid " + f.cmid);

            s.Append("Due date: " + f.duedate.ToLocalTime().ToString("d/M/yyyy") + "\n");
            diff = DateTime.UtcNow.Subtract(f.duedate);
            if (diff.TotalSeconds < 0) // Data ainda nao passou 
            {
                diff = -diff;
                s.Append("You still have " + diff.Days + " days, " + diff.Hours + " hours and " + diff.Minutes + " minutes\n");
            }
            else
            {
                s.Append("DUE TIME HAS PASSED!!!\n");
            }
            if (f.graderaw != 0)
                s.AppendLine("Grade: " + f.graderaw + " out of " + f.grademax +" => " + f.percentageformatted);
            else
            {
                s.Append("NO grades were given");
            }


            //if (lang.Equals("en")) {
            //    s.Append("ID: " + f.id + "\n");
            //    s.Append("Name: " + f.itemName + "\n");
            //    s.Append("Status:" + f.status + "\n");
            //    if (f.gradedatesubmitted.Equals(new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc)))
            //        s.Append("YOU HAVENT SUBMITTED ANYTHING\n");
            //    else
            //        s.Append("Submitted date: " + f.gradedatesubmitted.ToLocalTime() + "\n");
            //    if (f.gradeGiven) {
            //        s.Append("Grade: " + f.graderaw + " out of " + f.grademax + "= " + f.percentageFormatted + "\n");
            //        s.Append("WAS GRADED AT: " + f.gradedategraded.ToLocalTime());
            //    }
            //    else
            //    {
            //        s.Append("Grade not given\n");
            //    }
            //}
            //else
            //{
            //    s.Append("ID: " + f.id + "\n");
            //    s.Append("Nome: " + f.itemName + "\n");
            //    if (f.gradeGiven)
            //    {
            //        s.Append("Nota: " + f.graderaw + " de " + f.grademax + "= " + f.percentageFormatted + "\n");
            //    }
            //    else
            //    {
            //        s.Append("Falta de nota\n");
            //    }
            //}

            return s.ToString();
        }


        /**
         * Metodo que retorna o valor maximo de aproveitamento que o aluno pode ter num dado instante
         * 
         * */
        public int maxCurrentAproveitamento()
        {
            int result = 0;
            foreach(Folio f in folios)
            {
                if (f.graderaw != -1)
                {
                    result += (Convert.ToInt32(f.grademax) * (100 / Convert.ToInt32(f.grademax))); // pode nao ter chegado a data limite mas o professor ter avaliado
                }
            }

            return result;
        }

        public int currentAproveitamento()
        {
            int result = 0;
            foreach (Folio f in folios)
            {
                if (f.graderaw != -1)
                {
                    result += Convert.ToInt32(f.graderaw * (100 / Convert.ToInt32(f.grademax))); // pode nao ter chegado a data limite mas o professor ter avaliado
                }
            }

            return result;
        }

        public class gradeStruct
        {
            public String name;
            public double grade;// -2 se nao entregou nada ainda, -1 se entregou mas nao foi avaliado
            public double gradeMax;
            public int position;
            public DateTime dueDate;
            public Boolean isGraded = true;

            public gradeStruct(String name, double grade, double gradeMax, int position, DateTime dueDate)
            {
                this.name = name;
                this.grade = grade;
                if (grade == -1)
                    isGraded = false;
                this.gradeMax = gradeMax;
                this.position = position;
                this.dueDate = dueDate;
            }
        }

        
        public List<gradeStruct> getGradeValues()
        {
            List<gradeStruct> grades = new List<gradeStruct>();
            gradeStruct template;
            foreach(Folio f in folios)
            {
                template = new gradeStruct(f.name,getHighestGradeFolio(f.id),f.grademax,0,f.duedate);

                foreach(gradeStruct g in grades)
                {

                    if (template.dueDate < g.dueDate && (template.position > g.position || template.position == 0))
                    {
                        template.position = g.position;
                        g.position++;
                    }
                    else if (template.dueDate < g.dueDate && template.position < g.position)
                        g.position++;
                }
                if (template.position == 0)
                    template.position = grades.Count;
                grades.Add(template);
            }
            grades.Sort((s1,s2) => s1.position.CompareTo(s2.position));
            return grades;
        }


        public double getHighestGradeFolio(int folioId)
        {
            Folio f = getFolio(folioId);
            double grade = -1; 
            if(f!= null)
            {
                if (f.attempgrade.Count == 0)
                return -2; // NAO HOUVE ENTREGAS

                foreach(attemptgrade a in f.attempgrade)
                {
                    if (a.grade > grade)
                        grade = a.grade;
                }
            }
            

            return grade;
        }

        // Da toda a info dos folios em string
        public String foliosToString()
        {
            StringBuilder response = new StringBuilder();
            //foreach (Folio f in folios)
            //{
            //    response.Append("NAME: " + f.itemName + "\n");
            //    response.Append("MINIMO: " + f.grademin + "\n");
            //    response.Append("Maximo: " + f.grademax + "\n");
                
            //    if (f.gradeGiven)
            //    {
            //        response.Append("DATE: " + f.gradedategraded.ToLocalTime() + " value " + f.gradeformatted + "\n");
            //        response.Append(f.percentageFormatted + "\n");
            //    }
            //    else
            //        response.Append("Grade wasn't given yet.\n");
            //    response.Append("----------------\n");
            //}

            foreach(Folio f in folios)
            {
                if (f.visible>0)
                    response.AppendLine(f.dueInformation);
            }

            return response.ToString();
        }

        // Metodos para buscar valores
        public Topic GetTopic(int id)
        {
            
            
            foreach (Topic to in topics)
            {

                if (to.id == id)
                   return to;
            }
            return null;
        }

        public modules getModule(int tId,int id)
        {
            Topic t = GetTopic(tId);
            
            foreach (modules mo in t.modules)
            {
                if (mo.id == id)
                {
                    return mo;
                }
            }
            return null;
            
        }

        public modules getModuleById(int id)
        {
            modules placeholder;
            foreach(Topic t in topics)
            {
               placeholder= getModule(t.id, id);
                if (placeholder != null)
                    return placeholder;
            }
            return null;
        }

        public void removeModule(int id)
        {
            
            modules m = getSpecificModule(id);
            
            foreach(Topic t in topics)
            {
               
                if (t.modules.Contains(m))
                    t.modules.Remove(m);
            }

        }

        public modules getSpecificModule(int id)
        {
            //Topic t = GetTopic(tId);
            foreach (Topic t in topics)
            {

                foreach (modules mo in t.modules)
                {
                   
                    if (mo.id == id)
                    {
                        
                        return mo;
                    }
                }
            }
            return null;

        }

        public Folio getFolio(int fId)
        {
            
            foreach(Folio fo in folios)
            {
                if (fo.cmid == fId)
                    return fo;
            }

            return null;
        }

        public Forum getForum(int fId)
        {
            foreach (Forum fo in forums)
            {
                if (fo.cmid == fId)
                    return fo;
            }

            return null;
        }

        /**
         * UM MODULO PODE SER UM OBJECTO DO TIPO FOLIO OU FORUM OU ENTAO UM SIMPLES MODULO
         * Quando somos informados de um update o contexto modulo não define que tipo é portanto é necessario um getter ambiguo
         * */
        public object getUndefinedModule(int id)
        {
            modules m = getSpecificModule(id);
            if (m != null)
                return m;
            Folio f = getFolio(id);
            if (f != null)
                return f;
            Forum fo = getForum(id);
            if (fo != null)
                return fo;

            return null;
        }
        public Discussions GetDiscussions(int fId, int discId)
        {
            Forum f = getForum(fId);
            if(f != null)
            {
                foreach(Discussions d in f.discussions)
                {
                    if (d.id == discId)
                        return d;
                }
            }
            return null;
        }

        // METODOS PARA RECEBER WEBSERVICE RESPONSES JSON

        public void receiveCourseTopics(List<jsonValues.Topics> topics)
        {
            Topic placeHolder = null;
            modules moduleP = null;
            contents contentP = null;
            this.topics = new LinkedList<Topic>();
            foreach (jsonValues.Topics t in topics)
            {
                if (t.visible > 0)
                {
                    placeHolder = new Topic();
                    placeHolder.id = t.id;
                    placeHolder.name = t.name;
                    placeHolder.summary = HtmlDecode(t.summary);
                    placeHolder.visible = t.visible;
                    placeHolder.userVisible = (t.uservisible.Equals("true")) ? 1 : 0;
                    placeHolder.availabilityinfo = t.availabilityinfo;
                    foreach (jsonValues.Modules m in t.modules)
                    {
                        if (m.visibleoncoursepage > 0 & m.visible>0)
                        {
                            moduleP = new modules();
                            moduleP.id = m.id;
                            moduleP.name = m.name;
                            if (m.description != null)
                                moduleP.description = HtmlDecode(m.description);
                            moduleP.visible = m.visible;
                            moduleP.url = m.url;
                            moduleP.uservisible = (m.uservisible.Equals("true")) ? 1 : 0;
                            moduleP.visibleOnCoursePage = m.visibleoncoursepage;
                            
                            foreach (jsonValues.Contents c in m.contents)
                            {
                                contentP = new contents();
                                contentP.fileUrl = c.fileurl;
                                contentP.fileSize = c.filesize;
                                contentP.filePath = c.filepath;
                                contentP.fileName = c.filename;
                                contentP.isExternalFile = c.isexternalfile;
                                contentP.type = c.type;
                                contentP.author = c.author;
                                contentP.userId = c.userid;
                                if (c.summary != null)
                                    contentP.summary = HtmlDecode(c.summary);

                                //TODO Times to be ADDED
                                moduleP.contents.AddLast(contentP);
                            }
                            placeHolder.modules.AddLast(moduleP);
                        }
                    }
                    this.topics.AddLast(placeHolder);
                }
            }

        }


        public void receiveGrades(jsonValues.usergrades grades)
        {

            Folio template;
            //folios = new LinkedList<Folio>();
            DateTime date;
            foreach (jsonValues.gradeitems f in grades.gradeitems)
            {
                if (f.itemmodule.Equals("assign")) {
                    template = getFolio(f.cmid); 
                    if(template != null)
                    {
                        template.grademin = f.grademin;
                        template.grademax = f.grademax;
                        if(f.graderaw == null)
                            template.graderaw = -1; // ainda nao recebeu avaliacao
                        else if (f.graderaw.Equals(""))
                            template.graderaw = -1; // ainda nao recebeu avaliacao
                        else
                            template.graderaw = Convert.ToDouble(f.graderaw);

                        template.gradeformatted = f.gradeformatted;
                        template.weightformatted = f.weightformatted;
                        template.percentageformatted = f.percentageformatted;

                        if (f.gradedategraded != 0)
                        {
                            date = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                            date = date.AddSeconds(Convert.ToInt32(f.gradedategraded));
                            template.gradedategraded = date;
                        }
                       
                        if(f.gradedatesubmitted != 0)
                        {
                            date = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                            date = date.AddSeconds(Convert.ToInt32(f.gradedategraded));
                            template.gradedatesubmitted = date;
                        }

                    }
                }
                //template = new Folio();
                //template.itemName = f.itemname;
                //template.id = f.id;
                //template.grademin = f.grademin;
                //template.grademax = f.grademax;

                //template.status = f.status;
                //if (f.graderaw != 0) // foi dada nota
                //{
                //    template.graderaw = f.graderaw;
                //    template.gradeformatted = f.gradeformatted;
                //    template.percentageFormatted = f.percentageformatted;
                //    template.gradeGiven = true;

                //    date = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                //    date = date.AddSeconds(Convert.ToInt32(f.gradedategraded));
                //    template.gradedategraded = date;




                //}
                //date = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                //date = date.AddSeconds(Convert.ToInt32(f.gradedatesubmitted));
                //template.gradedatesubmitted = date;
                //template.feedback = f.feedback;
                //folios.AddLast(template);
            }

        }

        public void receiveAssignments(List<jsonValues.assignments> assignments)
        {
            
            Folio template;
            modules m;
            DateTime date;
            TimeSpan diff;
            foreach (jsonValues.assignments a in assignments)
            {
                m = getSpecificModule(a.cmid);
                
                if (m != null) //o modulo tem de existir na lista, senao eh por nao ser visivel
                {
                    template = new Folio();
                    template.visible = m.visible;
                    template.uservisible = m.uservisible;
                    template.visibleOnCoursePage = m.visibleOnCoursePage;
                    template.id = a.id;
                    template.cmid = a.cmid;
                    template.course = a.course;
                    template.name = a.name;
                    template.nosubmissions = a.nosubmissions;
                    template.grademax = a.grade;

                    date = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                    date = date.AddSeconds(Convert.ToInt32(a.duedate));
                    template.duedate = date;

                    date = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                    date = date.AddSeconds(Convert.ToInt32(a.allowsubmissionsfromdate));
                    template.allowsubmissionsfromdate = date;

                    date = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                    date = date.AddSeconds(Convert.ToInt32(a.cutoffdate));
                    template.cutoffdate = date;
                    if (m != null)
                        removeModule(a.cmid);

                    diff = DateTime.UtcNow.Subtract(template.duedate);

                    if (diff.TotalSeconds < 0) // Data ainda nao passou 
                    {
                        diff = -diff;
                        template.dueInformation = "";
                        if (diff.Days == 0)
                            template.dueInformation = "A entrega do efolio " + template.name + " é daqui a " + diff.Hours + " horas\n";
                        else
                            template.dueInformation = "Faltam " + diff.Days + " dias para a entrega do efolio " + template.name;

                    }
                    else
                    {
                        template.dueInformation = "O prazo de entrega do efolio " + template.name + " já passou\n";
                    }



                    folios.AddLast(template);
                }
                
            }
        }

        public void receiveAssignmentsGrade(List<jsonValues.assignments> assignments, int userid)
        {
            attemptgrade template;
            Folio f;
            DateTime date;
            foreach(jsonValues.assignments a in assignments) // em todos os assignments
            {
                f = getFolio(a.assignmentid); // encontrar folio/assignment mencionado
                if(f != null) // caso tenha encontrado
                {
                   foreach(jsonValues.grades g in a.grades) // todas as notas desse assignment
                    {
                        if(g.userid == userid) // so notas que pertencem ao aluno
                        {
                            template = new attemptgrade();
                            template.attemptnumber = g.attemptnumber;
                            template.id = g.id;
                            
                            if (Double.TryParse(g.grade, out template.grade)) // se verdade entao conseguiu fazer parse
                                template.grade = Double.Parse(g.grade); //NOTE: Nao eh necessario, ja foi parsed
                            else
                                template.grade = -1;// se for -1 entao nota nao foi dada
                            date = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                            date = date.AddSeconds(Convert.ToInt32(g.timecreated));
                            template.timecreated = date;

                            date = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                            date = date.AddSeconds(Convert.ToInt32(g.timemodified));
                            template.timemodified = date;

                            f.isDue = false; // houve alguma entrega

                            f.attempgrade.Add(template);
                        }
                    }

                }
            }
        }

        public void receiveForums(List<jsonValues.forums> forums)
        {
            Forum template;
            modules placeholder;
            this.forums = new LinkedList<Forum>();
            foreach(jsonValues.forums f in forums)
            {
                placeholder = getModuleById(f.cmid);
                if (placeholder != null)
                    removeModule(placeholder.id);
                template = new Forum();
                template.id = f.id;
                template.cmid = f.cmid;
                
                template.intro = f.intro;
                template.name = f.name;

                template.type = f.type;
                template.discussions = new List<Discussions>();
                this.forums.AddLast(template);
            }
        }

        public void receiveDiscussions(List<jsonValues.discussions> discussions, int forumcmid)
        {
            
            Forum f = getForum(forumcmid);
            Discussions template;
            foreach(jsonValues.discussions d in discussions)
            {
                template = new Discussions();
                template.id = d.discussion;
                template.created = d.created;
                template.message = HtmlDecode(d.message);
                template.modified = d.modified;
                template.name = d.name;
                template.subject = HtmlDecode(d.subject);
                template.timemodified = d.timemodified;
                template.userfullname = d.userfullname;
                template.userid = d.userid;
                template.usermodifiedfullname = d.usermodifiedfullname;
                template.posts = new List<Posts>();
                f.discussions.Add(template);
            }
        }

        public void receivePosts(List<jsonValues.posts> posts, int forumid,int discID)
        {
            Discussions d = GetDiscussions(forumid, discID);
            if(d != null)
            {
                Posts template;

                foreach(jsonValues.posts p in posts)
                {
                    template = new Posts();
                    template.created = p.created;
                    template.discussion = p.discussion;
                    template.id = p.id;
                    template.message = HtmlDecode( p.message);
                    template.modified = p.modified;
                    template.parent = p.parent;
                    template.subject = p.subject;
                    template.userid = p.userid;
                    d.posts.Add(template);
                }
            }
        }

        public void receiveGroups(List<jsonValues.groups> groups)
        {
            group template;
            foreach (jsonValues.groups g in groups)
            {
                template = new group();
                template.id = g.id;
                template.name = HtmlDecode(g.name);
                this.groups.AddLast(template);
            }
        }

        public void receiveUpdates(List<jsonValues.instances> instances, int lastLogin)
        {
            newsUpdate template;
            foreach(jsonValues.instances i in instances)
            {
                template = new newsUpdate();
                template.time = 0;
                if (i.contextlevel.Equals("module"))
                {
                    template.cmid = i.id;
                    template.component = getUndefinedModule(i.id);
                }


                foreach(jsonValues.updates u in i.updates)
                {
                    if (u.name.Equals("configuration"))
                        template.time = u.timeupdated;

                    if (u.name.Equals("usergrades"))
                    {
                        template.news = "O e-fólio " + ((template.component) as Folio).name + " foi avaliado";
                    }

                    if (u.name.Equals("discussions"))
                    {
                        template.news = "O Forum " + ((template.component) as Forum).name + " teve alguma adição";
                    }

                    if (u.name.Equals("contentfiles"))
                    {
                        template.news = "Houve adições ao Módulo " + template.cmid;
                    }

                    if (u.name.Equals("introattachmentfiles"))
                    {
                        template.news = "Houve adições na introdução do Módulo " + template.cmid;
                    }
                }
                if (template.component == null)
                    template.news = "Foi removido";
                else if(template.component != null && template.news == null)
                    template.news = "Houve adições ao Módulo " + template.cmid;
                if (template.time == 0)
                    template.time = lastLogin;

                news.Add(template);

                if (template.component.GetType().ToString().ToLower().Contains("forum"))
                    forumNews.Add(template);

                if (template.component.GetType().ToString().ToLower().Contains("folio"))
                    folioNews.Add(template);

                if (template.component.GetType().ToString().ToLower().Contains("module"))
                    moduleNews.Add(template);
            }
            newsAttempt = true;
        }

        public void compareUpdates(jsonValues.modulesViewed mod)
        {
            foreach (newsUpdate n in news)
            {
                if(n.cmid == mod.contextinstanceid)
                {
                    if (n.time < mod.timecreated)
                    {
                        if (moduleNews.Contains(n))
                            moduleNews.Remove(n);
                        if (folioNews.Contains(n))
                            folioNews.Remove(n);
                        if (forumNews.Contains(n))
                            forumNews.Remove(n);
                        news.Remove(n);
                    }
                }
            }
        }

        public int getAverageLoginSpace()
        {
            logins.Sort();
            int count = 1;
            int average=0;
            for(count = 1; count < logins.Count; count++)
            {
                average += logins[count] - logins[count - 1];
            }


            averageLoginSpace = (average / count);
            return (average/count);
        }
        
        public List<newsUpdate> GetUpdatedForuns()
        {
            return forumNews;
        }

        public List<newsUpdate> GetUpdatedFolios()
        {
            return folioNews;
        }

        public List<newsUpdate> GetUpdatedModules()
        {
            return moduleNews;
        }

        public static string HtmlDecode(string value)
        {
            String v;
            String[] s = value.Split('\n');
            StringBuilder sb = new StringBuilder();
            foreach (String st in s)
            {
                v = st.Replace("&amp;", "&").Replace("&lt;", "<").Replace("&gt;", ">").Replace("&quot;", "\"").Replace("&apos;", "'").Replace("<br />", "").Replace("<hr />", "").Replace("<h3>", "").Replace("</h3>", "").Replace("<li>", "").Replace("</li>", "")
                    .Replace("<ol>", "").Replace("</ol>", "").Replace("<span>", "").Replace("<b>", "").Replace("</b>", "").Replace("</p>", "").Replace("</span>", "").Replace("<u>", "").Replace("</u>", "").Replace("<ul>", "").Replace("</ul>", "").Replace("</div>", "")
                    .Replace("</a>", "").Replace("<strong>", "").Replace("</strong>", "").Replace("<hr>", "").Replace("<br>", "").Replace("</font>", "").Replace("\u00ed", "í").Replace("\u00fa", "ú");

                v = Regex.Replace(v, @"\<span(.*?)\>", "");
                v = Regex.Replace(v, @"\<p(.*?)\>", "");
                v = Regex.Replace(v, @"\<a(.*?)\>", "");
                v = Regex.Replace(v, @"\<div(.*?)\>", "");
                v = Regex.Replace(v, @"\<img(.*?)\>", "");
                v = Regex.Replace(v, @"\<font(.*?)\>", "");

                sb.Append(v);
            }
            

            return sb.ToString();

        }
    }
}
