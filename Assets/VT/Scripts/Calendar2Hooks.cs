using HookControl;
using UnityEngine;

namespace VT {
    public class Calendar2Hooks : Hook {
        [SerializeField]
        GameObject Image;

        public VoidFunc click2;

        public void UIClickCalendar2() {
            if (click2 == null)
                return;
            click2();
        }
    }
}
