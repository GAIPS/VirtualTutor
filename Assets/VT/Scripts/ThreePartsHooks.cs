using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace VT{

public class ThreePartsHooks : Hooks {
		[SerializeField]
		private Text topicTextLeft = null;
		[SerializeField]
		private Text topicTextTop = null;
		[SerializeField]
		private Text topicTextRight = null;
		[SerializeField]
		private GameObject topicLeft;
		[SerializeField]
		private GameObject topicRight;
		[SerializeField]
		private GameObject topicTop;

		public VoidFunc onLeft;
		public VoidFunc onTop;
		public VoidFunc onRight;
		public void UILeft(){
			if (onLeft != null)
				onLeft ();
		}
		public void UITop(){
			if (onTop != null)
				onTop ();
		}
		public void UIRight(){
			if (onRight != null)
				onRight ();
		}

		public string ContentLeft {
			get{ return this.topicTextLeft.text; }
			set {
				if (!string.IsNullOrEmpty (value) && !value.Equals (this.topicTextLeft.text))
					this.topicTextLeft.text = value;
			}
		}

			public string ContentTop {
			get{ return this.topicTextTop.text; }
			set {
				if (!string.IsNullOrEmpty (value) && !value.Equals (this.topicTextTop.text))
					this.topicTextTop.text = value;
			}
		}

				public string ContentRight {
			get{ return this.topicTextRight.text; }
			set {
				if (!string.IsNullOrEmpty (value) && !value.Equals (this.topicTextRight.text))
					this.topicTextRight.text = value;
			}
		}

		public void DisableContentTop(){
		}

 }
}