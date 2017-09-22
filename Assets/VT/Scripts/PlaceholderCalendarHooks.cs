using HookControl;
using UnityEngine;
using UnityEngine.UI;

namespace VT {
    public class PlaceholderCalendarHooks : Hook {

        [SerializeField]
        private Sprite sprite1;
        [SerializeField]
        private Sprite sprite2;
        [SerializeField]
        private Sprite sprite3;

        [SerializeField]
        private Image image = null;

        private void Start() {
            if (image) {
                image.sprite = sprite1;
            }
        }

        public VoidFunc onEnd;

        public void UIClickCalendar() {
            if (image) {
                if (image.sprite == sprite1) {
                    image.sprite = sprite2;
                } else if (image.sprite == sprite2) {
                    image.sprite = sprite3;
                } else {
                    if (onEnd != null) {
                        onEnd();
                    }
                }
            } else {
                if (onEnd != null) {
                    onEnd();
                }
            }
        }
    }
}
