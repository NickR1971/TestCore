using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class ConsoleService : MonoBehaviour
{
    protected IGameConsole gameConsole;
    private List<string> cmdList;

    protected void Init()
    {
        gameConsole = AllServices.Container.Get<IGameConsole>();
        cmdList = new List<string>();
    }

    protected string RegisterCommandName(string _cmd)
    {
        cmdList.Add(_cmd);
        return _cmd;
    }

    protected void AddCommand(string _cmd, Action<string> _act, string _hint)
    {
        gameConsole.AddCommand(new CGameConsoleCommand(RegisterCommandName(_cmd), _act, _hint));
    }

    private void OnDestroy()
    {
        foreach (string cmd in cmdList)
        {
            gameConsole.RemoveCommand(cmd);
        }
    }
}
