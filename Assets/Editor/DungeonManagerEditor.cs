using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DungeonManager))]
public class DungeonManagerEditor : Editor {
    SerializedProperty tlTile;
    SerializedProperty tmTile;
    SerializedProperty trTile;
    SerializedProperty mlTile;
    SerializedProperty mmTile;
    SerializedProperty mrTile;
    SerializedProperty blTile;
    SerializedProperty bmTile;
    SerializedProperty brTile;
    bool showTiles = true;

    void OnEnable()
    {
        tlTile = serializedObject.FindProperty("tlTile");
        tmTile = serializedObject.FindProperty("tmTile");
        trTile = serializedObject.FindProperty("trTile");
        mlTile = serializedObject.FindProperty("mlTile");
        mmTile = serializedObject.FindProperty("mmTile");
        mrTile = serializedObject.FindProperty("mrTile");
        blTile = serializedObject.FindProperty("blTile");
        bmTile = serializedObject.FindProperty("bmTile");
        brTile = serializedObject.FindProperty("brTile");
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        showTiles = EditorGUILayout.Foldout(showTiles, "Tiles");
        if (showTiles)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Top", GUILayout.Width(20));
            EditorGUILayout.PropertyField(tlTile, new GUIContent(""));
            EditorGUILayout.PropertyField(tmTile, new GUIContent(""));
            EditorGUILayout.PropertyField(trTile, new GUIContent(""));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Middle", GUILayout.Width(20));
            EditorGUILayout.PropertyField(mlTile, new GUIContent(""));
            EditorGUILayout.PropertyField(mmTile, new GUIContent(""));
            EditorGUILayout.PropertyField(mrTile, new GUIContent(""));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Bottom", GUILayout.Width(20));
            EditorGUILayout.PropertyField(blTile, new GUIContent(""));
            EditorGUILayout.PropertyField(bmTile, new GUIContent(""));
            EditorGUILayout.PropertyField(brTile, new GUIContent(""));
            EditorGUILayout.EndHorizontal();

            serializedObject.ApplyModifiedProperties();
        }

        DungeonManager myScript = (DungeonManager)target;
        if (GUILayout.Button("Generate Dungeon"))
        {
            myScript.GenerateDungeon();
        }
    }

}

