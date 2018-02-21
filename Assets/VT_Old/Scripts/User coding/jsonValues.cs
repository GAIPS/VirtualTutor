using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System;

namespace UserInfo
{
    public class jsonValues : MonoBehaviour
    {
        [Serializable]
        public class users
        {
            public int id;
            public string username;
            public string fullname;
            public string email;
            public string lang;
            public string auth;
            public int firstaccess;
            public int lastaccess;
            //public List<preferences> pref ;
        }

        [Serializable]
        public class Courses
        {
            public int id;
            public string shortname;
            public string fullname;
            public int idnumber;
            public int visible;
            public String summary;

            public List<assignments> assignments;
            
            //public int startdate;
            //public int enddata;

        }

        [Serializable]
        public class Grades
        {
            public int courseid;
            public string grade;
            public string graderaw;
        }
        [Serializable]
        public class Topics
        {
            public int id;
            public string name;
            public int visible;
            public String summary;
            public String uservisible;
            public List<Modules> modules;
            
            //public int startdate;
            //public int enddata;

        }

        [Serializable]
        public class Modules
        {
            public int id;
            public string url;
            public string name;
            public int visible;
            public String description;
            public String uservisible;
            public List<Contents> contents;
            
            //public int startdate;
            //public int enddata;

        }
        [Serializable]
        public class Contents
        {
            public int filesize;
            public int timecreated;
            public int timemodified;
            public string type;
            public string filename;
            public string filepath;
            public string fileurl;
            public string author;
            public int userid;
            public int isexternalfile;
            public String summary;
            public String uservisible;
            
            //public int startdate;
            //public int enddata;

        }

        [Serializable]
        public class usergrades
        {
            public int courseid;
            public int userid;
            public String userfullname;
            public List<gradeitems> gradeitems;
        }

        [Serializable]
        public class gradeitems
        {
            public int id;
            public String itemname;
            public int graderaw;
            // in case the value is null, this happens when the grade wasn't given
            public int gradedatesubmitted;
            public int gradedategraded;
            public string gradeformatted;
            public int grademin;
            public int grademax;
            public String percentageformatted;
            public String feedback;
            public String status; // optional
        }

        [Serializable]
        public class assignments
        {
            // UTILIZADO NO mod_assign_get_assignments
            public int id;
            public int cmid;
            public int course;
            public String name;
            public int nosubmissions; // se houve submissoes
            public int duedate;
            public int allowsubmissionsfromdate; // data que o aluno pode fazer submissoes
            public int grade; // grademax
            public int cutoffdate; // prazo limite

            // Utilizado no mod_assign_get_grades
            public int assignmentid;
            public List<grades> grades;
        }

        [Serializable]
        public class grades
        {
            public int id;
            public int userid; // verificar se eh o do aluno
            public int attemptnumber; // a tentativa nº
            public String grade; // if -1 entao nao foi dada
            public int timecreated; // submissao do aluno?
            public int timemodified; // avaliacao do professor?
        }


        // usado no mod_quiz_get_quizzes_by_courses
        [Serializable]
        public class quizzes
        {
            public int id;
            public int course;
            public int coursemodule;
            public String name;
            public String intro;
            public List<introfiles> introfiles;

            public int timeopen; // Optional The time when this quiz opens. (0 = no restriction.)
            public int timeclose; //Optional The time when this quiz closes. (0 = no restriction.)
            public int timelimit; // Optional The time limit for quiz attempts, in seconds.
            public String overduehandling; //Optional //The method used to handle overdue attempts.'autosubmit', 'graceperiod' or 'autoabandon'.
            public int attempts;
            public double grade;
        }

        [Serializable]
        public class introfiles
        {
            public String filename;
            public String filepath;
            public int filesize;
            public String fileurl;
            public int timemodified;
            public int isexternalfile;
        }

        // usado no core_course_get_updates_since 
        [Serializable]
        public class instances
        {
            public String contextlevel; //the context level
            public int id;  //Instance id
            public List<updates> updates;
        }

        [Serializable]
        public class updates
        {
            public String name; //Name of the area updated.
            public int timeupdated; //Optional Last time was updated
            public List<int> itemids; // Optional The ids of the items updated
        }

        [Serializable]
        public class forums
        {
            public int id; //Forum id
            public String type; //The forum type
            public String name; //Forum name
            public String intro;  //The forum intro
            public List<introfiles> introfiles;

        }

        [Serializable]
        public class discussions
        {
            public int id; //Post id -> temporariamente não tem utilidade
            public string name; //Discussion name
            public int timemodified;  //Time modified
            public int discussion; // discussion id
            public String subject; //subject of message
            public String message; //message posted
            public String userfullname; //Post author full name
            public String usermodifiedfullname; //Post modifier full name
            public int created;//Creation time
            public int modified;//Time modified
            public int userid; // id of user who created the discussion

        }

        [Serializable]
        public class posts
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


        [Serializable]
        public class warnings
        {
            public int itemid;
            public String warningcode;
            public String message;
        }

        // Use this for initialization
        void Start()
        {

        }
    }
}