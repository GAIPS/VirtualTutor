using HookControl;
using UnityEngine;

namespace VT {
    public class CoursesHooks : Hook {
        
        [SerializeField]
        private ButtonListHook listView;

        public void AddButton(string text, VoidFunc onClick) {
            if (listView) {
                listView.AddButton(text, onClick);
            } else {
                Debug.LogWarning("No List View found.", this);
            }
        }

        #region Animations

        [SerializeField]
        private Animator showHideAnimator = null;

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

        public void UIOnShowEnded() {
            if (onShowEnded != null) {
                onShowEnded();
            }
        }

        public void UIOnHideEnded() {
            if (onHideEnded != null) {
                onHideEnded();
            }
        }

        #endregion

    }
}
