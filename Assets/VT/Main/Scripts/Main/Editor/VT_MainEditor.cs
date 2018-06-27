using UnityEngine; 
using UnityEditor; 
 
[CustomEditor(typeof(VT_Main))] 
public class VT_MainEditor : Editor 
{ 
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector ();
        
        if(GUILayout.Button("Reset Data Storage"))
        {
            PersistentDataStorage.Instance.ResetState();
            PersistentDataStorage.Instance.SaveState();
        }
    } 
}