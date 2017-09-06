using HookControl;
using UnityEngine;

namespace VT {
    public class SplashScreenHooks : MonoBehaviour {

        [SerializeField]
        private Animator splashScreenAnimator;

        public VoidFunc OnAnimationEnd { get; set; }

        public void onAnimationEndFunc() {
            if (OnAnimationEnd != null) {
                OnAnimationEnd();
            }
        }
    }
}