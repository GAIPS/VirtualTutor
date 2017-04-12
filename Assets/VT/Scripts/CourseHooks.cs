using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VT
{
	public class CourseHooks : Hooks
	{
		[SerializeField]
		private GameObject confirm;
		[SerializeField]
		private GameObject newAddition;
		[SerializeField]
		private GameObject oldAddition;
		[SerializeField]
		private GameObject newRubber;
		[SerializeField]
		private GameObject oldRubber;
		[SerializeField]
		private GameObject oldGrade;
		[SerializeField]
		private GameObject newGrade;
		[SerializeField]
		private GameObject oldInput;
		[SerializeField]
		private GameObject newInput;
		[SerializeField]
		private Text oldGradeText;
		[SerializeField]
		private Text newGradeText = null;
		[SerializeField]
		private GameObject oldSendButton;
		[SerializeField]
		private GameObject newSendButton;

		public VoidFunc onConfirm;
		public VoidFunc onOldPlus;
		public VoidFunc onNewPlus;
		public VoidFunc onNewRubber;
		public VoidFunc onOldRubber;
		public StringFunc onOldInput;
		public StringFunc onNewInput;
		public VoidFunc sendOldgrade;
		public VoidFunc sendNewGrade;

		public void UIConfirm ()
		{
			if (onConfirm == null)
				return;
			onConfirm ();
		}
		public void UISendOldGrade(){
			if (sendOldgrade == null)
				return;
			oldSendButton.SetActive(false);
			oldInput.SetActive (false);
			oldGrade.SetActive (true);
			oldRubber.SetActive (true);
			sendOldgrade ();
		}
		public void UISendNewGrade(){
			if (sendNewGrade == null)
				return;
			newSendButton.SetActive (false);
			newInput.SetActive (false);
			newGrade.SetActive (true);
			newRubber.SetActive (true);

			sendNewGrade ();
		}

		public void UINewAddition ()
		{
			if (onNewPlus == null)
				return;
			newAddition.SetActive (false);
			newInput.SetActive (true);
			newSendButton.SetActive (true);

			onNewPlus ();
		}

		public void UIOldPlus ()
		{
			if (onOldPlus == null)
				return;
			oldInput.SetActive (true);
			oldSendButton.SetActive (true);
			onOldPlus ();
		}

		public void UINewRubber ()
		{
			if (onNewRubber == null)
				return;
			newRubber.SetActive (false);
			newGrade.SetActive (false);
			newAddition.SetActive (true);
			onNewRubber ();
		}

		public void UIOldRubber ()
		{
			if (onOldRubber == null)
				return;
			oldRubber.SetActive (false);
			oldGrade.SetActive (false);
			oldAddition.SetActive (true);
			onOldRubber ();
		}

		public void UIOldInput (string value)
		{
			onOldInput (value);
			OldGradeText = value;
		}

		public void UINewInput (string value)
		{
			onNewInput (value);
			NewGradeText = value;
		}

		public string OldGradeText {
			get {
				return this.oldGradeText.text;
			}
			set {
				if (!string.IsNullOrEmpty (value) && !value.Equals (this.oldGradeText.text))
					oldGradeText.text = value;
			}
		}

		public string NewGradeText {
			get {
				return this.newGradeText.text;
			}
			set {
				if (!string.IsNullOrEmpty (value) && !value.Equals (this.newGradeText.text))
					newGradeText.text = value;
			}
		}

	}
	
}
