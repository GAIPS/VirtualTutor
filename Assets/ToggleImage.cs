using HookControl;
using UnityEngine;
using UnityEngine.UI;

namespace VT {
    [RequireComponent(typeof(Button))]
    public class ToggleImage : MonoBehaviour {

        Button button = null;

        public bool isOn = false;
        private Sprite previousSprite = null;
        public Sprite toggleSprite = null;

        public BoolFunc onClick = null;

        // Use this for initialization
        void Start() {
            button = GetComponent<Button>();
            if (button) {
                previousSprite = button.image.sprite;
                button.onClick.AddListener(() => {
                        isOn = !isOn;
                        toggle(isOn);
                        if (onClick != null) {
                            onClick(isOn);
                        }
                    });
                toggle(isOn);
            }
        }

        private void toggle(bool isOn) {
            button.image.sprite = isOn ? toggleSprite : previousSprite;
        }
    }
}