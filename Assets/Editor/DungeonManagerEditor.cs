using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DungeonManager))]
public class DungeonManagerEditor : Editor {

    
    SerializedProperty tile;
    
    bool showTile = true;

    void OnEnable()
    {
       
        tile = serializedObject.FindProperty("tile");
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        showTile = EditorGUILayout.Foldout(showTile, "Tile");
        if (showTile)
        {
            
            EditorGUILayout.PropertyField(tile, new GUIContent(""));
            

            serializedObject.ApplyModifiedProperties();
        }

        DungeonManager myScript = (DungeonManager)target;
        if (GUILayout.Button("Generate Dungeon"))
        {
            myScript.GenerateDungeon();
        }
    }

}

