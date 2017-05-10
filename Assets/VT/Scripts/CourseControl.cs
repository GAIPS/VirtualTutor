using System;
using UnityEngine;

namespace VT {
    [Serializable]
    public class CourseControl : IControl {
        private CourseHooks hook;

        private Control control;
        private VoidFunc onConfirm;
        private VoidFunc onOldRubber;
        private VoidFunc onNewRubber;
        private VoidFunc onOldPlus;
        private VoidFunc onNewPlus;
        private StringFunc onOldInput;
        private StringFunc onNewInput;
        private VoidFunc onOldSend;
        private VoidFunc onNewSend;
        private VoidFunc onCheck2Plus;
        private VoidFunc onCheck2Rubber;
        private VoidFunc sendCheck2Grade;
        private StringFunc onCheck2Input;
        private BoolFunc toggle;
        private FloatFunc EaseSlider;
        private Checkpoint checkpoint1;
        private Checkpoint checkpoint2;
        private Checkpoint checkpoint3;
        private Checkpoint checkpoint4;
        private Checkpoint checkpoint5;
        private FloatFunc onLikeSlider;

        public CourseControl(GameObject prefab) {
            control = new Control();
            control.prefab = prefab;
        }

        public void Set(VoidFunc onConfirm,
                   VoidFunc onOldRubber,
                   VoidFunc onNewRubber,
                   VoidFunc onOldPlus,
                   VoidFunc onNewPlus,
                   StringFunc onOldInput,
                   StringFunc onNewInput,
                   VoidFunc onOldSend,
                   VoidFunc onNewSend,
                   VoidFunc onCheck2Plus,
                   VoidFunc onCheck2Rubber,
                   VoidFunc sendCheck2Grade,
                   StringFunc onCheck2Input,
                   BoolFunc toggle,
                   Checkpoint checkpoint1,
                   Checkpoint checkpoint2,
                   Checkpoint checkpoint3,
                   Checkpoint checkpoint4,
                   Checkpoint checkpoint5,
                   FloatFunc easeSlider,
                   FloatFunc onLikeSlider) {
            this.onConfirm = onConfirm;
            this.onOldRubber = onOldRubber;
            this.onNewRubber = onNewRubber;
            this.onOldPlus = onOldPlus;
            this.onNewPlus = onNewPlus;
            this.onOldInput = onOldInput;
            this.onNewInput = onNewInput;
            this.onNewSend = onNewSend;
            this.onOldSend = onOldSend;
            this.onCheck2Plus = onCheck2Plus;
            this.onCheck2Rubber = onCheck2Rubber;
            this.sendCheck2Grade = sendCheck2Grade;
            this.onCheck2Input = onCheck2Input;
            this.toggle = toggle;
            this.checkpoint1 = checkpoint1;
            this.checkpoint2 = checkpoint2;
            this.checkpoint3 = checkpoint3;
            this.checkpoint4 = checkpoint4;
            this.checkpoint5 = checkpoint5;
            this.EaseSlider = easeSlider;
            this.onLikeSlider = onLikeSlider;

        }

        public ShowResult Show() {
            var ret = control.Show();
            if (ret == ShowResult.FIRST || ret == ShowResult.OK) {
                hook = control.instance.GetComponent<CourseHooks>();
                if (hook != null) {
                    hook.onConfirm = this.onConfirm;
                    hook.onOldRubber = this.onOldRubber;
                    hook.onNewRubber = this.onNewRubber;
                    hook.onOldPlus = this.onOldPlus;
                    hook.onNewPlus = this.onNewPlus;
                    hook.onNewInput = this.onNewInput;
                    hook.onOldInput = this.onOldInput;
                    hook.sendNewGrade = this.onNewSend;
                    hook.sendOldgrade = this.onOldSend;
                    hook.onCheck2Plus = this.onCheck2Plus;
                    hook.onCheck2Rubber = this.onCheck2Rubber;
                    hook.sendCheck2Grade = this.sendCheck2Grade;
                    hook.onCheck2Input = this.onCheck2Input;
                    hook.CheckPoint2DateText = this.checkpoint2.Date;
                    hook.CheckPoint3Date = this.checkpoint3.Date;
                    hook.Checkpoint5Date = this.checkpoint5.Date;
                    hook.OldDate = this.checkpoint1.Date;
                    hook.NewDate = this.checkpoint4.Date;
                    hook.toggle = this.toggle;
                    hook.EaseSlider = this.EaseSlider;
                    hook.onLikeSlider = this.onLikeSlider;
                    var checkpoint1Test = checkpoint1 as Evaluation;
                    if (checkpoint1Test != null) {
                        hook.OldGradeText = checkpoint1Test.Score;
                    }
                    var checkPoint2Test = checkpoint2 as Evaluation;
                    if (checkPoint2Test != null) {
                        hook.CheckPoint2GradeText = checkPoint2Test.Score;
                    }

                    var checkPoint3Box = checkpoint3 as CheckBoxPoint;
                    if (checkPoint3Box != null) {
                        hook.CheckPoint3Value = checkPoint3Box.Done;						
                    }

                    hook.Show();
                }
            }
            return ret;
        }

        public ShowResult SetAndShow(VoidFunc onConfirm,
                                VoidFunc onOldRubber,
                                VoidFunc onNewRubber,
                                VoidFunc onOldPlus,
                                VoidFunc onNewPlus,
                                StringFunc onOldInput,
                                StringFunc onNewInput,
                                VoidFunc onOldSend,
                                VoidFunc onNewSend,
                                VoidFunc onCheck2Plus,
                                VoidFunc onCheck2Rubber,
                                VoidFunc sendCheck2Grade,
                                StringFunc onCheck2Input,
                                BoolFunc toggle,
                                Checkpoint checkpoint1,
                                Checkpoint checkpoint2,
                                Checkpoint checkpoint3,
                                Checkpoint checkpoint4,
                                Checkpoint checkpoint5,
                                FloatFunc easeSlider,
                                FloatFunc onLikeSlider) {
            this.Set(onConfirm,
             onOldRubber,
             onNewRubber,
             onOldPlus,
             onNewPlus,
             onOldInput,
             onNewInput,
             onOldSend,
             onNewSend,
             onCheck2Plus,
             onCheck2Rubber,
             sendCheck2Grade,
             onCheck2Input,
             toggle,
             checkpoint1,
             checkpoint2,
             checkpoint3,
             checkpoint4,
             checkpoint5,
             easeSlider,
             onLikeSlider);
            return Show();
        }

        public void Destroy() {
            if (hook) {
                hook.onHideEnded = () => {
                    control.Destroy();
                };
                hook.Hide();
            } else {
                control.Destroy();
            }
        }

        public void Disable() {
            if (hook) {
                hook.onHideEnded = () => {
                    control.Disable();
                };
                hook.Hide();
            } else {
                control.Disable();
            }
        }

        public bool IsVisible() {
            return control.IsVisible();
        }

        public void Enable() {
            control.Enable();
        }
    }
}
