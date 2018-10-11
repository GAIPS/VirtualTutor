using System.Collections.Generic;
using UnityEngine;

namespace HookControl {
    public class ButtonListHook : Hook {
        [SerializeField]
        private GameObject buttonPrefab;
        [SerializeField]
        private GameObject listObject;

        public List<Control> buttons;

        public void AddButton(string text, VoidFunc onClick) {
            Control buttonControl = new Control(buttonPrefab);
            ShowResult result = buttonControl.Show();
            if (result == ShowResult.FIRST) {
                buttons.Add(buttonControl);
                if (listObject) {
                    buttonControl.Instance.transform.SetParent(listObject.transform);
                }

                // set button hook
                ButtonHook buttonHook = buttonControl.Instance.GetComponent<ButtonHook>();
                if (buttonHook != null) {
                    buttonHook.onClick = onClick;
                    buttonHook.text = text;
                } else {
                    Debug.LogWarning("No Button Hook found.");
                }
            }
        }
    }
}
