using UnityEngine;

namespace HookControl {
    public enum ShowResult
    {
        FIRST,
        OK,
        FAIL
    }

    public interface IControl
    {
        string GetName();
        
        GameObject Instance { get; }
        
        ShowResult Show ();

        void Destroy ();

        void Disable ();

        bool IsVisible ();

    }
}
