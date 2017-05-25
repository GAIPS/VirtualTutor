using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace VT
{
	public class DiscussHooks : Hooks
	{
		[SerializeField]
		private GameObject confirm;

		public StringFunc input;
		public VoidFunc onConfirm;

		public void UIConfirm ()
		{
			if (onConfirm == null)
				return;
					
			onConfirm ();
		}

		public void UIInput (string value)
		{
			input (value);
		}
	

	}
}
