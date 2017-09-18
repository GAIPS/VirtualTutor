using HookControl;
using UnityEngine;

namespace VT {
    public class Calendar1Control :IControl
    {

        private Control control;
        private VoidFunc click1;

        public Calendar1Control(GameObject prefab){
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
                var hooks = control.instance.GetComponent<Calendar1Hooks> ();
                if (hooks != null) {
                    hooks.click1 = this.click1;
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
