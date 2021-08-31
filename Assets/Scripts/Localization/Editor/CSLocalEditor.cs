using System;
using UnityEditor;
using UnityEngine;

[CustomEditor((typeof(CSLocal)))]
public class CSLocalEditor : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Generate enum file.cs"))
        {
            CSLocal local;
            local = (CSLocal)target;
            local.GenerateScript();
        }           
    }
}
