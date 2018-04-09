using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(VT_Main))]
public class VT_MainEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EditorStyles.label.fontStyle = FontStyle.Normal;

        var vt_main = target as VT_Main;
        serializedObject.Update();

        EditorGUILayout.LabelField("Dialog Options", EditorStyles.largeLabel, GUILayout.MinHeight(20));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("yarnDialogDatabase"), true);

        GUILayout.Space(15);
        EditorGUILayout.LabelField("Strategies Options", EditorStyles.largeLabel, GUILayout.MinHeight(20));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("intentions"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("forceIntention"), true);
        if (vt_main.forceIntention)
            EditorGUILayout.PropertyField(serializedObject.FindProperty("forceIntentionName"), true);

        GUILayout.Space(15); 
        EditorGUILayout.LabelField("Dialog Manager Options", EditorStyles.largeLabel, GUILayout.MinHeight(20));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("headAnimationManager"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("bubbleManager"), true);
        
        serializedObject.ApplyModifiedProperties();
    }
}