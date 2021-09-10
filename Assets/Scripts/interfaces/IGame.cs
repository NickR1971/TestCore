using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGame : IService
{
    SaveData GetData();
    void OnSave();
    void AddOnSaveAction(Action _a);
    void RemoveOnSaveAction(Action _a);
}
