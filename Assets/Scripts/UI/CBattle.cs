﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CBattle : CUI
{
    private IGame game;
    private IGameConsole gameConsole;
    private ICamera myCamera;

    private void Start()
    {
        InitUI();
        game = AllServices.Container.Get<IGame>();
        game.CreateGame(game.GetData());
        myCamera = AllServices.Container.Get<ICamera>();
        myCamera.SetPosition(new Vector3(0, 5, -7));
    }
}
