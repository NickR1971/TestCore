using System;
using System.Linq;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable] class CLocalisationData
{
    public string key;
    public string value;
};
[Serializable] class CTest
{
    public CLocalisationData[] loc;
}

public class CLocal : MonoBehaviour
{
    [SerializeField] private TextAsset textUI;
    private CTest localUI;

    public void Init(SortedList<string, string> localStr)
    {
        if (textUI == null)
        {
            Debug.Log("Localisation file not found!");
            return;
        }
        localUI = JsonUtility.FromJson<CTest>(textUI.text);
        foreach (CLocalisationData ldata in localUI.loc)
        {
            localStr.Add(ldata.key, ldata.value);
        }
    }
}
