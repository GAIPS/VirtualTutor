#if UNITY_ANDROID
using UnityEngine;
using System;

namespace DeadMosquito.AndroidGoodies.Internal
{
    class DialogOnDismissListenerProxy : AndroidJavaProxy
    {
        private readonly Action _onDismiss;

        public DialogOnDismissListenerProxy(Action onDismiss)
            : base("android.content.DialogInterface$OnDismissListener")
        {
            _onDismiss = onDismiss;
        }

        void onDismiss(AndroidJavaObject dialog)
        {
            GoodiesSceneHelper.Queue(_onDismiss);
        }
    }
}
#endif