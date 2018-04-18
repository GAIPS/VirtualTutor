using HookControl;
using UnityEngine;

namespace VT {
    public class SplashScreenControl : IControl {
        private Control control;
        private VoidFunc onEndSplashScreen;

        public SplashScreenControl(GameObject prefab) {
            control = new Control();
            control.prefab = prefab;
        }

        public void Set(VoidFunc onEndSplashScreen) {
            this.onEndSplashScreen = onEndSplashScreen;
        }

        public ShowResult Show() {
            var ret = control.Show();
            if (ret == ShowResult.FIRST || ret == ShowResult.OK) {
                var hooks = control.instance.GetComponent<SplashScreenHooks>();
                if (hooks) {
                    hooks.OnAnimationEnd = onEndSplashScreen;
                }
            }
            return ret;
        }

        public ShowResult SetAndShow(VoidFunc onEndSplashScreen) {
            this.Set(onEndSplashScreen);
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
    }
}

