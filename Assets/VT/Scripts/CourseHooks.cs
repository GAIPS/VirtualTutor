using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VT {
    public class CourseHooks : Hooks {
        [SerializeField]
        private GameObject confirm;
        [SerializeField]
        private GameObject newAddition;
        [SerializeField]
        private GameObject oldAddition;
        [SerializeField]
        private GameObject checkPoint2Add;
        [SerializeField]
        private GameObject newRubber;
        [SerializeField]
        private GameObject checkPoint2Rubber;
        [SerializeField]
        private GameObject oldRubber;
        [SerializeField]
        private GameObject oldInput;
        [SerializeField]
        private GameObject newInput;
        [SerializeField]
        private GameObject checkPoint2Input;
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
        private GameObject Checkpoint3Check;
        [SerializeField]
        private bool checkPoint3Value;
        [SerializeField]
        private Slider EasySlider;
        [SerializeField]
        private Slider LikeSlider;
        [SerializeField]
        private Slider ImportanceSlider;
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
        public VoidFunc onOldPlus;
        public VoidFunc onNewPlus;
        public VoidFunc onCheck2Plus;
        public VoidFunc onNewRubber;
        public VoidFunc onOldRubber;
        public VoidFunc onCheck2Rubber;
        public StringFunc onOldInput;
        public StringFunc onNewInput;
        public StringFunc onCheck2Input;
        public VoidFunc sendOldgrade;
        public VoidFunc sendNewGrade;
        public VoidFunc sendCheck2Grade;
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

        public void UISendOldGrade(string value) {
            if (sendOldgrade == null)
                return;
            sendOldgrade();
        }

        public void UISendNewGrade(string value) {
            if (sendNewGrade == null)
                return;
            sendNewGrade();
        }

        public void UISendCheck2Grade(string value) {
            if (sendCheck2Grade == null)
                return;
            sendCheck2Grade();
        }

        public void UINewAddition() {
            if (onNewPlus == null)
                return;
            newInput.SetActive(true);
            newRubber.SetActive(true);
            newAddition.SetActive(false);

            onNewPlus();
        }

        public void UIOldPlus() {
            if (onOldPlus == null)
                return;
            oldInput.SetActive(true);
            oldRubber.SetActive(true);
            oldAddition.SetActive(false);
            onOldPlus();
        }

        public void UICheck2Plus() {
            if (onCheck2Plus == null)
                return;
            checkPoint2Input.SetActive(true);
            checkPoint2Rubber.SetActive(true);
            checkPoint2Add.SetActive(false);
            onCheck2Plus();
        }

        public void UINewRubber() {
            if (onNewRubber == null)
                return;
            newRubber.SetActive(false);
            newInput.SetActive(false);
            newAddition.SetActive(true);
            onNewRubber();
        }

        public void UIOldRubber() {
            if (onOldRubber == null)
                return;
            oldRubber.SetActive(false);
            oldInput.SetActive(false);
            oldAddition.SetActive(true);
            onOldRubber();
        }

        public void UICheck2Rubber() {
            if (onCheck2Rubber == null)
                return;
            checkPoint2Rubber.SetActive(false);
            checkPoint2Input.SetActive(false);
            checkPoint2Add.SetActive(true);
            onCheck2Rubber();
        }

        public void UIOldInput(string value) {
            onOldInput(value);
        }

        public void UIEaseSlider(float value) {
            value = EasySlider.value;
            EaseSlider(value);

        }

        public void UICheck2Input(string value) {
            onCheck2Input(value);
            CheckPoint2GradeText = value;
        }

        public void UINewInput(string value) {
            onNewInput(value);
            NewGradeText = value;
        }

        public void UIToggle(bool value) {
            if (toggle == null)
                return;
            toggle(value);
        }

        public string OldGradeText {
            get {
                var input = this.oldInput.GetComponent<InputField>();
                if (input) {
                    return input.text;
                }
                return string.Empty;
            }
            set {
                var input = this.oldInput.GetComponent<InputField>();
                if (input) {
                    if (!string.IsNullOrEmpty(value) && !value.Equals(input.text))
                        input.text = value;
                }
            }
        }

        public string NewGradeText {
            get {
                var input = this.newInput.GetComponent<InputField>();
                if (input) {
                    return input.text;
                }
                return string.Empty;
            }
            set {
                var input = this.newInput.GetComponent<InputField>();
                if (input) {
                    if (!string.IsNullOrEmpty(value) && !value.Equals(input.text))
                        input.text = value;
                }
            }
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

        public string CheckPoint2GradeText {
            get {
                var input = this.checkPoint2Input.GetComponent<InputField>();
                if (input) {
                    return input.text;
                }
                return string.Empty;
            }
            set {
                var input = this.checkPoint2Input.GetComponent<InputField>();
                if (input) {
                    if (!string.IsNullOrEmpty(value) && !value.Equals(input.text))
                        input.text = value;
                }
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
