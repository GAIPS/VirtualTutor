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
                    buttonControl.Instance.transform.SetParent(listObject.transform);
                }

                CheckpointHook checkpointHook = buttonControl.Instance.GetComponent<CheckpointHook>();
                if (checkpointHook != null) {
                    checkpointHook.Set(checkpoint);
                } else {
                    Debug.LogWarning("No Checkpoint Hook found.");
                }
            }
        }

        #endregion Checkpoints
    }

    
}
