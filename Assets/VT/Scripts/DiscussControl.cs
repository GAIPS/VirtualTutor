using System;
using UnityEngine;
using UnityEngine.UI;

namespace VT
{
	[Serializable]
	public class DiscussControl :IControl
	{
		private Control control;
		private VoidFunc onOne;
		private VoidFunc onTwo;
		private VoidFunc onThree;
		private string button1Text;
		private string button2Text;
		private string button3Text;

		public DiscussControl (GameObject prefab)
		{
			control = new Control ();
			control.prefab = prefab;
		}

		public void Set (string button1Text, string button2Text, string button3Text, VoidFunc onOne, VoidFunc onTwo, VoidFunc onThree)
		{
			this.button1Text = button1Text;
			this.button2Text = button2Text;
			this.button3Text = button3Text;
			this.onOne = onOne;
			this.onTwo = onTwo;
			this.onThree = onTwo;
		}
		public ShowResult Show()
		{
			var ret = control.Show ();
			if (ret == ShowResult.FIRST || ret == ShowResult.OK) {
				var hooks = control.instance.GetComponent<DiscussHooks> ();
				if (hooks != null) {
					hooks.Button1Text = this.button1Text;
					hooks.Button2Text = this.button2Text;
					hooks.Button3Text = this.button3Text;
					hooks.onOne = this.onOne;
					hooks.onTwo = this.onTwo;
					hooks.onThree = this.onThree;
				}
			}
			return ret;
		}

		public ShowResult SetAndShow(string button1Text,string button2Text, string button3Text, VoidFunc onOne, VoidFunc onTwo, VoidFunc onThree){
			this.Set (button1Text, button2Text, button3Text, onOne, onTwo, onThree);
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
