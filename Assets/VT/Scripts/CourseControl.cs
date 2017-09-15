using HookControl;
using System;
using UnityEngine;

namespace VT {
    [Serializable]
    public class CourseControl : IControl {
        private CourseHooks hook;

        private Control control;

        private Course course;

        private VoidFunc onConfirm;



        public CourseControl(GameObject prefab) {
            control = new Control();
            control.prefab = prefab;
        }

        public void Set(VoidFunc onConfirm, Course course) {
            this.onConfirm = onConfirm;
            this.course = course;
        }

        public void Set(Course course) {
            // TODO Implement event system (whatever that means)
            this.Set(null, course);
        }

        public ShowResult Show() {
            var ret = control.Show();
            if (ret == ShowResult.FIRST || ret == ShowResult.OK) {
                hook = control.instance.GetComponent<CourseHooks>();
                if (hook != null) {
                    hook.onConfirm = this.onConfirm;
                    if (course != null) {
                        hook.CourseName = course.Name;
                        hook.onEaseSlider = (float value) => { SliderUpdate(value, out course.importance); };
                        hook.onLikeSlider = (float value) => { SliderUpdate(value, out course.importance); };
                        hook.onImportanceSlider = (float value) => { SliderUpdate(value, out course.importance); };

                        // HACK take into account that the list needs to be ordered or to be sorted when on screen.
                        for (int i = course.Checkpoints.Count - 1; i >= 0; i--) {
                            hook.AddCheckpoint(course.Checkpoints[i]);
                        }
                    }
                    hook.Show();
                }
            }
            return ret;
        }

        public ShowResult SetAndShow(VoidFunc onConfirm, Course course) {
            this.Set(onConfirm, course);
            return Show();
        }

        public ShowResult SetAndShow(Course course) {
            this.Set(course);
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

        public void SliderUpdate(float value, out float savedValue) {
            savedValue = value;
        }
    }
}
