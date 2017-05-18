using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace VT
{
	public class DiscussHooks : Hooks
	{
		[SerializeField]
		private GameObject Button1;
		[SerializeField]
		private GameObject Button2;
		[SerializeField]
		private GameObject Button3;
		[SerializeField]
		private Text button1Text = null;
		[SerializeField]
		private Text button2Text = null;
		[SerializeField]
		private Text button3Text = null;

		public VoidFunc onOne;
		public VoidFunc onTwo;
		public VoidFunc onThree;

		public void UIone ()
		{
			if (onOne != null)
				onOne ();
		}

		public void UITwo ()
		{
			if (onTwo != null)
				onTwo ();
		}

		public void UIThree ()
		{
			if (onThree != null)
				onThree ();
		}

		public string Button1Text {
			get{ return button1Text.text; }
			set {
				if (!string.IsNullOrEmpty(value) && !value.Equals (this.button1Text.text))
					button1Text.text = value;
			}
		}

		public string Button2Text {
			get{ return button2Text.text; }
			set {
				if (!string.IsNullOrEmpty(value) && !value.Equals (this.button2Text.text))
					button2Text.text = value;
			}
		}
		public string Button3Text{
			get{ return button3Text.text;}
			set{
				if (!string.IsNullOrEmpty(value) && !value.Equals (this.button3Text.text))
					button3Text.text = value;
			}
		}
	}
}
