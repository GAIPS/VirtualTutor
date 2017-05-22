#if UNITY_ANDROID
using UnityEngine;
using System;

namespace DeadMosquito.AndroidGoodies.Internal
{
    class DialogOnMultiChoiceClickListenerProxy : AndroidJavaProxy
    {
        private readonly Action<int, bool> _onClick;

        public DialogOnMultiChoiceClickListenerProxy(Action<int, bool> onClick)
            : base("android.content.DialogInterface$OnMultiChoiceClickListener")
        {
            _onClick = onClick;
        }

        void onClick(AndroidJavaObject dialog, int which, bool isChecked)
        {
            GoodiesSceneHelper.Queue(() => _onClick(which, isChecked));
        }
    }
}
#endif
