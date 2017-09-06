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

        private VoidFunc click;
        private VoidFunc click2;
        private Course course1;
        private Course course2;

        public CoursesControl(GameObject prefab) {
            control = new Control();
            control.prefab = prefab;

            courses = new List<Course>();
        }

        public void Set(List<Course> courses) {
            this.courses = courses;
        }

        public void Set(VoidFunc click, VoidFunc click2, Course course1, Course course2) {
            this.click = click;
            this.click2 = click2;
            this.course1 = course1;
            this.course2 = course2;
        }

        public ShowResult Show() {
            var ret = control.Show();
            if (ret == ShowResult.FIRST || ret == ShowResult.OK) {
                hook = control.instance.GetComponent<CoursesHooks>();
                if (hook != null) {
                    hook.AddButton(course1.Name, click);
                    hook.AddButton(course2.Name, click2);
                    hook.Show();
                }
            }
            return ret;
        }

        public ShowResult SetAndShow(VoidFunc click, VoidFunc click2, Course course1, Course course2) {
            this.Set(click, click2, course1, course2);
            return Show();
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
    }
}
