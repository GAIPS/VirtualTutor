using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace VT
{
	public class ExpressionsHooks : Hooks
	{
		[SerializeField]
		private Text leftText = null;
		[SerializeField]
		private Text rightText = null;
		[SerializeField]
		private List<Sprite> leftExpressions = null;
		[SerializeField]
		private List<Sprite> rightExpressions= null;
		[SerializeField]
		private Image rightImage = null;
		[SerializeField]
		private Image leftImage = null;
		[SerializeField]
		private GameObject rightLine;
		[SerializeField]
		private GameObject leftLine;
		[SerializeField]
		private GameObject leftImageObject;
		[SerializeField]
		private GameObject rightImageObject;
	

		public string LeftContent {
			get{ return this.leftText.text; }
			set {
				if (!string.IsNullOrEmpty (value) && !value.Equals (this.leftText.text))
					this.leftText.text = value;
			}
				
		}

		public string RightContent {
			get{ return this.rightText.text; }
			set {
				if (!string.IsNullOrEmpty (value) && !value.Equals (this.rightText.text))
					this.rightText.text = value;
			}
		}

		public  Sprite LeftSprite {
			get{ return this.leftImage.sprite; }
			set{ this.leftImage.sprite = value; }
		}

		public Sprite RightSprite {
			get{ return this.rightImage.sprite; }
			set{ this.rightImage.sprite = value; }
		}

		public List<Sprite> LeftSprites {
			get{ return this.leftExpressions; }
			set{ this.leftExpressions = value; }
		}

		public List<Sprite> RightSprites {
			get{ return this.rightExpressions; }
			set{ this.rightExpressions = value; }
		}
	}
}
