﻿using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Preview))]
public class PreviewEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EditorStyles.label.fontStyle = FontStyle.Normal;

//        var preview = target as Preview;
        serializedObject.Update();

        EditorGUILayout.LabelField("UI Options", EditorStyles.largeLabel, GUILayout.MinHeight(20));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_stringInput"), true);

        GUILayout.Space(15);
        EditorGUILayout.LabelField("Dialog Options", EditorStyles.largeLabel, GUILayout.MinHeight(20));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("YarnDialogDatabase"), true);

        GUILayout.Space(15);
        EditorGUILayout.LabelField("Strategies Options", EditorStyles.largeLabel, GUILayout.MinHeight(20));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("IntentionName"), true);

        GUILayout.Space(15);
        EditorGUILayout.LabelField("Dialog Manager Options", EditorStyles.largeLabel, GUILayout.MinHeight(20));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("HeadAnimationManager"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("BubbleManager"), true);

        serializedObject.ApplyModifiedProperties();
    }
}