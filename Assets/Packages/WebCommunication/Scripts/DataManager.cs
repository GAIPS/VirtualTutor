using System;
using System.Collections.Generic;
using UnityEngine;
using UserInfo;

public class DataManager : MonoBehaviour
{
    [SerializeField]
    private UserData _user = new UserData();
    private List<Course> _courses = new List<Course>();

    public UserData GetUser()
    {
        if (_user == null)
        {
            _user = new UserData();
            Debug.Log("NEW USER");
        }

        return _user;
    }

    public List<Course> GetCourses()
    {
        if (_courses == null)
        {
            _courses = new List<Course>();
            Debug.Log("NEW Course");
        }

        return _courses;
    }

    public Course GetCourseById(int id)
    {
        foreach (Course co in _courses)
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
    public void ReceiveUser(jsonValues.users u)
    {
        _user.receiveUsers(u);
    }

    // DADOS DO COURSE
    /*
     * Recebe informacao basica das cadeiras que o utilizador esta inscrito EM JSON
     */
    public void ReceiveCourses(List<jsonValues.Courses> c, Boolean multiCourses, int courseId)
    {
        Course template;


        foreach (jsonValues.Courses co in c)
        {
            if (!multiCourses) // Caso seja uma cadeira
            {
                if (co.id == courseId)
                {
                    template = new Course();
                    template.id = co.id;
                    template.idNumber = co.idnumber;
                    template.shortName = co.shortname;
                    template.fullName = co.fullname;
                    template.summary = Course.HtmlDecode(co.summary);

                    template.visible = co.visible;
                    _courses.Add(template);
                }
            }

            else
            {
                template = new Course();
                template.id = co.id;
                template.idNumber = co.idnumber;
                template.shortName = co.shortname;
                template.fullName = co.fullname;
                template.summary = Course.HtmlDecode(co.summary);

                template.visible = co.visible;
                _courses.Add(template);
            }
        }
    }

    /*
        * 
        */
    public void ReceiveGrades(List<jsonValues.Grades> g)
    {
        Course template;
        double n;
        bool isNumeric;
        foreach (jsonValues.Grades gr in g)
        {
            template = GetCourseById(gr.courseid);
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