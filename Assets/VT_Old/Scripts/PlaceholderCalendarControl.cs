using HookControl;
using UnityEngine;

namespace VT {
    public class PlaceholderCalendarControl :IControl
    {

        private Control control;
        private VoidFunc click1;

        public PlaceholderCalendarControl(GameObject prefab){
            control = new Control ();
            control.prefab = prefab;
        }

        public void Set (VoidFunc click1)
        {
            this.click1 = click1;
        }
        public ShowResult Show ()
        {
            var ret = control.Show ();
            if (ret == ShowResult.FIRST || ret == ShowResult.OK) {
                var hooks = control.instance.GetComponent<PlaceholderCalendarHooks> ();
                if (hooks != null) {
                    hooks.onEnd = this.click1;
                }
            }
            return ret;
        }

        public ShowResult SetAndShow (VoidFunc click1)
        {
            this.Set (click1);
            return Show ();
        }

        public void Disable ()
        {
            control.Disable ();
        }

        public void Destroy ()
        {
            control.Destroy ();
        }

        public bool IsVisible ()
        {
            return control.IsVisible ();
        }
        public void Enable(){
            control.Enable ();
        }
    }
}
