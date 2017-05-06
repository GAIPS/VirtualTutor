﻿using System.Collections;
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
		private GameObject checkPoint2Add;
		[SerializeField]
		private GameObject newRubber;
		[SerializeField]
		private GameObject checkPoint2Rubber;
		[SerializeField]
		private GameObject oldRubber;
		[SerializeField]
		private GameObject oldGrade;
		[SerializeField]
		private GameObject newGrade;
		[SerializeField]
		private GameObject checkPoint2Grade;
		[SerializeField]
		private GameObject oldInput;
		[SerializeField]
		private GameObject newInput;
		[SerializeField]
		private GameObject checkPoint2Input;
		[SerializeField]
		private Text oldGradeText;
		[SerializeField]
		private Text newGradeText;
		[SerializeField]
		private Text checkPoint2GradeText = null;
		[SerializeField]
		private Text checkPoint2DateText = null;
		[SerializeField]
		private Text newDate = null;
		[SerializeField]
		private Text oldDate = null;
		[SerializeField]
		private Text checkPoint5Date = null;
		[SerializeField]
		private GameObject oldSendButton;
		[SerializeField]
		private GameObject newSendButton;
		[SerializeField]
		private GameObject checkPoint2Send;
		[SerializeField]
		private Text checkPoint3Date = null;
		[SerializeField]
		private GameObject Checkpoint3Check;

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

		public void UIConfirm ()
		{
			if (onConfirm == null)
				return;
			onConfirm ();
		}

		public void UISendOldGrade ()
		{
			if (sendOldgrade == null)
				return;
			oldSendButton.SetActive (false);
			oldInput.SetActive (false);
			oldGrade.SetActive (true);
			oldRubber.SetActive (true);
			sendOldgrade ();
		}

		public void UISendNewGrade ()
		{
			if (sendNewGrade == null)
				return;
			newSendButton.SetActive (false);
			newInput.SetActive (false);
			newGrade.SetActive (true);
			newRubber.SetActive (true);

			sendNewGrade ();
		}

		public void UISendCheck2Grade ()
		{
			if (sendCheck2Grade == null)
				return;
			checkPoint2Send.SetActive (false);
			checkPoint2Input.SetActive (false);
			checkPoint2Grade.SetActive (true);
			checkPoint2Rubber.SetActive (true);
			sendCheck2Grade ();
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
			oldAddition.SetActive (false);
			onOldPlus ();
		}

		public void UICheck2Plus ()
		{
			if (onCheck2Plus == null)
				return;
			checkPoint2Input.SetActive (true);
			checkPoint2Send.SetActive (true);
			checkPoint2Add.SetActive (false);
			onCheck2Plus ();
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

		public void UICheck2Rubber ()
		{
			if (onCheck2Rubber == null)
				return;
			checkPoint2Rubber.SetActive (false);
			checkPoint2Grade.SetActive (false);
			checkPoint2Add.SetActive (true);
			onCheck2Rubber ();
		}

		public void UIOldInput (string value)
		{
			onOldInput (value);
			OldGradeText = value;
		}

		public void UICheck2Input (string value)
		{
			onCheck2Input (value);
			CheckPoint2GradeText = value;
		}

		public void UINewInput (string value)
		{
			onNewInput (value);
			NewGradeText = value;
		}

		public void UIToggle (bool value)
		{
			if (toggle == null)
				return;
			toggle (value);
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

		public string CheckPoint3Date {
			get { 
				return this.checkPoint3Date.text;
			}
			set {
				if (!string.IsNullOrEmpty (value) && !value.Equals (this.checkPoint3Date.text))
					checkPoint3Date.text = value;
			}
		}

		public string CheckPoint2DateText {
			get {
				return this.checkPoint2DateText.text;
			}
			set {
				if (!string.IsNullOrEmpty (value) && !value.Equals (this.checkPoint2DateText.text))
					checkPoint2DateText.text = value;
			}
		}

		public string OldDate {
			get {
				return this.oldDate.text;
			}
			set {
				if (!string.IsNullOrEmpty (value) && !value.Equals (this.oldDate.text))
					oldDate.text = value;
			}
		}

		public string NewDate {
			get {
				return this.newDate.text;
			}
			set {
				if (!string.IsNullOrEmpty (value) && !value.Equals (this.newDate.text))
					newDate.text = value;
			}
		}

		public string Checkpoint5Date {
			get{ return this.checkPoint5Date.text; }
			set {
				if (!string.IsNullOrEmpty (value) && !value.Equals (this.checkPoint5Date.text))
					checkPoint5Date.text = value;
			}
		}

		public string CheckPoint2GradeText {
			get{ return this.checkPoint2GradeText.text; }
			set {
				if (!string.IsNullOrEmpty (value) && !value.Equals (this.checkPoint2GradeText.text))
					this.checkPoint2GradeText.text = value;
			}
		}
	}

	
}