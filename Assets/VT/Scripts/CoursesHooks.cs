using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VT
{
	public class CoursesHooks:Hooks
	{
		[SerializeField]
		GameObject Button;

		public VoidFunc clickCourse;

		public void onCourse(){
			if (clickCourse == null)
				return;
			clickCourse ();
		}

	}
}
