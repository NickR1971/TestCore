using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CBattle : CUI
{
    private IGame game;
    private IGameConsole gameConsole;

    private void Start()
    {
        InitUI();
        game = AllServices.Container.Get<IGame>();
        game.CreateGame(game.GetData());
    }
}
