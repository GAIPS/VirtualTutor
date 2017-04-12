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
		private List<Sprite> leftExpressions= null;
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
				if (!string.IsNullOrEmpty (value)) {
					leftImageObject.SetActive (true);
					this.leftText.text = value;
				} else if (string.IsNullOrEmpty (value))
					leftImageObject.SetActive (false);
				
				
			}
		}

		public string RightContent {
			get{ return this.rightText.text; }
			set {
				if (!string.IsNullOrEmpty (value)) {
					rightImageObject.SetActive (true);
					this.rightText.text = value;
				} else if (string.IsNullOrEmpty (value))
					rightImageObject.SetActive (false);
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
		public GameObject RightLine {
			get {
				return this.rightLine;
			}
			set {
				rightLine = value;
			}
		}

		public GameObject LeftLine {
			get {
				return this.leftLine;
			}
			set {
				leftLine = value;
			}
		}
		
	}
}
