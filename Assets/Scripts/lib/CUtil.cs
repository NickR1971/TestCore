using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class CUtil
{
    public static bool CheckNameForSave(string _name) => (_name.IndexOfAny(Path.GetInvalidFileNameChars()) < 0);

    public static string GetWord(string _str)
    {
        int i;

        for(i=0; i<_str.Length;i++)
        {
            if (_str[i] <= ' ') break;
        }
        return _str.Substring(0, i);
    }

    public static float StringToFloat(string _str)
    {
        return float.Parse(_str.Replace('.', ','));
    }
}
