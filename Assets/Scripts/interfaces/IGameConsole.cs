using System;

public interface IGameConsole : IService
{
    void ShowMessage(string _msg);
    void AddCommand(CGameConsoleCommand _command);
    void Show();
    void Hide();
}