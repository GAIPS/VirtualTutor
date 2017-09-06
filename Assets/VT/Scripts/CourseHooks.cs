using HookControl;
using UnityEngine;
using UnityEngine.UI;

namespace VT {
    public class CourseHooks : Hook {
//        [SerializeField]
//        private GameObject confirm = null;
        [SerializeField]
        private Text newGradeText = null;
        [SerializeField]
        private Text checkPoint2DateText = null;
        [SerializeField]
        private Text newDate = null;
        [SerializeField]
        private Text oldDate = null;
        [SerializeField]
        private Text checkPoint5Date = null;
        [SerializeField]
        private Text checkPoint3Date = null;
        [SerializeField]
        private bool checkPoint3Value;
        [SerializeField]
        private Slider EasySlider = null;
        [SerializeField]
        private Slider LikeSlider = null;
        [SerializeField]
        private Slider ImportanceSlider = null;
        [SerializeField]
        private Text courseName = null;
        [SerializeField]
        private Text checkpoint1Title = null;
        [SerializeField]
        private Text checkpoint2Title = null;
        [SerializeField]
        private Text checkpoint3Title = null;
        [SerializeField]
        private Text checkpoint4Title = null;
        [SerializeField]
        private Text checkpoint5Title = null;
        public VoidFunc onConfirm;
        public BoolFunc toggle;
        public FloatFunc EaseSlider;
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
            EaseSlider(value);

        }
            
       
        public void UIToggle(bool value) {
            if (toggle == null)
                return;
            toggle(value);
        }

       
       

        public string CheckPoint3Date {
            get { 
                return this.checkPoint3Date.text;
            }
            set {
                if (!string.IsNullOrEmpty(value) && !value.Equals(this.checkPoint3Date.text))
                    checkPoint3Date.text = value;
            }
        }

        public string NewGradeText{
            get{
                return this.newGradeText.text;
            }
            set{ 
                if (!string.IsNullOrEmpty (value) && !value.Equals (this.newGradeText.text))
                    newGradeText.text = value;
            }
        }

        public string CheckPoint2DateText {
            get {
                return this.checkPoint2DateText.text;
            }
            set {
                if (!string.IsNullOrEmpty(value) && !value.Equals(this.checkPoint2DateText.text))
                    checkPoint2DateText.text = value;
            }
        }

        public string OldDate {
            get {
                return this.oldDate.text;
            }
            set {
                if (!string.IsNullOrEmpty(value) && !value.Equals(this.oldDate.text))
                    oldDate.text = value;
            }
        }

        public string NewDate {
            get {
                return this.newDate.text;
            }
            set {
                if (!string.IsNullOrEmpty(value) && !value.Equals(this.newDate.text))
                    newDate.text = value;
            }
        }

        public string Checkpoint5Date {
            get{ return this.checkPoint5Date.text; }
            set {
                if (!string.IsNullOrEmpty(value) && !value.Equals(this.checkPoint5Date.text))
                    checkPoint5Date.text = value;
            }
        }

        

        public bool CheckPoint3Value {
            get {
                return this.checkPoint3Value;
            }
            set {
                checkPoint3Value = value;
            }
        }

        public string CourseName {
            get{ return this.courseName.text; }
            set { 
                if (!string.IsNullOrEmpty(value) && !value.Equals(this.courseName.text))
                    this.courseName.text = value;
            }

        }

        public string Checkpoint1Title {
            get{ return this.checkpoint1Title.text; }
            set { 
                if (!string.IsNullOrEmpty(value) && !value.Equals(this.checkpoint1Title.text))
                    this.checkpoint1Title.text = value;
            }
        }

        public string Checkpoint2Title {
            get{ return this.checkpoint2Title.text; }
            set { 
                if (!string.IsNullOrEmpty(value) && !value.Equals(this.checkpoint2Title.text))
                    this.checkpoint2Title.text = value;
            }
        }

        public string Checkpoint3Title {
            get{ return this.checkpoint3Title.text; }
            set { 
                if (!string.IsNullOrEmpty(value) && !value.Equals(this.checkpoint3Title.text))
                    this.checkpoint3Title.text = value;
            }
        }

        public string Checkpoint4Title {
            get{ return this.checkpoint4Title.text; }
            set { 
                if (!string.IsNullOrEmpty(value) && !value.Equals(this.checkpoint4Title.text))
                    this.checkpoint4Title.text = value;
            }
        }

        public string Checkpoint5Title {
            get{ return this.checkpoint5Title.text; }
            set { 
                if (!string.IsNullOrEmpty(value) && !value.Equals(this.checkpoint5Title.text))
                    this.checkpoint5Title.text = value;
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
