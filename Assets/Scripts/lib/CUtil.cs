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

    public static int StringToInt(string _str)
    {
        return int.Parse(GetWord(_str));
    }

    public static bool IsDigit(char _c)
    {
        switch (_c)
        {
            case '0':
            case '1':
            case '2':
            case '3':
            case '4':
            case '5':
            case '6':
            case '7':
            case '8':
            case '9':
                return true;
        }

        return false;
    }

    public static void LogConsole(string _msg)
    {
        AllServices.Container.Get<IGameConsole>().ShowMessage(_msg);
    }
}
