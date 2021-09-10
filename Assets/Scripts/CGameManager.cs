using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CGameManager
{
	public static event Action onSave;
    private static SaveData gameData = null;

	public static void SetGameData(SaveData _data)
    {
        gameData = _data;
    }

    public static SaveData GetData() => gameData;

    public static void OnSave()
    {
        onSave?.Invoke();
    }

}
