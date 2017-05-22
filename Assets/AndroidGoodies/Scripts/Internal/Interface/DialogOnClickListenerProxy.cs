#if UNITY_ANDROID
using UnityEngine;
using System;

namespace DeadMosquito.AndroidGoodies.Internal
{
    class DialogOnClickListenerProxy : AndroidJavaProxy
    {
        private const string InterfaceSignature = "android.content.DialogInterface$OnClickListener";

        private readonly Action _onClickVoid;
        private readonly Action<int> _onClickInt;
        private readonly bool _dismissOnClick;

        public DialogOnClickListenerProxy(Action onClick)
            : base(InterfaceSignature)
        {
            _onClickVoid = onClick;
        }

        public DialogOnClickListenerProxy(Action<int> onClick, bool dismissOnClick = false)
            : base(InterfaceSignature)
        {
            _onClickInt = onClick;
            _dismissOnClick = dismissOnClick;
        }

        public void onClick(AndroidJavaObject dialog, int which)
        {
            if (_onClickVoid != null)
            {
                GoodiesSceneHelper.Queue(_onClickVoid);
            }
            if (_onClickInt != null)
            {
                GoodiesSceneHelper.Queue(() => _onClickInt(which));
            }
            if (_dismissOnClick)
            {
                GoodiesSceneHelper.Queue(() => dialog.Call("dismiss"));
            }
        }
    }
}
#endif