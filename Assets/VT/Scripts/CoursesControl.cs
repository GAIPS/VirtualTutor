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
				var hooks = control.instance.GetComponent<CoursesHooks> ();
				if (hooks != null) {
					hooks.clickCourse = this.click;
				}
			}
			return ret;
		}

		public ShowResult SetAndShow (VoidFunc click)
		{
			this.Set (click);
			return Show ();
		}

		public void Disable ()
		{
			control.Disable ();
		}

		public void Destroy ()
		{
			control.Destroy ();
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
