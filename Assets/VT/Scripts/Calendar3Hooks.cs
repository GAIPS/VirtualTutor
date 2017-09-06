using HookControl;
using UnityEngine;
namespace VT {
    public class Calendar3Hooks :Hook {
		[SerializeField]
		GameObject Image;

		public VoidFunc click3;

		public void UIClickCalendar3(){
			if (click3 == null)
				return;
			click3 ();
		}
	}
}
