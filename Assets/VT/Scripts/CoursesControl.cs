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

		public CoursesControl (GameObject prefab)
		{
			control = new Control ();
			control.prefab = prefab;
		}

		public void Set (VoidFunc click)
		{
			this.click = click;
		}

		public ShowResult Show ()
		{
			var ret = control.Show ();
			if (ret == ShowResult.FIRST || ret == ShowResult.OK) {
                hook = control.instance.GetComponent<CoursesHooks> ();
				if (hook != null) {
					hook.clickCourse = this.click;

                    hook.Show();
				}
			}
			return ret;
		}

		public ShowResult SetAndShow (VoidFunc click)
		{
			this.Set (click);
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
