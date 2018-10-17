using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public UserInfo.UserData user;
    public List<UserInfo.Course> courses = new List<UserInfo.Course>();


    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public Boolean getURunning = false;

    public UserInfo.UserData getUser()
    {
        while (getURunning) {}

        getURunning = true;
        if (user == null)
        {
            user = new UserInfo.UserData();
            Debug.Log("NEW USER");
        }

        getURunning = false;

        return user;
    }

    public List<UserInfo.Course> getCourses()
    {
        if (courses == null)
        {
            courses = new List<UserInfo.Course>();
            Debug.Log("NEW Course");
        }

        return courses;
    }

    public UserInfo.Course getCourseById(int id)
    {
        foreach (UserInfo.Course co in courses)
        {
            if (co.id == id)
                return co;
        }

        return null;
    }

    // dados do User
    /*
      * Recebe informacao basica do utilizador EM JSON
      */
    public void receiveUser(jsonValues.users u)
    {
        user.receiveUsers(u);
    }

    // DADOS DO COURSE
    /*
     * Recebe informacao basica das cadeiras que o utilizador esta inscrito EM JSON
     */
    public void receiveCourses(List<jsonValues.Courses> c, Boolean multiCourses, int courseId)
    {
        UserInfo.Course template;


        foreach (jsonValues.Courses co in c)
        {
            if (!multiCourses) // Caso seja uma cadeira
            {
                if (co.id == courseId)
                {
                    template = new UserInfo.Course();
                    template.id = co.id;
                    template.idNumber = co.idnumber;
                    template.shortName = co.shortname;
                    template.fullName = co.fullname;
                    template.summary = UserInfo.Course.HtmlDecode(co.summary);

                    template.visible = co.visible;
                    courses.Add(template);
                }
            }

            else
            {
                template = new UserInfo.Course();
                template.id = co.id;
                template.idNumber = co.idnumber;
                template.shortName = co.shortname;
                template.fullName = co.fullname;
                template.summary = UserInfo.Course.HtmlDecode(co.summary);

                template.visible = co.visible;
                courses.Add(template);
            }
        }
    }

    /*
        * 
        */
    public void receiveGrades(List<jsonValues.Grades> g)
    {
        UserInfo.Course template;
        double n;
        bool isNumeric;
        foreach (jsonValues.Grades gr in g)
        {
            template = getCourseById(gr.courseid);
            if (template != null)
            {
                isNumeric = double.TryParse(gr.grade, out n);
                if (gr.grade != null && isNumeric)
                {
                    template.grade = double.Parse(gr.grade);
                }
            }
        }
    }
}