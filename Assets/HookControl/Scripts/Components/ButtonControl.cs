using System;
using UnityEngine;

namespace HookControl {
    /// <summary>
    /// This class is more of a way to use than a real usage cenario, although it's fully usable.
    /// </summary>
    [Serializable]
    public class ButtonControl : IControl {

        private Control control;

        private VoidFunc onClick;
        private string text;

        public ButtonControl(GameObject prefab) {
            control = new Control();
            control.prefab = prefab;
        }

        public void Set(string text, VoidFunc onClick) {
            this.text = text;
            this.onClick = onClick;
        }
        public ShowResult Show() {
            var ret = control.Show();
            if (ret == ShowResult.FIRST || ret == ShowResult.OK) {
                var hooks = control.instance.GetComponent<ButtonHook>();
                if (hooks != null) {
                    hooks.text = this.text;
                    hooks.onClick = this.onClick;
                }
            }
            return ret;
        }

        public ShowResult SetAndShow(string text, VoidFunc click1) {
            this.Set(text, click1);
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
