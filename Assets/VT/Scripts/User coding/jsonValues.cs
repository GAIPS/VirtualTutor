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
        }
        // Use this for initialization
        void Start()
        {

        }
    }
}