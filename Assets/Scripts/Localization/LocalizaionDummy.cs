using System;
using UnityEngine;

public class LocalizaionDummy : MonoBehaviour, ILocalization
{
    [SerializeField] private bool isActive;
    private void Awake()
    {
        if (isActive)
            AllServices.Container.Register<ILocalization>(this);
    }
    
    //-----------------------------------------------------
    // ILocalization
    public string GetString(ELocalStringID _id)
    {
        return _id.ToString();
    }

    public string GetString(string _key)
    {
        return _key;
    }

    public void LoadLanguage(UsedLocal _language)
    {
        Debug.Log($"Set language {_language}");
    }

    public void SetOnChange(Action _action)
    {
    }

    public void RemoveOnChange(Action _action)
    {
    }
}
