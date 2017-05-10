using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VT
{
	public class CoursesHooks:Hooks
	{
		[SerializeField]
		GameObject Button;

		public VoidFunc clickCourse;

		public void onCourse(){
			if (clickCourse == null)
				return;
			clickCourse ();
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
