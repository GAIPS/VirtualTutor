using System;
using UnityEngine;

namespace VT {
    [Serializable]
    public class ThreePartsControl : IControl {
        private Control control;
        private Topic currentTopic;
        private float start = 0.0f;
        ThreePartsHooks hooks;
        bool started;

        public ThreePartsControl(GameObject prefab) {
            control = new Control();
            control.prefab = prefab;
        }

        public void Set(Topic currentTopic) {
            this.currentTopic = currentTopic;
        }

        public ShowResult Show() {
            started = true;
            var ret = control.Show();
            if (ret == ShowResult.FIRST || ret == ShowResult.OK) {
                hooks = control.instance.GetComponent<ThreePartsHooks>();
                if (hooks) {
                    hooks.ContentLeft = currentTopic.Inputs[0].message;
                    hooks.ContentTop = currentTopic.Inputs[1].message;
                    hooks.ContentRight = currentTopic.Inputs[2].message;
                    hooks.onLeft = currentTopic.Inputs[0].onClick;
                    hooks.onTop = currentTopic.Inputs[1].onClick;
                    hooks.onRight = currentTopic.Inputs[2].onClick;
                }
            }
            return ret;
        }

        public void update(float delta) {
            start += delta;
            Topic.Input inputLeft = currentTopic.Inputs[0];
            Topic.Input inputTop = currentTopic.Inputs[1];
            Topic.Input inputRight = currentTopic.Inputs[2];
            if (hooks && started) {
                if (inputLeft.message != null && inputLeft.start > start) {
                    hooks.ContentLeft = null;
                }
                if (inputTop.message != null && inputTop.start > start) {
                    hooks.ContentTop = null;
                }
                if (inputRight.message !=null && inputRight.start > start) {
                    hooks.ContentRight = null;
                }
                if (inputLeft.start <= start) {
                    hooks.ContentLeft = inputLeft.message;
                    hooks.onLeft = inputLeft.onClick;
                }
                if (inputTop.start <= start) {
                    hooks.ContentTop = inputTop.message;
                    hooks.onTop = inputTop.onClick;
                }
                if (inputRight.start <= start) {
                    hooks.ContentRight = inputRight.message;
                    hooks.onRight = inputRight.onClick;
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