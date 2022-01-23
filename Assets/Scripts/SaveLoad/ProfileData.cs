using System;
using System.Collections;
using System.Collections.Generic;

public class SearchString
{
    private string str;

    public SearchString(string _str)
    {
        str = _str;
    }

    public bool Check(string _str)
    {
        if (str == null) return false;
        if (_str == null) return false;
        return str == _str;
    }
}

[Serializable]
public class ProfileData
{
    public string name;
    public int savedGamesNumber;
    public List<string> savedList;
    public List<string> commentList;

    public ProfileData()
    {
        savedGamesNumber = 0;
        savedList = new List<string>();
        commentList = new List<string>();
    }

    public bool IsSaveExist() => savedList.Count > 0;

    public void AddSave(string _name, string _comment)
    {
        savedList.Insert(0, _name);
        if (_comment == null) _comment = "<Empty>";
        commentList.Insert(0, _comment);
    }

    public bool RemoveSave(string _name)
    {
        var cs = new SearchString(_name);
        int index = savedList.FindIndex(cs.Check);
        if (index < 0) return false;
        savedList.RemoveAt(index);
        commentList.RemoveAt(index);
        return true;
    }

    public string[] GetSavedList()
    {
        return savedList.ToArray();
    }
    public string[] GetCommentList()
    {
        return commentList.ToArray();
    }
}