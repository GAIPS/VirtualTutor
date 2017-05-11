using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


namespace VT
{
	[Serializable]
	public class CoursesControl :IControl
	{
        private CoursesHooks hook;
		private Control control;
		private VoidFunc click;
		private VoidFunc click2;
		private Course course1;
		private Course course2;
		public CoursesControl (GameObject prefab)
		{
			control = new Control ();
			control.prefab = prefab;
		}

		public void Set (VoidFunc click,VoidFunc click2,Course course1, Course course2)
		{
			this.click = click;
			this.click2 = click2;
			this.course1 = course1;
			this.course2 = course2;
		}

		public ShowResult Show ()
		{
			var ret = control.Show ();
			if (ret == ShowResult.FIRST || ret == ShowResult.OK) {
                hook = control.instance.GetComponent<CoursesHooks> ();
				if (hook != null) {
					hook.clickCourse = this.click;
					hook.clickCourse2 = this.click2;
					hook.Course1Name = course1.Name;
					hook.Course2Name = course2.Name;
                    hook.Show();
				}
			}
			return ret;
		}

		public ShowResult SetAndShow (VoidFunc click,VoidFunc click2, Course course1, Course course2)
		{
			this.Set (click,click2,course1,course2);
			return Show ();
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

		public bool IsVisible ()
		{
			return control.IsVisible ();
		}
		public void Enable(){
			control.Enable ();
		}
	}
}
