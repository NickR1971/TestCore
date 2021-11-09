using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
        myCamera.SetPosition(EMapDirection.northwest);
    }
    protected override void OnUpdate()
    {

        if (inputController.IsPressed(MyButton.Rstick)) myCamera.SetPosition(EMapDirection.center);
        inputController.GetRightStick(out float h, out float v);
        int d = 0;
        if (v > 0.1f) d = 3; // N
        if (v < -0.1f) d = 6; // S
        if (h > 0.1f) d += 1; // E
        if (h < -0.1f) d += 2; // W

        switch(d)
        {
            case 1: // E
                myCamera.SetPosition(EMapDirection.east);
                break;
            case 2: // W
                myCamera.SetPosition(EMapDirection.west);
                break;
            case 3: // N
                myCamera.SetPosition(EMapDirection.north);
                break;
            case 4: // NE
                myCamera.SetPosition(EMapDirection.northeast);
                break;
            case 5: // NW
                myCamera.SetPosition(EMapDirection.northwest);
                break;
            case 6: // S
                myCamera.SetPosition(EMapDirection.south);
                break;
            case 7: // SE
                myCamera.SetPosition(EMapDirection.southeast);
                break;
            case 8: // SW
                myCamera.SetPosition(EMapDirection.southwest);
                break;
        }
    }
}
