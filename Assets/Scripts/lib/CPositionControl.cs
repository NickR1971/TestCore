using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPositionControl
{
    private Transform transform;
    private CMove move;
    private CTimer rotationTimer;
    private CTimer waitTimer;
    private float direction = 0;
    private float moveSpeed = 1.0f;
    private float rotationTime = 0.20f; // скільки секунд потрібно для повороту на 90 градусів
    private float startDirection;
    private float rotationAngle;

    public CPositionControl(Transform _transform)
    {
        transform = _transform;
        move = new CMove();
        rotationTimer = new CTimer();
        waitTimer = new CTimer();
    }

    public void MoveTo(Vector3 _target, float _speed = 0)
    {
        if (move.IsActive()) return;

        if (_speed == 0) _speed = moveSpeed;
        move.SetPositions(transform.position, _target);
        move.SetActionSpeed(moveSpeed = _speed);
        move.StartAction();
    }

    public void MoveForward(float _speed= 0)
    {
        MoveTo(transform.position + transform.forward, _speed);
    }

    public bool IsMoving() => move.IsActive();
    public bool IsRotating() => rotationTimer.IsActive();
    public bool IsWaiting() => waitTimer.IsActive();

    public void Wait(float _time)
    {
        if (IsWaiting()) return;

        waitTimer.SetActionTime(_time);
        waitTimer.StartAction();
    }

    public void SetDirection(float _angle)
    {
        while (_angle >= 360) _angle = _angle - 360;
        while (_angle < 0) _angle = _angle + 360;
        transform.rotation = Quaternion.Euler(0, direction = _angle, 0);
    }

    public void Rotate(float _angle)
    {
        if (_angle==0 || rotationTimer.IsActive()) return;

        startDirection = direction;
        rotationAngle = _angle;
        if (_angle < 0) _angle = -_angle;
        rotationTimer.SetActionTime(rotationTime * _angle / 90.0f);
        rotationTimer.StartAction();
    }

    public void Update()
    {
        if (move.IsActive())
        {
            move.UpdatePosition();
            transform.position = move.GetCurrentPosition();
        }
        if (rotationTimer.IsActive())
        {
            rotationTimer.UpdateState();
            SetDirection(startDirection + rotationAngle * rotationTimer.GetState());
        }
        if(waitTimer.IsActive())
        {
            waitTimer.UpdateState();
        }
    }

    public void SetRotationTime(float _t)
    {
        if (_t > 0) rotationTime = _t;
    }

    public float GetDirection() => direction;

    public bool IsBusy() => (IsMoving() || IsRotating() || IsWaiting());
}
