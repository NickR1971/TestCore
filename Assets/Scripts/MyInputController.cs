using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum EMyInputCode
{
    ButtonStart,    ButtonSelect,
    ButtonA, ButtonB, ButtonX, ButtonY,
    TriggerLeft, TriggerRight, BumperLeft, BumperRight,
    CrossLeft, CrossRight, CrossUp, CrossDown,
    LeftStick, RightStick
}

public interface IInputController : IService
{
    void SetActive(bool _isActive);
    bool IsPressed(EMyInputCode _code);
    bool GetButton(out EMyInputCode _code);
    void GetLeftStick(out float _h, out float _v);
    void GetRightStick(out float _h, out float _v);
    bool IsPressedEnter();
    bool IsPressedEscape();
}

public class MyInputController : MonoBehaviour, IInputController
{
    private bool isActive = false;
    private Gamepad gamepad;
    private Keyboard keyboard;

    private void Start()
    {
        keyboard = Keyboard.current;
        if (keyboard == null) Debug.Log("No keyboard found");
        //if (keyboard[Key.Enter].isPressed) ;

        gamepad = Gamepad.current;
        if (gamepad == null) Debug.Log("No gamepad found");
        else
        {
            Debug.Log(gamepad.name);
        }
    }

    private void Update()
    {
        if (isActive == false) return;
    }

    //-------------------------------------
    // IInputController
    public void SetActive(bool _isActive)
    {
        isActive = _isActive;
    }

    public bool IsPressed(EMyInputCode _code)
    {
        return false;
    }

    public bool GetButton(out EMyInputCode _code)
    {
        _code = EMyInputCode.ButtonStart;
        return false;
    }

    public void GetLeftStick(out float _h, out float _v)
    {
        _h = 0; _v = 0;
    }

    public void GetRightStick(out float _h, out float _v)
    {
        _h = 0; _v = 0;
    }

    public bool IsPressedEnter()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) return true;

        //if (keyboard[Key.Enter].isPressed) return true;
        //if (keyboard[Key.NumpadEnter].isPressed) return true;

        return false;
    }

    public bool IsPressedEscape()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) return true;

        //if (keyboard[Key.Escape].isPressed) return true;

        return false;
    }
}
