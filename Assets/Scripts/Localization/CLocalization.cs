using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UsedLocal { english = 0, ukrainian = 1 }

public static class CLocalization
{
    public static event Action reloadText;

    private static SortedList<string, string> localStrings;

    public static bool Init()
    {
        reloadText = null;
        if (localStrings != null) return false;
        
        localStrings = new SortedList<string, string>();
        return true;
    }

    public static void LoadLocalPrefab(GameObject _localPrefab)
    {
        CLocal local;
        GameObject loc = MonoBehaviour.Instantiate(_localPrefab);
        localStrings.Clear();
        local = loc.GetComponent<CLocal>();
        local.Init(localStrings);
        MonoBehaviour.Destroy(loc);
        reloadText?.Invoke();
    }

    public static string GetString(string _key)
    {
        if (localStrings.TryGetValue(_key, out string value))
            return value;
            
        return $"<<empty key[{_key}]>>";
    }
    public static string GetString(ELocalStringID _id)
    {
        return GetString(_id.ToString());
    }
}
