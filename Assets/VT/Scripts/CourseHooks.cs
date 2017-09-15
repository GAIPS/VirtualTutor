using HookControl;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VT {
    public class CourseHooks : Hook {
//        [SerializeField]
//        private GameObject confirm = null;
        
        [SerializeField]
        private Slider EasySlider = null;
        [SerializeField]
        private Slider LikeSlider = null;
        [SerializeField]
        private Slider ImportanceSlider = null;
        [SerializeField]
        private Text courseName = null;
        public VoidFunc onConfirm;
        public FloatFunc onEaseSlider;
        public FloatFunc onLikeSlider;
        public FloatFunc onImportanceSlider;

        public void UIConfirm() {
            if (onConfirm == null)
                return;
            onConfirm();
        }

        public void UILikeSlider(float value) {
            value = LikeSlider.value;
            onLikeSlider(value);
        }

        public void UIImportanceSlider(float value) {
            value = ImportanceSlider.value;
            onImportanceSlider(value);
        }

       

        public void UIEaseSlider(float value) {
            value = EasySlider.value;
            onEaseSlider(value);

        }
        
        public string CourseName {
            get{ return this.courseName.text; }
            set { 
                if (!string.IsNullOrEmpty(value) && !value.Equals(this.courseName.text))
                    this.courseName.text = value;
            }

        }
        
        #region Checkpoints

        [SerializeField]
        private GameObject checkpointPrefab;
        [SerializeField]
        private GameObject listObject;

        public List<Control> checkpointsControls;

        public void AddCheckpoint(Checkpoint checkpoint) {
            Control buttonControl = new Control(checkpointPrefab);
            ShowResult result = buttonControl.Show();
            if (result == ShowResult.FIRST) {
                checkpointsControls.Add(buttonControl);
                if (listObject) {
                    buttonControl.instance.transform.SetParent(listObject.transform);
                }

                CheckpointHook checkpointHook = buttonControl.instance.GetComponent<CheckpointHook>();
                if (checkpointHook != null) {
                    checkpointHook.Set(checkpoint);
                } else {
                    Debug.LogWarning("No Checkpoint Hook found.");
                }
            }
        }

        #endregion Checkpoints

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
