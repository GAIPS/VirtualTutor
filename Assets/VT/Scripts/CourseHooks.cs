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
		public VoidFunc onConfirm;
		public VoidFunc onOldPlus;
		public VoidFunc onNewPlus;
		public VoidFunc onNewRubber;
		public VoidFunc onOldRubber;

		public void UIConfirm ()
		{
			if (onConfirm == null)
				return;
			onConfirm ();
		}

		public void UINewAddition ()
		{
			if (onNewPlus == null)
				return;
			newAddition.SetActive (false);
			newGrade.SetActive (true);
			newRubber.SetActive (true);
			onNewPlus ();
		}

		public void UIOldPlus ()
		{
			if (onOldPlus == null)
				return;
			oldAddition.SetActive (false);
			oldRubber.SetActive (true);
			oldGrade.SetActive (true);
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

	}
	
}
