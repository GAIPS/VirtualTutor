using System.Collections;
using System.Collections.Generic;
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