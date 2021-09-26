using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CBattle : CUI
{
    private IGame game;
    private IGameConsole gameConsole;
    private CRand rand;

    private void Start()
    {
        InitUI();
        game = AllServices.Container.Get<IGame>();
        rand = new CRand(game.GetData().id);
    }
}
