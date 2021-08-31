using System;
using System.Linq;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new local", menuName = "Scriptable/Local")]
public class CSLocal : ScriptableObject
{
    [SerializeField] private TextAsset[] textLocal=new TextAsset[2];
    [SerializeField] private string enumName;

    public void GenerateScript()
    {
        CTest localUI;
        TextAsset text_ui = textLocal[0];
        string WriteToFileName = $"{Application.dataPath}/Scripts/Localization/{enumName}.cs";
        localUI = JsonUtility.FromJson<CTest>(text_ui.text);
        if (localUI == null)
        {
            Debug.LogError("localUI is null!");
            return;
        }

        var constants = localUI.loc.Select(item => item.key);

        var content = $"public enum {enumName} \n{{ \n" +
            string.Join(",\n", constants) +
            $" \n}}";

        File.WriteAllText(WriteToFileName, content);
        Debug.Log($"File {WriteToFileName} is saved");
    }
}
