using HookControl;
using System;
using UnityEngine;

namespace VT {
    [Serializable]
	public class DiscussControl :IControl
	{
		private Control control;
		private VoidFunc onConfrim;
		private StringFunc onInput;
		public DiscussControl (GameObject prefab)
		{
			control = new Control ();
			control.prefab = prefab;
		}

		public void Set (VoidFunc onConfirm, StringFunc onInput)
		{
			this.onInput = onInput;
			this.onConfrim = onConfirm;
		}
		public ShowResult Show()
		{
			var ret = control.Show ();
			if (ret == ShowResult.FIRST || ret == ShowResult.OK) {
				var hooks = control.instance.GetComponent<DiscussHooks> ();
				if (hooks != null) {
					hooks.input = this.onInput;
					hooks.onConfirm = this.onConfrim;
				}
			}
			return ret;
		}

		public ShowResult SetAndShow(VoidFunc onConfirm, StringFunc onInput){
			this.Set (onConfirm,onInput);
			return Show ();
		}
		public void Disable(){
			control.Disable ();
		}	
		public void Destroy(){
			control.Destroy ();
		}
		public bool IsVisible(){
			return control.IsVisible ();
		}
		public void Enable(){
			control.Enable ();
		}
	}
}
