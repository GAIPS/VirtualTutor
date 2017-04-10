using System;
using UnityEngine;

namespace VT
{
	[Serializable]
	public class ThreePartsControl : IControl
	{
		private Control control;
		private Topic currentTopic;
	
		public ThreePartsControl(GameObject prefab){
			control = new Control ();
			control.prefab = prefab;
		} 
		public void Set(Topic currentTopic){
			this.currentTopic = currentTopic;
		}
		public ShowResult Show(){
			var ret = control.Show ();
			if(ret == ShowResult.FIRST || ret == ShowResult.OK){
				var hooks = control.instance.GetComponent<ThreePartsHooks> ();
				if (hooks) {
					hooks.ContentLeft =currentTopic.Inputs[0].message;
					hooks.ContentTop = currentTopic.Inputs[1].message;
					hooks.ContentRight = currentTopic.Inputs[2].message;
					hooks.onLeft = currentTopic.Inputs [0].onClick;
					hooks.onTop = currentTopic.Inputs[1].onClick;
					hooks.onRight = currentTopic.Inputs[2].onClick;
				}
			}
			return ret;
		}
		public ShowResult SetAndShow(Topic currentTopic){
			this.Set (currentTopic);
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
	}
}