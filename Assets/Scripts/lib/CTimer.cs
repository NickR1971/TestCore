using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*********************************************
 * class CTimer
 * встановлює таймер для відліку інтервалів
 * та синхронізації
 * незалежно від частоти оновлення кадрів
 * стан таймеру може мати значення від
 * 0.0f - стартова позиція до
 * 1.0f - коли час вичерпано
 * стан змінюється рівномірно та пропорційно
 * вичерпаному часу
 * *******************************************/

public class CTimer
{
    private float startTime = 0;
    private float actionTime = 1;
    private float currentState = 0;
    private bool isAction = false;
    
    public void StartAction()
    {
        startTime = Time.time;
        currentState = 0;
        isAction = true;
    }

    public bool UpdateState()
    {
     float t;

        if (!isAction) return false;
        
        t = Time.time - startTime;
        if (t >= actionTime)
        {
            currentState = 1;
            isAction = false;
        }
        else currentState = t / actionTime;

     return isAction;
    }

    public void SetActionTime(float _t)
    {
        actionTime = _t;
    }

    public float GetState()
    {
        return currentState;
    }

    public void ResetState()
    {
        currentState = 0;
    }

    public bool IsActive()
    {
        return isAction;
    }
}
