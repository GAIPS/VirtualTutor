using HookControl;
using UnityEngine;

namespace VT {
    public class CourseControl : IControl {
        private CourseHooks hook;

        private Control control;

        private Course course;

        public event CourseFunc CourseSelectionEvent;



        public CourseControl(GameObject prefab) {
            control = new Control();
            control.prefab = prefab;
        }

        public void Set(Course course) {
            this.course = course;
        }

        private void sendCourseEvent() {
            if (CourseSelectionEvent != null) {
                CourseSelectionEvent(course);
            }
        }

        public ShowResult Show() {
            var ret = control.Show();
            if (ret == ShowResult.FIRST || ret == ShowResult.OK) {
                hook = control.instance.GetComponent<CourseHooks>();
                if (hook != null) {
                    hook.onConfirm = sendCourseEvent;
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
