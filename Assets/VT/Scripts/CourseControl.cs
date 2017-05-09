using System;
using UnityEngine;

namespace VT
{
	[Serializable]
	public class CourseControl : IControl
	{
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
		public CourseControl (GameObject prefab)
		{
			control = new Control ();
			control.prefab = prefab;
		}

		public void Set (VoidFunc onConfirm, VoidFunc onOldRubber, VoidFunc onNewRubber, VoidFunc onOldPlus, VoidFunc onNewPlus, StringFunc onOldInput, StringFunc onNewInput, VoidFunc onOldSend, VoidFunc onNewSend, VoidFunc onCheck2Plus, VoidFunc onCheck2Rubber, VoidFunc sendCheck2Grade, StringFunc onCheck2Input, BoolFunc toggle, Checkpoint checkpoint1, Checkpoint checkpoint2, Checkpoint checkpoint3, Checkpoint checkpoint4, Checkpoint checkpoint5,FloatFunc easeSlider,FloatFunc onLikeSlider)
		{
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

		public ShowResult Show ()
		{
			var ret = control.Show ();
			if (ret == ShowResult.FIRST || ret == ShowResult.OK) {
				var hooks = control.instance.GetComponent<CourseHooks> ();
				if (hooks != null) {
					hooks.onConfirm = this.onConfirm;
					hooks.onOldRubber = this.onOldRubber;
					hooks.onNewRubber = this.onNewRubber;
					hooks.onOldPlus = this.onOldPlus;
					hooks.onNewPlus = this.onNewPlus;
					hooks.onNewInput = this.onNewInput;
					hooks.onOldInput = this.onOldInput;
					hooks.sendNewGrade = this.onNewSend;
					hooks.sendOldgrade = this.onOldSend;
					hooks.onCheck2Plus = this.onCheck2Plus;
					hooks.onCheck2Rubber = this.onCheck2Rubber;
					hooks.sendCheck2Grade = this.sendCheck2Grade;
					hooks.onCheck2Input = this.onCheck2Input;
					hooks.CheckPoint2DateText = this.checkpoint2.Date;
					hooks.CheckPoint3Date = this.checkpoint3.Date;
					hooks.Checkpoint5Date = this.checkpoint5.Date;
					hooks.OldDate = this.checkpoint1.Date;
					hooks.NewDate = this.checkpoint4.Date;
					hooks.toggle = this.toggle;
					hooks.EaseSlider = this.EaseSlider;
					hooks.onLikeSlider = this.onLikeSlider;
					var checkpoint1Test = checkpoint1 as Evaluation;
					if (checkpoint1Test != null) {
						hooks.OldGradeText = checkpoint1Test.Score;
					}
					var checkPoint2Test = checkpoint2 as Evaluation;
					if (checkPoint2Test != null) {
						hooks.CheckPoint2GradeText = checkPoint2Test.Score;
					}

					var checkPoint3Box = checkpoint3 as CheckBoxPoint;
					if (checkPoint3Box != null){
						hooks.CheckPoint3Value = checkPoint3Box.Done;						
					}

				}
			}
			return ret;
		}

		public ShowResult SetAndShow (VoidFunc onConfirm, VoidFunc onOldRubber, VoidFunc onNewRubber, VoidFunc onOldPlus, VoidFunc onNewPlus, StringFunc onOldInput, StringFunc onNewInput, VoidFunc onOldSend, VoidFunc onNewSend, VoidFunc onCheck2Plus, VoidFunc onCheck2Rubber, VoidFunc sendCheck2Grade, StringFunc onCheck2Input, BoolFunc toggle, Checkpoint checkpoint1, Checkpoint checkpoint2, Checkpoint checkpoint3, Checkpoint checkpoint4, Checkpoint checkpoint5,FloatFunc easeSlider, FloatFunc onLikeSlider)
		{
			this.Set (onConfirm, onOldRubber, onNewRubber, onOldPlus, onNewPlus, onOldInput, onNewInput, onOldSend, onNewSend, onCheck2Plus, onCheck2Rubber, sendCheck2Grade, onCheck2Input, toggle, checkpoint1, checkpoint2, checkpoint3, checkpoint4, checkpoint5,easeSlider,onLikeSlider);
			return Show ();
		}

		public void Destroy ()
		{
			control.Destroy ();
		}

		public void Disable ()
		{
			control.Disable ();
		}

		public bool IsVisible ()
		{
			return control.IsVisible ();
		}

		public void Enable ()
		{
			control.Enable ();
		}
	}
}
