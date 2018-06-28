using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(VT_Main))]
public class VT_MainEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        var dataStorage = PersistentDataStorage.Instance;
        GUILayout.Space(15);
        EditorGUILayout.LabelField("Data Storage", EditorStyles.largeLabel, GUILayout.MinHeight(20));
        GUI.skin.label.wordWrap = true;
        GUILayout.Label(dataStorage.GetState().ToString());
        if (GUILayout.Button("Reset Data Storage"))
        {
            dataStorage.ResetState();
            dataStorage.SaveState();
        }
    }
}