using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VT
{

	public class ThreePartsHooks : Hooks
	{
		[SerializeField]
		private Text topicTextLeft = null;
		[SerializeField]
		private Text topicTextTop = null;
		[SerializeField]
		private Text topicTextRight = null;
		[SerializeField]
		private Text topicTextExtra = null;
		[SerializeField]
        private GameObject topicLeft = null;
		[SerializeField]
        private GameObject topicRight = null;
		[SerializeField]
        private GameObject topicTop = null;
		[SerializeField]
        private GameObject topicExtra = null;
        

		public VoidFunc onLeft;
		public VoidFunc onTop;
		public VoidFunc onRight;
		public VoidFunc onExtra;

		public void UIExtra()
		{
			if (onExtra != null)
				onExtra ();
		}
		public void UILeft ()
		{
			if (onLeft != null)
				onLeft ();
		}

		public void UITop ()
		{
			if (onTop != null)
				onTop ();
		}

		public void UIRight ()
		{
			if (onRight != null)
				onRight ();
		}

		public string ContentLeft {
			get{ return this.topicTextLeft.text; }
			set {
				if (!string.IsNullOrEmpty (value)) {
                    show(topicLeft);
                    this.topicTextLeft.text = value;
				} else if (string.IsNullOrEmpty (value))
                    hide(topicLeft);
			}
		}

		public string ContentTop {
			get{ return this.topicTextTop.text; }
			set {
				if (!string.IsNullOrEmpty (value)) {
                    show(topicTop);
					this.topicTextTop.text = value;
                } else if (string.IsNullOrEmpty (value))
                    hide(topicTop);
			}
		}

		public string ContentRight {
			get{ return this.topicTextRight.text; }
			set {
				if (!string.IsNullOrEmpty (value)) {
                    show(topicRight);
					this.topicTextRight.text = value;
				} else if (string.IsNullOrEmpty (value))
                    hide(topicRight);
			}
        }

		public string ContentExtra {
			get{ return this.topicTextExtra.text; }
			set{if (!string.IsNullOrEmpty (value)) {
					show (topicExtra);
					this.topicTextExtra.text = value;
				} else if (string.IsNullOrEmpty (value))
					hide (topicExtra);
			}
		}

        protected void show(GameObject ballon) {
            if (!ballon) {
                return;
            }
            var animator = ballon.GetComponent<Animator>();
            if (animator) {
                animator.SetBool("Showing", true);
            } else {
                ballon.SetActive(true);
            }
        }

        protected void hide(GameObject ballon) {
            if (!ballon) {
                return;
            }
            var animator = ballon.GetComponent<Animator>();
            if (animator) {
                animator.SetBool("Showing", false);
            } else {
                ballon.SetActive(false);
            }
        }

	}
}