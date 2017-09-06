using HookControl;
using UnityEngine;
namespace VT {
    public class Calendar1Hooks : Hook {

        [SerializeField]
        GameObject Image;

        public VoidFunc click1;

        public void UIClickCalendar1() {
            if (click1 == null)
                return;
            click1();
        }
    }
}
