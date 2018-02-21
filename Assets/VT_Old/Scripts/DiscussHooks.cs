using HookControl;
using UnityEngine;


namespace VT {
    public class DiscussHooks : Hook
    {
        [SerializeField]
        private GameObject confirm;

        public StringFunc onMessageInput;
        public VoidFunc onConfirm;

        public void UIConfirm ()
        {
            if (onConfirm == null)
                return;
                    
            onConfirm ();
        }

        public void UIInput (string value)
        {
            onMessageInput (value);
        }
    

    }
}
