using HookControl;
using System.Collections.Generic;
using UnityEngine;
using UserInfo;

namespace VT {
    public class CoursesControl : IControl {
        private CoursesHooks hook;
        private Control control;

        //private List<UserInfo.Course> courses;

        private UserData user;

        public event CourseFunc CourseSelectionEvent;
        private CourseControl courseControl;

        public GameObject Instance
        {
            get { return control.Instance; }
            set { control.Instance = value; }
        }

        public CoursesControl(GameObject prefab, CourseControl courseControl) {
            control = new Control();
            control.prefab = prefab;
            GameObject login = GameObject.Find("MoodleLogin");
            user = login.GetComponent(typeof(UserData)) as UserData;
            
            //courses = user.courses; //new List<Course>();

            this.courseControl = courseControl;
        }

        public string GetName()
        {
            return "CoursesControl";
        }

        public void OpenDetails(UserInfo.Course course) {
            if (CourseSelectionEvent != null) {
                CourseSelectionEvent(course); 
            }
            if (courseControl != null) {
                courseControl.SetAndShow(course);
            } else {
                Debug.LogWarning("No Course Control found.");
            }
        }

        public void Set(List<UserInfo.Course> courses) {
            //this.courses = courses;
        }

        public ShowResult Show() {
            var ret = control.Show();
            if (ret == ShowResult.FIRST || ret == ShowResult.OK) {
                hook = control.Instance.GetComponent<CoursesHooks>();
                if (hook != null) {
                    foreach (UserInfo.Course course in user.courses) {
                        
                        hook.AddButton(course.fullName, () => { OpenDetails(course); });
                    }
                    hook.Show();
                }
            }
            return ret;
        }

        public ShowResult SetAndShow(List<UserInfo.Course> courses) {
            this.Set(courses);
            return Show();
        }

        public void Destroy() {
            if (hook) {
                hook.onHideEnded = () => {
                    control.Destroy();
                };
                hook.Hide();
            } else {
                control.Destroy();
            }
        }

        public void Disable() {
            if (hook) {
                hook.onHideEnded = () => {
                    control.Disable();
                };
                hook.Hide();
            } else {
                control.Disable();
            }
        }

        public bool IsVisible() {
            return control.IsVisible();
        }
    }
}
