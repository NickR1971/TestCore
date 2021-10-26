using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public enum MyButton
{
    CrossUp=0, CrossDown=1, CrossLeft=3, CrossRight=4,
    Y=4, A=5, B=6, X=7,
    LStick=8, Rstick=9,
    LBumper=10, RBumper=11,
    LTrigger=12, RTrigger=13,
    Select=14, Start=15
}

public interface IInputController : IService
{
    bool IsPressed(MyButton _code);
    bool IsPressed(Key _key);
    void GetLeftStick(out float _horizontal, out float _vertical);
    void GetRightStick(out float _horizontal, out float _vertical);
    bool IsPressedEnter();
    bool IsPressedEscape();
}

public class MyInputController : MonoBehaviour, IInputController
{
    private Gamepad gamepad;
    private Keyboard keyboard;
    private readonly GamepadButton[] buttons = 
    {
        GamepadButton.DpadUp, GamepadButton.DpadDown, GamepadButton.DpadLeft, GamepadButton.DpadRight,
        GamepadButton.Y, GamepadButton.A, GamepadButton.B, GamepadButton.X,
        GamepadButton.LeftStick, GamepadButton.RightStick,
        GamepadButton.LeftShoulder, GamepadButton.RightShoulder,
        GamepadButton.LeftTrigger, GamepadButton.RightTrigger,
        GamepadButton.Select, GamepadButton.Start
    };

    private readonly Key[] keys =
    {
        Key.UpArrow, Key.DownArrow, Key.LeftArrow, Key.RightArrow,
        Key.Backquote, Key.Enter, Key.Escape, Key.Backslash,
        Key.LeftShift, Key.Numpad5,
        Key.Tab, Key.Quote,
        Key.Q, Key.E,
        Key.End, Key.Home
    };

    private void Start()
    {
        keyboard = Keyboard.current;
        if (keyboard == null) Debug.Log("No keyboard found");

        gamepad = Gamepad.current;
        if (gamepad == null) Debug.Log("No gamepad found");
        else
        {
            Debug.Log(gamepad.name);
        }
    }

    //-------------------------------------
    // IInputController

    public bool IsPressed(MyButton _code)
    {
        if (gamepad != null)
        {
            return gamepad[buttons[(int)_code]].wasPressedThisFrame;
        }
        if (keyboard != null)
        {
            return keyboard[keys[(int)_code]].wasPressedThisFrame;
        }
        return false;
    }
    public bool IsPressed(Key _key) => keyboard[_key].wasPressedThisFrame;

    public void GetLeftStick(out float _horizontal, out float _vertical)
    {
        if(gamepad==null)
        {
            _horizontal = 0;
            _vertical = 0;

            if (keyboard[Key.A].wasPressedThisFrame) _horizontal = -1;
            if (keyboard[Key.D].wasPressedThisFrame) _horizontal = 1;
            if (keyboard[Key.S].wasPressedThisFrame) _vertical = -1;
            if (keyboard[Key.W].wasPressedThisFrame) _vertical = 1;
            return;
        }

        Vector2 v;

        v = gamepad.leftStick.ReadValue();
        _horizontal = v.x;
        _vertical = v.y;
    }

    public void GetRightStick(out float _horizontal, out float _vertical)
    {
        if(gamepad==null)
        {
            _horizontal = 0;
            _vertical = 0;

            if (keyboard[Key.Numpad4].wasPressedThisFrame) _horizontal = -1;
            if (keyboard[Key.Numpad6].wasPressedThisFrame) _horizontal = 1;
            if (keyboard[Key.Numpad2].wasPressedThisFrame) _vertical = -1;
            if (keyboard[Key.Numpad8].wasPressedThisFrame) _vertical = 1;
            return;
        }

        Vector2 v;

        v = gamepad.rightStick.ReadValue();
        _horizontal = v.x;
        _vertical = v.y;
    }

    public bool IsPressedEnter()
    {
        //if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) return true;

        if (keyboard[Key.Enter].wasPressedThisFrame) return true;
        if (keyboard[Key.NumpadEnter].wasPressedThisFrame) return true;

        return false;
    }

    public bool IsPressedEscape()
    {
        //if (Input.GetKeyDown(KeyCode.Escape)) return true;

        if (keyboard[Key.Escape].wasPressedThisFrame) return true;

        return false;
    }
}
