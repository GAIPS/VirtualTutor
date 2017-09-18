using HookControl;
using UnityEngine;

namespace VT {
    public class BalloonsControl : IControl {
        private Control control;
        private Topic currentTopic;
        public float Start = 0.0f;
        BalloonsHooks hooks;
        bool started;

        public BalloonsControl(GameObject prefab) {
            control = new Control();
            control.prefab = prefab;
        }

        public void Set(Topic currentTopic) {
            this.currentTopic = currentTopic;
            Start = 0.0f;
        }

        public ShowResult Show() {
            started = true;
            var ret = control.Show();
            if (ret == ShowResult.FIRST || ret == ShowResult.OK) {
                hooks = control.instance.GetComponent<BalloonsHooks>();
                if (hooks) {
//					hooks.ContentLeft = currentTopic.Inputs [0].message;
//					hooks.ContentTop = currentTopic.Inputs [1].message;
//                  hooks.ContentRight = currentTopic.Inputs [2].message;
                    hooks.ContentLeft = null;
                    hooks.ContentTop = null;
                    hooks.ContentRight = null;
                    hooks.ContentExtra = null;
                    hooks.onLeft = currentTopic.Inputs[0].onClick;
                    hooks.onTop = currentTopic.Inputs[1].onClick;
                    hooks.onRight = currentTopic.Inputs[2].onClick;
                    if (currentTopic.Inputs.Length > 3) {
                        hooks.onExtra = currentTopic.Inputs[3].onClick;
                    }
                }
            }
            return ret;
            
        }

        public void update(float delta) {
            Start += delta;
            Topic.Input inputLeft = currentTopic.Inputs[0];
            Topic.Input inputTop = currentTopic.Inputs[1];
            Topic.Input inputRight = currentTopic.Inputs[2];
            if (hooks && started) {
                if (inputLeft.start <= Start) {
                    hooks.ContentLeft = inputLeft.message;
                    hooks.onLeft = inputLeft.onClick;
                } else {
                    if (inputLeft.message != null) {
                        hooks.ContentLeft = null;
                    }
                }
                if (inputTop.start <= Start) {
                    hooks.ContentTop = inputTop.message;
                    hooks.onTop = inputTop.onClick;
                } else {
                    if (inputTop.message != null) {
                        hooks.ContentTop = null;
                    }
                }
                if (inputRight.start <= Start) {
                    hooks.ContentRight = inputRight.message;
                    hooks.onRight = inputRight.onClick;
                } else {
                    if (inputRight.message != null) {
                        hooks.ContentRight = null;
                    }
                }
                if (currentTopic.Inputs.Length > 3) {
                    Topic.Input inputExtra = currentTopic.Inputs[3];
                    if (inputExtra.start <= Start) {
                        hooks.ContentExtra = inputExtra.message;
                        hooks.onExtra = inputExtra.onClick;
                    } else {
                        if (inputExtra.message != null) {
                            hooks.ContentExtra = null;
                        }
                    }
                }

            }
        }

        public ShowResult SetAndShow(Topic currentTopic) {
            this.Set(currentTopic);
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