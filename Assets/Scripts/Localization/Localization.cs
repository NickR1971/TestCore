using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Localization : MonoBehaviour, ILocalization
{
    [SerializeField] private GameObject[] localData = new GameObject[2];
    [SerializeField] private bool isActive;
    private void Awake()
    {
        if (isActive)
            AllServices.Container.Register<ILocalization>(this);
        if (CLocalization.Init())
			CLocalization.LoadLocalPrefab(localData[0]);

    }

    //-----------------------------------------------------
    // ILocalization
    public string GetString(ELocalStringID _id)
    {
        return CLocalization.GetString(_id);
    }

    public string GetString(string _key)
    {
        return CLocalization.GetString(_key);
    }

    public void LoadLanguage(UsedLocal _language)
    {
        CLocalization.LoadLocalPrefab(localData[(int)_language]);
    }

    public void SetOnChange(Action _action)
    {
        CLocalization.reloadText += _action;
    }

    public void RemoveOnChange(Action _action)
    {
        CLocalization.reloadText -= _action;
    }
}
