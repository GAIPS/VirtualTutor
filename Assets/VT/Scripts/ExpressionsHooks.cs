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
		private List<Sprite> rightExpressions = null;
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
		[SerializeField]
		private List<AudioClip> audios = null;
		[SerializeField]
		private AudioSource audioSource;

		public string LeftContent {
			get{ return this.leftText.text; }
			set {
				if (!string.IsNullOrEmpty (value)) {
					show (leftLine);
					this.leftText.text = value;
				} else
					hide (leftLine);
				
				
			}
		}

		public string RightContent {
			get{ return this.rightText.text; }
			set {
				if (!string.IsNullOrEmpty (value)) {
					show (rightLine);
					this.rightText.text = value;
				} else
					hide (rightLine);
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

		public List<AudioClip> Audios {
			get {
				return this.audios;
			}
			set {
				audios = value;
			}
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

		protected void show (GameObject ballon)
		{
			if (!ballon) {
				return;
			}
			var animator = ballon.GetComponent<Animator> ();
			if (animator) {
				animator.SetBool ("Showing", true);
			} else {
				ballon.SetActive (true);
			}
		}

		protected void hide (GameObject ballon)
		{
			if (!ballon) {
				return;
			}
			var animator = ballon.GetComponent<Animator> ();
			if (animator) {
				animator.SetBool ("Showing", false);
			} else {
				ballon.SetActive (false);
			}
		}
public AudioSource AudioSource {
			get {
				return this.audioSource;
			}
			set {
				audioSource = value;
			}
		}
		
	}
}
