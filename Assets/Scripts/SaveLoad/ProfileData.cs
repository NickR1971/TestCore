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
        savedList.Add("new save");
    }

    public bool IsSaveExist() => savedList.Count > 1;

    public void AddSave(string _name)
    {
        savedList.Insert(1, _name);
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