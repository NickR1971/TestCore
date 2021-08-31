using System;
using UnityEditor;
using UnityEngine;

[CustomEditor((typeof(CTextLocalize)))]
public class CTextLocalizeEditor : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Select String ID"))
            ShowSelectMenu();
    }

    private void ShowSelectMenu()
    {
        var menu = new GenericMenu();
        var allStringIDs = Enum.GetValues(typeof(ELocalStringID));

        foreach (var strID in allStringIDs)
            menu.AddItem(new GUIContent(strID.ToString().Replace("_","/")), false, ChangeValue, strID);

        menu.ShowAsContext();
        GUI.FocusControl(null);
    }

    private void ChangeValue(object key)
    {
        ((CTextLocalize)target).strID = key.ToString();
        EditorUtility.SetDirty(target);
    }
}