using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VT
{
	public class CoursesHooks:Hooks
	{
		[SerializeField]
		GameObject Button;
		[SerializeField]
		private Text course1Name = null;
		[SerializeField]
		private Text course2Name = null;

		public VoidFunc clickCourse;
		public VoidFunc clickCourse2;

		public void onCourse(){
			if (clickCourse == null)
				return;
			clickCourse ();
        }
		public void onCourse2(){
			if (clickCourse2 == null)
				return;
			clickCourse2();
		}
		public string Course1Name{
			get{ return this.course1Name.text; }
			set{ 
				if (!string.IsNullOrEmpty (value) && !value.Equals (this.course1Name.text))
					this.course1Name.text = value;
			}
		}
		public string Course2Name{
			get{ return this.course2Name.text; }
			set{ 
				if (!string.IsNullOrEmpty (value) && !value.Equals (this.course2Name.text))
					this.course2Name.text = value;
			}
		}
        #region Animations

        [SerializeField]
        private Animator showHideAnimator;

        /// <summary>
        /// Called when the Show animation ended.
        /// </summary>
        public VoidFunc onShowEnded;
        /// <summary>
        /// Called when the Hide animation ended.
        /// </summary>
        public VoidFunc onHideEnded;

        public void Show() {
            if (showHideAnimator) {
                showHideAnimator.SetBool("Showing", true);
            } else {
                Debug.LogWarning("Not animator found while trying to show.");
                UIOnShowEnded();
            }
        }

        public void Hide() {
            if (showHideAnimator) {
                showHideAnimator.SetBool("Showing", false);
            } else {
                Debug.LogWarning("Not animator found while trying to hide.");
                UIOnHideEnded();
            }
        }

        public void UIOnShowEnded () {
            if (onShowEnded != null) {
                onShowEnded();
            }
        }

        public void UIOnHideEnded () {
            if (onHideEnded != null) {
                onHideEnded();
            }
        }

        #endregion

	}
}
