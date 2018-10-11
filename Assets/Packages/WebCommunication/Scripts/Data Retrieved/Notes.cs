using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UserInfo {
    // classe utilizada para guardar as notas do tutor virtual
    public class Notes {


        [Serializable]
        public class Values
        {
            // notes related
            public List<jsonValues.notes> notes;
        }

        public List<Note> notasGerais = new List<Note>();
        public List<Note> notasCurso = new List<Note>();
        public List<Note> notasPessoais = new List<Note>();

        public studentParameters sParam;

        public class studentParameters{
            public int studentId;
            public double aproveitamento;
            public double assiduidade;
            public String tutorName;

            public studentParameters(int studentId,double aproveitamento,double assiduidade,String tutorName)
            {
                this.studentId = studentId;
                this.aproveitamento = aproveitamento;
                this.assiduidade = assiduidade;
                this.tutorName = tutorName;
            }
        }

        public enum publishstate
        {
            COURSE, PERSONAL
        }

        public class Note
        {
            public publishstate publishstate;
            public string content;
            public int created;
            public int lastmodified;
            public int id;
        }
        // Use this for initialization
        void Start() {

        }

        // Update is called once per frame
        void Update() {

        }

        

        public Boolean existNote(int studentid)
        { 
            foreach (Note n in notasPessoais)
            {
                if (n.content.Contains(studentid + " "))
                {
                    return true;
                }
            }

            return false;
        }

        private void clearNotes()
        {
            notasGerais = new List<Note>();
            notasCurso = new List<Note>();
            notasPessoais = new List<Note>();
        }

        public void receiveNotes(List<jsonValues.notes> notes, int userid)
        {
            clearNotes();
            Note template;
            foreach(jsonValues.LNotes n in notes[0].sitenotes)
            {
                template = new Note();
                template.id = n.id;
                template.content = n.content;
                template.created = n.created;
                template.lastmodified = n.lastmodified;
                
                if (n.publishstate.Equals("draft"))
                    template.publishstate = publishstate.PERSONAL;
                else
                    template.publishstate = publishstate.COURSE;
                notasGerais.Add(template);
            }

            foreach (jsonValues.LNotes n in notes[0].personalnotes)
            {
                template = new Note();
                template.id = n.id;
                template.content = n.content;
                template.created = n.created;
                template.lastmodified = n.lastmodified;

                if (n.publishstate.Equals("draft"))
                {
                    template.publishstate = publishstate.PERSONAL;
                    //Debug.Log(Course.HtmlDecode(template.content));
                    Values v = UnityEngine.JsonUtility.FromJson<Values>(Course.HtmlDecode(template.content));
                    foreach(jsonValues.notes not in v.notes)
                    {
                        if(not.id == userid)
                        {
                            sParam = new studentParameters(not.id, not.aproveitamento, not.assiduidade, not.tutorname);
                        }
                    }
                }
                else
                    template.publishstate = publishstate.COURSE;
                notasPessoais.Add(template);
            }

            foreach (jsonValues.LNotes n in notes[0].coursenotes)
            {
                template = new Note();
                template.id = n.id;
                template.content = n.content;
                template.created = n.created;
                template.lastmodified = n.lastmodified;

                if (n.publishstate.Equals("draft"))
                    template.publishstate = publishstate.PERSONAL;
                else
                    template.publishstate = publishstate.COURSE;
                notasCurso.Add(template);
            }
        }

        internal Note getNote(int id)
        {
            foreach (Note n in notasPessoais)
            {
                if (n.content.Contains(id + " "))
                {
                    return n;
                }
            }

            return null;
        }
    }
}
