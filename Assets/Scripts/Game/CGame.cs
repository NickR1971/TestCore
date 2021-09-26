using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGame : MonoBehaviour, IGame
{
    private IDialog dialog;
    private IGameConsole gameConsole;

    private void Start()
    {
        dialog = AllServices.Container.Get<IDialog>();
        gameConsole = AllServices.Container.Get<IGameConsole>();
    }

    //--------------------------------------------------------------
    // IGame interface
    //--------------------------------------------------------------
    public SaveData GetData()
    {
        return CGameManager.GetData();
    }

    public void OnSave()
    {
        CGameManager.OnSave();
    }

    public void AddOnSaveAction(Action _a)
    {
        CGameManager.onSave += _a;
    }

    public void RemoveOnSaveAction(Action _a)
    {
        CGameManager.onSave -= _a;
    }

}
