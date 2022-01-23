using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CGameManager
{
	public static event Action onSave;
    public const int versionMajor=0;
    public const int versionMinor=1;
    public const int versionPatch=0;
    private static SaveData gameData = null;

	public static void SetGameData(SaveData _data)
    {
        gameData = _data;
    }

    public static SaveData GetData() => gameData;

    public static void OnSave()
    {
        onSave?.Invoke();

        DateTime dt = DateTime.Now;
        if (gameData.comment == null) gameData.comment = "";
        gameData.comment += dt.ToString();
        gameData.versionMajor = versionMajor;
        gameData.versionMinor = versionMinor;
        gameData.versionPatch = versionPatch;
    }

}
