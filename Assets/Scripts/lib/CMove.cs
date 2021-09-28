using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/********************************************
 * class CMove
 * алгоритм для розрахунку плавного руху
 * між двома позиціями
 * розрахунок спирається на встановлений час
 * руху від стартової позиції до кінцевої
 ********************************************/

public class CMove
{
    private Vector3 targetPosition;
    private Vector3 startPosition;
    private Vector3 currentPosition;
    private float actionSpeed;
    private CTimer actionTimer;

    //---------------------------------------------------------------------------
    // конструктор встановлює час дії на 1 секунду за замовчуванням 
    public CMove()
    {
        actionSpeed = 0;
        actionTimer = new CTimer();
        startPosition = new Vector3(0, 0, 0);
        targetPosition = startPosition;
    }

    //---------------------------------------------------------------------------
    // calcActionTime
    // розрахунок часу дії відповідно до швидкості та відстані між позиціями
    // швидкість нуль або менше ігнорується
    private void CalcActionTime()
    {
        if (actionSpeed > 0)
        {
            actionTimer.SetActionTime(Vector3.Distance(startPosition, targetPosition) / actionSpeed);
        }
    }

    //---------------------------------------------------------------------------
    // SetPositions
    // встановлюємо маршрут руху
    // та перераховуємо час дії якщо встановлена швидкість
    public void SetPositions(Vector3 _start, Vector3 _target)
    {
        startPosition = _start;
        targetPosition = _target;
        CalcActionTime();
    }
    //---------------------------------------------------------------------------
    // SetActionTime
    public void SetActionTime(float _actionTime)
    {
        actionTimer.SetActionTime(_actionTime);
    }

    //---------------------------------------------------------------------------
    // SetActionSpeed
    // істановлюємо час дії згідно заданої швидкості
    public void SetActionSpeed(float _actionSpeed)
    {
        actionSpeed = _actionSpeed;
        CalcActionTime();
    }

    //---------------------------------------------------------------------------
    // StartAction
    public void StartAction()
    {
        actionTimer.StartAction();
        currentPosition = startPosition;
    }

    //---------------------------------------------------------------------------
    // isActive
    // перевірка чи активна дія на цей час
    public bool IsActive() { return actionTimer.IsActive(); }

    //---------------------------------------------------------------------------
    // GetCurrentPosition
    // отримуємо поточну позицію
    public Vector3 GetCurrentPosition()
    {
        return currentPosition;
    }

    //---------------------------------------------------------------------------
    // UpdatePosition
    // розраховує поточну позицію та повертає false коли рух завершено
    public bool UpdatePosition()
    {
        if (actionTimer.UpdateState())
            currentPosition = Vector3.Lerp(startPosition, targetPosition, actionTimer.GetState());
        else
            currentPosition = targetPosition;

     return IsActive();
    }

    //---------------------------------------------------------------------------
    // CorrectTargetPosition
    // Корекція координат призначення використовується якщо ціль рухається
    public void CorrectTargetPosition(Vector3 _target)
    {
        targetPosition = _target;
    }

}
