using UnityEngine;
using System.Collections;
using UnityEditor;

#if UNITY_EDITOR
[CustomEditor(typeof(GameSaver))]
public class GameSaverEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        GameSaver saver = (GameSaver)target;
        if (GUILayout.Button("DeleteSaves"))
        {
            saver.DeleteAll();
        }
        
    }
}
#endif