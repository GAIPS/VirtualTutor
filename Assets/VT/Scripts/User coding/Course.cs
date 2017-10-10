using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using VT;

namespace UserInfo
{
    public class Course
    {
        
        int position = 0;


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

        public LinkedList<Topic> topics = new LinkedList<Topic>();
        public LinkedList<Folio> folios = new LinkedList<Folio>();

        // Dados iniciados para corresponder aos sliders
        public float like;
        public float know;
        public float importance;

        private List<Checkpoint> checkpoints = new List<Checkpoint>();
        public List<Checkpoint> Checkpoints
        {
            get
            {
                return this.checkpoints;
            }
            set
            {
                checkpoints = value;
            }
        }

        public class Folio
        {
            public int id;
            public String itemName;
            public DateTime gradedategraded;
            public bool gradeGiven = false;
            public String gradeformatted; // verificar se nao eh '-'
            public int graderaw;
            public String percentageFormatted;
            public int grademin;
            public int grademax;
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

            public LinkedList<modules> modules = new LinkedList<modules>();
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
               
                s.Append("TOPIC\n");
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
                s.Append("MODULE\n");
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
                        s.Append("Content:\n");
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
            StringBuilder s = new StringBuilder();
            Folio f = getFolio(folioId);
            if (lang.Equals("en")) {
                s.Append("ID: " + f.id + "\n");
                s.Append("Name: " + f.itemName + "\n");
                if (f.gradeGiven) {
                    s.Append("Grade: " + f.graderaw + "out of " + f.grademax + "= " + f.percentageFormatted + "%\n");
                }
                else
                {
                    s.Append("Grade not given\n");
                }
            }
            else
            {
                s.Append("ID: " + f.id + "\n");
                s.Append("Nome: " + f.itemName + "\n");
                if (f.gradeGiven)
                {
                    s.Append("Nota: " + f.graderaw + " de " + f.grademax + "= " + f.percentageFormatted + "%\n");
                }
                else
                {
                    s.Append("Falta de nota\n");
                }
            }
            
            return s.ToString();
        }

        // Da toda a info dos folios em string
        public String foliosToString()
        {
            StringBuilder response = new StringBuilder();
            foreach (Folio f in folios)
            {
                response.Append("NAME: " + f.itemName + "\n");
                response.Append("MINIMO: " + f.grademin + "\n");
                response.Append("Maximo: " + f.grademax + "\n");
                if (f.gradeGiven)
                {
                    response.Append("DATE: " + f.gradedategraded + " value " + f.gradeformatted + "\n");
                    response.Append(f.percentageFormatted + "\n");
                }
                else
                    response.Append("Grade wasn't given yet.\n");
                response.Append("----------------\n");
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

        public Folio getFolio(int fId)
        {
            
            foreach(Folio fo in folios)
            {
                if (fo.id == fId)
                    return fo;
            }

            return null;
        }


        // METODOS PARA FILTRAR WEBSERVICE RESPONSES JSON

        public void receiveCourseTopics(List<jsonValues.Topics> topics)
        {
            Topic placeHolder = null;
            modules moduleP = null;
            contents contentP = null;
            foreach(jsonValues.Topics t in topics)
            {
                placeHolder = new Topic();
                placeHolder.id = t.id;
                placeHolder.name = t.name;
                placeHolder.summary = HtmlDecode(t.summary);
                placeHolder.visible = t.visible;
                placeHolder.userVisible = (t.uservisible.Equals("true")) ? 1 : 0;
                foreach(jsonValues.Modules m in t.modules)
                {
                    moduleP = new modules();
                    moduleP.id = m.id;
                    moduleP.name = m.name;
                    if(m.description != null)
                    moduleP.description = HtmlDecode( m.description);
                    moduleP.visible = m.visible;
                    moduleP.url = m.url;
                    moduleP.uservisible = (m.uservisible.Equals("true")) ? 1 : 0;
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
                        if(c.summary != null)
                        contentP.summary = HtmlDecode(c.summary);
                        
                        //TODO Times to be ADDED
                        moduleP.contents.AddLast(contentP);
                    }
                    placeHolder.modules.AddLast(moduleP);
                }
                this.topics.AddLast(placeHolder);
            }

        }

        public void receiveGrades(jsonValues.usergrades grades)
        {
            int id; //X
            String itemName; //X
            DateTime gradedategraded;
            bool gradeGiven = false; //X
            float gradeformatted; // verificar se nao eh '-' X
            int graderaw; //X
            String percentageFormatted; //X
            int grademin; //X
            int grademax; //X
            Folio template;
            
            foreach(jsonValues.gradeitems f in grades.gradeitems)
            {
                template = new Folio();
                template.itemName = f.itemname;
                template.id = f.id;
                template.grademin = f.grademin;
                template.grademax = f.grademax;
                if(f.graderaw != null) // foi dada nota
                {
                    template.graderaw = f.graderaw;
                    template.gradeformatted = f.gradeformatted;
                    template.percentageFormatted = f.percentageformatted;
                    template.gradeGiven = true;

                    DateTime date = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                    date = date.AddSeconds(Convert.ToInt32(f.gradedategraded));
                    template.gradedategraded = date;
                }
                folios.AddLast(template);
            }
            
        }



        // METODOS PARA FILTRAR WEBSERVICE RESPONSES XML 

        public void filterCourse(String content)
        {
            
            String[] filter = content.Split('\n');




            for (this.position = 0; position < filter.Length; position++)
            {

                if (filter[position].Contains("<SINGLE>")) // Foi encontrada um topico
                {
                    
                    topics.AddLast(FilterTopics(filter));
                    
                }
            }

            
        }

        private Topic FilterTopics(String[] filter)
        {

            String[] variable;

            Topic placeHolder = null;
            modules temp = null;
            
            while (!filter[position].Contains("</SINGLE>") && position< filter.Length) // Limita o modulo MULTIPLE
            {
                if (filter[position].Contains("<KEY name=\"id\">")) {// ID
                    placeHolder = new Topic();

                    variable = filter[position].Split(new[] { "<KEY name=\"id\"><VALUE>" }, StringSplitOptions.None);
                    variable = variable[1].Split(new[] { "</VALUE>" }, StringSplitOptions.None);
                    //filtered.Append("O ID do utilizador é: " + variable[0] + "\n");
                    placeHolder.id = Int32.Parse(variable[0]);
                }

                if (filter[position].Contains("<KEY name=\"name\">")) // NOME 
                {
                    variable = filter[position].Split(new[] { "<KEY name=\"name\"><VALUE>" }, StringSplitOptions.None);
                    variable = variable[1].Split(new[] { "</VALUE>" }, StringSplitOptions.None);
                    //filtered.Append("O Nome do utilizador é: " + variable[0] + "\n");
                    placeHolder.name = variable[0];
                }

                if (filter[position].Contains("<KEY name=\"visible\">")) // SE ESTA VISIVEL
                {
                    
                    variable = filter[position].Split(new[] { "<KEY name=\"visible\"><VALUE>" }, StringSplitOptions.None);
                    variable = variable[1].Split(new[] { "</VALUE>" }, StringSplitOptions.None);
                    //filtered.Append("O visible do utilizador é: " + variable[0] + "\n");
                    placeHolder.visible = Int32.Parse(variable[0]);
                }

                if (filter[position].Contains("<KEY name=\"uservisible\">")) // SE ESTA VISIVEL AO UTILIZADOR
                {
                    
                    variable = filter[position].Split(new[] { "<KEY name=\"uservisible\"><VALUE>" }, StringSplitOptions.None);
                    variable = variable[1].Split(new[] { "</VALUE>" }, StringSplitOptions.None);
                    //filtered.Append("O uservisible do utilizador é: " + variable[0] + "\n");
                    placeHolder.userVisible = Int32.Parse(variable[0]);
                }

                if (filter[position].Contains("<KEY name=\"url\">")) // LINK
                {
                    variable = filter[position].Split(new[] { "<KEY name=\"url\"><VALUE>" }, StringSplitOptions.None);
                    variable = variable[1].Split(new[] { "</VALUE>" }, StringSplitOptions.None);
                    //filtered.Append("O Nome do utilizador é: " + variable[0] + "\n");
                    placeHolder.url = variable[0];
                }


                if (filter[position].Contains("<KEY name=\"summary\">")) // sumario
                {
                    variable = filter[position].Split(new[] { "<KEY name=\"summary\"><VALUE>" }, StringSplitOptions.None);
                    variable = variable[1].Split(new[] { "</VALUE>" }, StringSplitOptions.None);
                    //filtered.Append("O Nome do utilizador é: " + variable[0] + "\n");
                    placeHolder.summary =HtmlDecode(variable[0]);
                }

                if (filter[position].Contains("<KEY name=\"hiddenbynumsections\">"))
                {// hiddenbynumsections
                    variable = filter[position].Split(new[] { "<KEY name=\"hiddenbynumsections\"><VALUE>" }, StringSplitOptions.None);
                    variable = variable[1].Split(new[] { "</VALUE>" }, StringSplitOptions.None);
                    //filtered.Append("O hiddenbynumsections do utilizador é: " + variable[0] + "\n");
                    placeHolder.hiddenByNumSections = Int32.Parse(variable[0]);
                }

                if (filter[position].Contains("<KEY name=\"modules\"><MULTIPLE>"))
                {
                    temp = filterModule(filter);
                    if(temp != null)
                    placeHolder.modules.AddLast(temp);
                }
                position++;
            }
            
            return placeHolder;
        }

        private modules filterModule(String[] filter)
        {
            
            String[] variable;

            modules placeHolder = null;
            contents temp=null;

            while (!filter[position].Contains("</MULTIPLE>") && position < filter.Length) // Limita o modulo 
            {
                if (filter[position].Contains("<KEY name=\"id\">")) // ID DO MODULO
                {
                    placeHolder = new modules();
                    
                    variable = filter[position].Split(new[] { "<KEY name=\"id\"><VALUE>" }, StringSplitOptions.None);
                    variable = variable[1].Split(new[] { "</VALUE>" }, StringSplitOptions.None);
                    //filtered.Append("O ID do utilizador é: " + variable[0] + "\n");
                    placeHolder.id = Int32.Parse(variable[0]);
                }

                if (filter[position].Contains("<KEY name=\"name\">")) // NOME DO MODULO
                {
                    variable = filter[position].Split(new[] { "<KEY name=\"name\"><VALUE>" }, StringSplitOptions.None);
                    variable = variable[1].Split(new[] { "</VALUE>" }, StringSplitOptions.None);
                    //filtered.Append("O Nome do utilizador é: " + variable[0] + "\n");
                    placeHolder.name = variable[0];
                }

                if (filter[position].Contains("<KEY name=\"modname\">"))
                {
                    variable = filter[position].Split(new[] { "<KEY name=\"modname\"><VALUE>" }, StringSplitOptions.None);
                    variable = variable[1].Split(new[] { "</VALUE>" }, StringSplitOptions.None);
                    //filtered.Append("O Nome do utilizador é: " + variable[0] + "\n");
                    placeHolder.modname = variable[0];
                }

                if (filter[position].Contains("<KEY name=\"url\">")) // LINK DO MODULO
                {
                    variable = filter[position].Split(new[] { "<KEY name=\"url\"><VALUE>" }, StringSplitOptions.None);
                    if (variable.Length > 1)
                    {
                        variable = variable[1].Split(new[] { "</VALUE>" }, StringSplitOptions.None);
                        //filtered.Append("O Nome do utilizador é: " + variable[0] + "\n");
                        placeHolder.url = variable[0];
                    }
                }

                //if (filter[position].Contains("<KEY name=\"description\">")) // DESCRICAO
                //{
                //    variable = filter[position].Split(new[] { "<KEY name=\"description\"><VALUE>" }, StringSplitOptions.None);
                //    variable = variable[1].Split(new[] { "</VALUE>" }, StringSplitOptions.None);
                //    //filtered.Append("O Nome do utilizador é: " + variable[0] + "\n");
                //    placeHolder.description = variable[0];
                //}

                if (filter[position].Contains("<KEY name=\"visible\">")) // SE ESTA VISIVEL
                {
                    
                    variable = filter[position].Split(new[] { "<KEY name=\"visible\"><VALUE>" }, StringSplitOptions.None);
                    variable = variable[1].Split(new[] { "</VALUE>" }, StringSplitOptions.None);
                    //filtered.Append("O visible do utilizador é: " + variable[0] + "\n");
                    placeHolder.visible = Int32.Parse(variable[0]);
                }

                if (filter[position].Contains("<KEY name=\"uservisible\">")) // SE ESTA VISIVEL AO UTILIZADOR
                {
                    
                    variable = filter[position].Split(new[] { "<KEY name=\"uservisible\"><VALUE>" }, StringSplitOptions.None);
                    variable = variable[1].Split(new[] { "</VALUE>" }, StringSplitOptions.None);
                    //filtered.Append("O uservisible do utilizador é: " + variable[0] + "\n");
                    placeHolder.uservisible = Int32.Parse(variable[0]);
                }

                if (filter[position].Contains("<KEY name=\"visibleoncoursepage\">")) // SE ESTA VISIBLE NA PAGINA DA CADEIRA
                {
                    
                    variable = filter[position].Split(new[] { "<KEY name=\"visibleoncoursepage\"><VALUE>" }, StringSplitOptions.None);
                    variable = variable[1].Split(new[] { "</VALUE>" }, StringSplitOptions.None);
                    //filtered.Append("O visibleoncoursepage do utilizador é: " + variable[0] + "\n");
                    placeHolder.visibleOnCoursePage = Int32.Parse(variable[0]);
                }

                if(filter[position].Contains("<KEY name=\"contents\"><MULTIPLE>"))
                {
                    temp = filterContent(filter);
                    if(temp !=null)
                    placeHolder.contents.AddLast(temp);
                }

                position++;
            }


            return placeHolder;
        }

        private contents filterContent(String[] filter)
        {
            String[] variable;
           
            contents placeHolder = null;
            DateTime datePlace = new DateTime();

            while (!filter[position].Contains("</MULTIPLE>") && position < filter.Length) // Limita o topico
            {

                if (filter[position].Contains("<KEY name=\"type\">")) 
                {
                    if(placeHolder == null)
                        placeHolder = new contents();
                    variable = filter[position].Split(new[] { "<KEY name=\"type\"><VALUE>" }, StringSplitOptions.None);
                    if (variable.Length > 1)
                    {
                        variable = variable[1].Split(new[] { "</VALUE>" }, StringSplitOptions.None);
                        //filtered.Append("O Nome do utilizador é: " + variable[0] + "\n");
                        placeHolder.type = variable[0];
                    }
                }

                if (filter[position].Contains("<KEY name=\"author\">"))
                {
                    if (placeHolder == null)
                        placeHolder = new contents();
                    variable = filter[position].Split(new[] { "<KEY name=\"author\"><VALUE>" }, StringSplitOptions.None);
                    if (variable.Length > 1)
                    {
                        variable = variable[1].Split(new[] { "</VALUE>" }, StringSplitOptions.None);
                        //filtered.Append("O Nome do utilizador é: " + variable[0] + "\n");
                        placeHolder.author = variable[0];
                    }
                }

                if (filter[position].Contains("<KEY name=\"filename\">")) 
                {
                    variable = filter[position].Split(new[] { "<KEY name=\"filename\"><VALUE>" }, StringSplitOptions.None);
                    if (variable.Length > 1)
                    {
                        variable = variable[1].Split(new[] { "</VALUE>" }, StringSplitOptions.None);
                        //filtered.Append("O Nome do utilizador é: " + variable[0] + "\n");
                        placeHolder.fileName = variable[0];
                    }
                }

                if (filter[position].Contains("<KEY name=\"filepath\">"))
                {
                    variable = filter[position].Split(new[] { "<KEY name=\"filepath\"><VALUE>" }, StringSplitOptions.None);
                    if (variable.Length > 1)
                    {
                        variable = variable[1].Split(new[] { "</VALUE>" }, StringSplitOptions.None);
                        //filtered.Append("O Nome do utilizador é: " + variable[0] + "\n");
                        placeHolder.filePath = variable[0];
                    }
                }

                if (filter[position].Contains("<KEY name=\"fileurl\">"))
                {
                    variable = filter[position].Split(new[] { "<KEY name=\"fileurl\"><VALUE>" }, StringSplitOptions.None);
                    if (variable.Length > 1)
                    {
                        variable = variable[1].Split(new[] { "</VALUE>" }, StringSplitOptions.None);
                        //filtered.Append("O Nome do utilizador é: " + variable[0] + "\n");
                        placeHolder.fileUrl = variable[0];
                    }
                }


                //filtrar o timecreated
                if (filter[position].Contains("<KEY name=\"timecreated\">"))
                {
                    variable = filter[position].Split(new[] { "<KEY name=\"timecreated\"><VALUE>" }, StringSplitOptions.None);
                    if (variable.Length > 1)
                    {
                        variable = variable[1].Split(new[] { "</VALUE>" }, StringSplitOptions.None);
                        datePlace = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                        datePlace = datePlace.AddSeconds(Convert.ToInt32(variable[0]));
                        //filtered.Append("O primeiro acesso do utilizador foi: " + datePlace.ToLocalTime() + "\n");
                        placeHolder.timeCreated = datePlace;
                    }
                }


                //filtrar o timemodified
                if (filter[position].Contains("<KEY name=\"timemodified\">"))
                {
                    variable = filter[position].Split(new[] { "<KEY name=\"timemodified\"><VALUE>" }, StringSplitOptions.None);
                    if (variable.Length > 1)
                    {
                        variable = variable[1].Split(new[] { "</VALUE>" }, StringSplitOptions.None);
                        datePlace = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                        datePlace = datePlace.AddSeconds(Convert.ToInt32(variable[0]));
                        //filtered.Append("O primeiro acesso do utilizador foi: " + datePlace.ToLocalTime() + "\n");
                        placeHolder.timeModified = datePlace;
                    }
                }

                if (filter[position].Contains("<KEY name=\"filesize\">")) 
                {
                    variable = filter[position].Split(new[] { "<KEY name=\"filesize\"><VALUE>" }, StringSplitOptions.None);
                    if (variable.Length > 1)
                    {
                        variable = variable[1].Split(new[] { "</VALUE>" }, StringSplitOptions.None);
                        //filtered.Append("O filesize do utilizador é: " + variable[0] + "\n");
                        placeHolder.fileSize = Int32.Parse(variable[0]);
                    }
                }

                if (filter[position].Contains("<KEY name=\"isexternalfile\">"))
                {
                    variable = filter[position].Split(new[] { "<KEY name=\"isexternalfile\"><VALUE>" }, StringSplitOptions.None);
                    if (variable.Length > 1)
                    {
                        variable = variable[1].Split(new[] { "</VALUE>" }, StringSplitOptions.None);
                        //filtered.Append("O isexternalfile do utilizador é: " + variable[0] + "\n");
                        placeHolder.isExternalFile = Int32.Parse(variable[0]);
                    }
                }

                if (filter[position].Contains("<KEY name=\"userid\">"))
                {
                    variable = filter[position].Split(new[] { "<KEY name=\"userid\"><VALUE>" }, StringSplitOptions.None);
                    if (variable.Length > 1)
                    {
                        variable = variable[1].Split(new[] { "</VALUE>" }, StringSplitOptions.None);
                        //filtered.Append("O userid do utilizador é: " + variable[0] + "\n");
                        placeHolder.userId = Int32.Parse(variable[0]);
                    }
                }


                position++;
            }



            return placeHolder;
        }

        // Metodo 
        public void filterfolios(String[] content)
        {
            String[] variable;
            int position = 0;
            Folio placeHolder = null;
            DateTime datefirst = new DateTime();

            while (!content[position].Contains("<KEY name=\"gradeitems\"><MULTIPLE>") && position<content.Length)
                position++;

            while (!content[position].Contains("</MULTIPLE>") && position < content.Length)
            {
                if (content[position].Contains("<SINGLE>")) // comeco de um folio
                {
                    placeHolder = new Folio();
                }

                if (content[position].Contains("<KEY name=\"id\">"))
                {// ID
                    variable = content[position].Split(new[] { "<KEY name=\"id\"><VALUE>" }, StringSplitOptions.None);
                    variable = variable[1].Split(new[] { "</VALUE>" }, StringSplitOptions.None);
                    //filtered.Append("O ID do utilizador é: " + variable[0] + "\n");
                    placeHolder.id = Int32.Parse(variable[0]);
                }


                if (content[position].Contains("<KEY name=\"itemname\">")) // NOME DO folio
                {
                    variable = content[position].Split(new[] { "<KEY name=\"itemname\"><VALUE>" }, StringSplitOptions.None);
                    variable = variable[1].Split(new[] { "</VALUE>" }, StringSplitOptions.None);
                    //filtered.Append("O Nome do utilizador é: " + variable[0] + "\n");
                    placeHolder.itemName = variable[0];
                }

                if (content[position].Contains("<KEY name=\"gradedategraded\">"))
                {
                    variable = content[position].Split(new[] { "<KEY name=\"gradedategraded\"><VALUE>" }, StringSplitOptions.None);
                    if (variable.Length > 1)
                    {
                        variable = variable[1].Split(new[] { "</VALUE>" }, StringSplitOptions.None);
                        datefirst = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                        datefirst = datefirst.AddSeconds(Convert.ToInt32(variable[0]));
                        //filtered.Append("O primeiro acesso do utilizador foi: " + datefirst.ToLocalTime() + "\n");
                        placeHolder.gradedategraded = datefirst;
                        placeHolder.gradeGiven = true;
                    }

                }
                // float gradeformatted
                if (content[position].Contains("<KEY name=\"gradeformatted\">"))
                {
                    variable = content[position].Split(new[] { "<KEY name=\"gradeformatted\"><VALUE>" }, StringSplitOptions.None);
                    variable = variable[1].Split(new[] { "</VALUE>" }, StringSplitOptions.None);
                    //filtered.Append("O ID do utilizador é: " + variable[0] + "\n");
                    float n;
                    bool isNumeric = float.TryParse(variable[0], out n);

                    if (placeHolder != null && isNumeric)
                    {
                        placeHolder.gradeformatted = variable[0];
                        placeHolder.gradeGiven = true;
                    }
                    
                }

                if (content[position].Contains("<KEY name=\"graderaw\">")) // grade raw
                {
                    variable = content[position].Split(new[] { "<KEY name=\"graderaw\"><VALUE>" }, StringSplitOptions.None);
                    if (variable.Length > 1)
                    {
                        variable = variable[1].Split(new[] { "</VALUE>" }, StringSplitOptions.None);
                        //filtered.Append("O Nome do utilizador é: " + variable[0] + "\n");
                        placeHolder.graderaw = Int32.Parse(variable[0]);
                    }
                }

                if (content[position].Contains("<KEY name=\"grademin\">")) // grade min
                {
                    variable = content[position].Split(new[] { "<KEY name=\"grademin\"><VALUE>" }, StringSplitOptions.None);
                    variable = variable[1].Split(new[] { "</VALUE>" }, StringSplitOptions.None);
                    //filtered.Append("O Nome do utilizador é: " + variable[0] + "\n");
                    placeHolder.grademin = Int32.Parse(variable[0]);
                }

                if (content[position].Contains("<KEY name=\"grademax\">")) // grade max
                {
                    variable = content[position].Split(new[] { "<KEY name=\"grademax\"><VALUE>" }, StringSplitOptions.None);
                    variable = variable[1].Split(new[] { "</VALUE>" }, StringSplitOptions.None);
                    //filtered.Append("O Nome do utilizador é: " + variable[0] + "\n");
                    placeHolder.grademax = Int32.Parse(variable[0]);
                }


                // float percentage formated
                if (content[position].Contains("<KEY name=\"percentageformatted\">"))
                {
                    variable = content[position].Split(new[] { "<KEY name=\"percentageformatted\"><VALUE>" }, StringSplitOptions.None);
                    variable = variable[1].Split(new[] { "</VALUE>" }, StringSplitOptions.None);
                    variable = variable[0].Split(new[] { " " }, StringSplitOptions.None);
                    //filtered.Append("O ID do utilizador é: " + variable[0] + "\n");
                    float n;
                    bool isNumeric = float.TryParse(variable[0], out n);

                    if (placeHolder != null && isNumeric)
                    {
                        placeHolder.percentageFormatted = variable[0];
                        
                    }

                }


                if (content[position].Contains("</SINGLE>")) // fim de um folio
                {
                    folios.AddLast(placeHolder);
                }

                position++;
            }

        }

      
        public static string HtmlDecode(string value)
        {
            String v;
            String[] s = value.Split('\n');
            StringBuilder sb = new StringBuilder();
            foreach (String st in s)
            {
                v = st.Replace("&amp;", "&").Replace("&lt;", "<").Replace("&gt;", ">").Replace("&quot;", "\"").Replace("&apos;", "'").Replace("<br />", "").Replace("<hr />", "").Replace("<h3>", "").Replace("</h3>", "").Replace("<li>", "").Replace("</li>", "")
                    .Replace("<ol>", "").Replace("</ol>", "").Replace("<span>", "").Replace("<b>", "").Replace("</b>", "").Replace("</p>", "").Replace("</span>", "").Replace("<u>", "").Replace("</u>", "").Replace("<ul>", "").Replace("</ul>", "").Replace("</div>", "").Replace("</a>", "").Replace("<strong>","").Replace("</strong>", "").Replace("<hr>", "").Replace("<br>", "").Replace("</font>", "");

                v = Regex.Replace(v, @"\<span(.*?)\>", "");
                v = Regex.Replace(v, @"\<p(.*?)\>", "");
                v = Regex.Replace(v, @"\<a(.*?)\>", "");
                v = Regex.Replace(v, @"\<div(.*?)\>", "");
                v = Regex.Replace(v, @"\<img(.*?)\>", "");
                v = Regex.Replace(v, @"\<font(.*?)\>", "");

                sb.Append(v + "\n");
            }
            

            return sb.ToString();

        }
    }
}
