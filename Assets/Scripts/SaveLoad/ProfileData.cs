using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class ProfileData
{
    public string name;
    public int savedGamesNumber;
    public List<string> savedList;

    public ProfileData()
    {
        savedGamesNumber = 0;
        savedList = new List<string>();
    }

    public bool IsSaveExist() => savedList.Count > 0;

    public void AddSave(string _name)
    {
        savedList.Insert(0, _name);
    }

    public bool RemoveSave(string _name)
    {
        return savedList.Remove(_name);
    }

    public string[] GetSavedList()
    {
        return savedList.ToArray();
    }
}