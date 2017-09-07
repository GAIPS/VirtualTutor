using HookControl;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace VT {
    [Serializable]
    public class CoursesControl : IControl {
        private CoursesHooks hook;
        private Control control;

        private List<Course> courses;

        private CourseControl courseControl;

        public CoursesControl(GameObject prefab, CourseControl courseControl) {
            control = new Control();
            control.prefab = prefab;

            courses = new List<Course>();

            this.courseControl = courseControl;
        }

        public void Set(List<Course> courses) {
            this.courses = courses;
        }

        public ShowResult Show() {
            var ret = control.Show();
            if (ret == ShowResult.FIRST || ret == ShowResult.OK) {
                hook = control.instance.GetComponent<CoursesHooks>();
                if (hook != null) {
                    foreach (Course course in courses) {
                        hook.AddButton(course.Name, () => { OpenDetails(course); });
                    }
                    hook.Show();
                }
            }
            return ret;
        }

        public ShowResult SetAndShow(List<Course> courses) {
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
        public void Enable() {
            control.Enable();
        }

        public void OpenDetails(Course course) {
            if (courseControl != null) {
                // TODO Start here and break everything to make course details control great again.
                courseControl.SetAndShow(null, course);
            } else {
                Debug.LogWarning("No Course Control found.");
            }
        }
    }
}
