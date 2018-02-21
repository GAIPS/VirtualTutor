using HookControl;
using UnityEngine;

namespace VT {
    public class DiscussControl : IControl {
        private Control control;

        private string message = string.Empty;

        public event StringFunc MessageEvent;

        public DiscussControl(GameObject prefab) {
            control = new Control();
            control.prefab = prefab;
        }

        public ShowResult Show() {
            var ret = control.Show();
            if (ret == ShowResult.FIRST || ret == ShowResult.OK) {
                var hooks = control.instance.GetComponent<DiscussHooks>();
                if (hooks != null) {
                    hooks.onMessageInput = (string message) => {
                        this.message = message;
                    };
                    hooks.onConfirm = () => {
                        if (MessageEvent != null) {
                            MessageEvent(message);
                        }
                    };
                }
            }
            return ret;
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
