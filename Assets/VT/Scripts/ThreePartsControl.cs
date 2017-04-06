using System;
using UnityEngine;

namespace VT
{
	[Serializable]
	public class ThreePartsControl : IControl
	{
		private Control control;
		private string messageLeft;
		private string messageTop;
		private string messageRight;
		private VoidFunc left, top, right;
	
		public ThreePartsControl(GameObject prefab){
			control = new Control ();
			control.prefab = prefab;
		} 
		public void Set(string messageLeft,string messageTop,string messageRight, VoidFunc left, VoidFunc top, VoidFunc right){
			this.messageLeft = messageLeft;
			this.messageTop = messageTop;
			this.messageRight = messageRight;
			this.left = left;
			this.top = top;
			this.right = right;
		}
		public ShowResult Show(){
			var ret = control.Show ();
			if(ret == ShowResult.FIRST || ret == ShowResult.OK){
				var hooks = control.instance.GetComponent<ThreePartsHooks> ();
				if (hooks) {
					hooks.ContentLeft = messageLeft;
					hooks.ContentTop = messageTop;
					hooks.ContentRight = messageRight;
					hooks.onLeft = left;
					hooks.onTop = top;
					hooks.onRight = right;
				}
			}
			return ret;
		}
		public ShowResult SetAndShow(string messageLeft,string messageTop,string messageRight, VoidFunc left, VoidFunc top, VoidFunc right){
			this.Set (messageLeft,messageTop,messageRight, left, top, right);
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