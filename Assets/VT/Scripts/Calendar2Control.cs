using HookControl;
using System;
using UnityEngine;
namespace VT {
    [Serializable]
	public class Calendar2Control : IControl {


		private Control control;
		private VoidFunc click2;

		public Calendar2Control(GameObject prefab){
			control = new Control ();
			control.prefab = prefab;
		}

		public void Set (VoidFunc click2)
		{
			this.click2 = click2;
		}
		public ShowResult Show ()
		{
			var ret = control.Show ();
			if (ret == ShowResult.FIRST || ret == ShowResult.OK) {
				var hooks = control.instance.GetComponent<Calendar2Hooks> ();
				if (hooks != null) {
					hooks.click2 = this.click2;
				}
			}
			return ret;
		}

		public ShowResult SetAndShow (VoidFunc click2)
		{
			this.Set (click2);
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

