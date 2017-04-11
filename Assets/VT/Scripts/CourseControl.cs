using System;
using UnityEngine;

namespace VT
{
	[Serializable]
	public class CourseControl : IControl
	{
		private Control control;
		private VoidFunc onConfirm;
		private VoidFunc onOldRubber;
		private VoidFunc onNewRubber;
		private VoidFunc onOldPlus;
		private VoidFunc onNewPlus;

		public CourseControl (GameObject prefab)
		{
			control = new Control ();
			control.prefab = prefab;
		}

		public void Set (VoidFunc onConfirm, VoidFunc onOldRubber, VoidFunc onNewRubber, VoidFunc onOldPlus, VoidFunc onNewPlus)
		{
			this.onConfirm = onConfirm;
			this.onOldRubber = onOldRubber;
			this.onNewRubber = onNewRubber;
			this.onOldPlus = onOldPlus;
			this.onNewPlus = onNewPlus;

		}

		public ShowResult Show ()
		{
			var ret = control.Show ();
			if (ret == ShowResult.FIRST || ret == ShowResult.OK) {
				var hooks = control.instance.GetComponent<CourseHooks> ();
				if (hooks != null) {
					hooks.onConfirm = this.onConfirm;
					hooks.onOldRubber = this.onOldRubber;
					hooks.onNewRubber = this.onNewRubber;
					hooks.onOldPlus = this.onOldPlus;
					hooks.onNewPlus = this.onNewPlus;
				}
			}
			return ret;
		}
		public ShowResult SetAndShow(VoidFunc onConfirm, VoidFunc onOldRubber, VoidFunc onNewRubber, VoidFunc onOldPlus, VoidFunc onNewPlus){
			this.Set (onConfirm, onOldRubber, onNewRubber, onOldPlus, onNewPlus);
			return Show ();
		}
		public void Destroy(){
			control.Destroy ();
		}
		public void Disable(){
			control.Disable ();
		}
		public bool IsVisible(){
			return control.IsVisible();
		}
		public void Enable(){
			control.Enable ();
		}
	}
}
