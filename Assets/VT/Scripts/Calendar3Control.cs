using HookControl;
using System;
using UnityEngine;

namespace VT {
    [Serializable]

    public class Calendar3Control : IControl {

        private Control control;
        private VoidFunc click3;

        public Calendar3Control(GameObject prefab) {
            control = new Control();
            control.prefab = prefab;
        }

        public void Set(VoidFunc click3) {
            this.click3 = click3;
        }
        public ShowResult Show() {
            var ret = control.Show();
            if (ret == ShowResult.FIRST || ret == ShowResult.OK) {
                var hooks = control.instance.GetComponent<Calendar3Hooks>();
                if (hooks != null) {
                    hooks.click3 = this.click3;
                }
            }
            return ret;
        }

        public ShowResult SetAndShow(VoidFunc click3) {
            this.Set(click3);
            return Show();
        }

        public void Disable() {
            control.Disable();
        }

        public void Destroy() {
            control.Destroy();
        }

        public bool IsVisible() {
            return control.IsVisible();
        }
        public void Enable() {
            control.Enable();
        }
    }
}
