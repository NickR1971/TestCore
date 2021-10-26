using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CBattle : CUI
{
    private IGame game;
    private IGameConsole gameConsole;
    private ICamera myCamera;
    private IInputController input;

    private void Start()
    {
        InitUI();
        game = AllServices.Container.Get<IGame>();
        game.CreateGame(game.GetData());
        myCamera = AllServices.Container.Get<ICamera>();
        //myCamera.SetPosition(new Vector3(-2, 5, -7));
        myCamera.SetPosition(EMapDirection.northwest);
        input = AllServices.Container.Get<IInputController>();
    }
    private void Update()
    {
        if (input.IsPressed(Key.Numpad8)) myCamera.SetPosition(EMapDirection.north);
        if (input.IsPressed(Key.Numpad9)) myCamera.SetPosition(EMapDirection.northeast);
        if (input.IsPressed(Key.Numpad6)) myCamera.SetPosition(EMapDirection.east);
        if (input.IsPressed(Key.Numpad3)) myCamera.SetPosition(EMapDirection.southeast);
        if (input.IsPressed(Key.Numpad2)) myCamera.SetPosition(EMapDirection.south);
        if (input.IsPressed(Key.Numpad1)) myCamera.SetPosition(EMapDirection.southwest);
        if (input.IsPressed(Key.Numpad4)) myCamera.SetPosition(EMapDirection.west);
        if (input.IsPressed(Key.Numpad7)) myCamera.SetPosition(EMapDirection.northwest);
        if (input.IsPressed(Key.Numpad5)) myCamera.SetPosition(EMapDirection.center);
    }
}
