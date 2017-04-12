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
		private StringFunc onOldInput;
		private StringFunc onNewInput;
		private VoidFunc onOldSend;
		private VoidFunc onNewSend;

		public CourseControl (GameObject prefab)
		{
			control = new Control ();
			control.prefab = prefab;
		}

		public void Set (VoidFunc onConfirm, VoidFunc onOldRubber, VoidFunc onNewRubber, VoidFunc onOldPlus, VoidFunc onNewPlus, StringFunc onOldInput, StringFunc onNewInput,VoidFunc onOldSend, VoidFunc onNewSend)
		{
			this.onConfirm = onConfirm;
			this.onOldRubber = onOldRubber;
			this.onNewRubber = onNewRubber;
			this.onOldPlus = onOldPlus;
			this.onNewPlus = onNewPlus;
			this.onOldInput = onOldInput;
			this.onNewInput = onNewInput;
			this.onNewSend = onNewSend;
			this.onOldSend = onOldSend;
	

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
					hooks.onNewInput = this.onNewInput;
					hooks.onOldInput = this.onOldInput;
					hooks.sendNewGrade = this.onNewSend;
					hooks.sendOldgrade = this.onOldSend;
				}
			}
			return ret;
		}
		public ShowResult SetAndShow(VoidFunc onConfirm, VoidFunc onOldRubber, VoidFunc onNewRubber, VoidFunc onOldPlus, VoidFunc onNewPlus, StringFunc onOldInput, StringFunc onNewInput, VoidFunc onOldSend, VoidFunc onNewSend){
			this.Set (onConfirm, onOldRubber, onNewRubber, onOldPlus, onNewPlus,onOldInput,onNewInput,onOldSend,onNewSend);
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
