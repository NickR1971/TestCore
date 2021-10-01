using System;

public interface IGame : IService
{
    void CreateGame(SaveData _data);
    SaveData GetData();
    void OnSave();
    void AddOnSaveAction(Action _a);
    void RemoveOnSaveAction(Action _a);
}
