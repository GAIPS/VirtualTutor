using UnityEngine;
using UnityEngine.UI;

namespace HookControl {
    public class ButtonHook : Hook {

        [SerializeField]
        private Text textObject;

        public string text {
            set {
                if (textObject) {
                    textObject.text = value;
                } else {
                    Debug.LogWarning("Text could not be changed.", this);
                }
            }
        }

        public VoidFunc onClick;

        public void UIOnClick() {
            if (onClick == null)
                return;
            onClick();
        }
    }
}
