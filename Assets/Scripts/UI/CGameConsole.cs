using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CGameConsoleCommand
{
    public string command;
    public string strHint;
    public Action<string> action;


    public CGameConsoleCommand()
    {
        command = "none";
        strHint = "";
        action = null;
    }
    public CGameConsoleCommand(string _cmd, Action<string> _act=null, ELocalStringID _hint=ELocalStringID.core_empty)
    {
        ILocalization localization = AllServices.Container.Get<ILocalization>();
        command = _cmd;
        strHint = localization.GetString(_hint);
        action = _act;
    }

    public CGameConsoleCommand(string _cmd, Action<string> _act, string _hint)
    {
        command = _cmd;
        strHint = _hint;
        action = _act;
    }
}

public class CGameConsole : MonoBehaviour, IGameConsole
{
    [SerializeField] private InputField inputText;
    [SerializeField] private Transform containerTransform;
    [SerializeField] private GameObject consoleString;
    [SerializeField] private Scrollbar scroll;
    private ILocalization localization;
    private const int maxMsgList = 50;
    private int currentMsg = 0;
    private GameObject[] msgList = new GameObject[maxMsgList];
    private SortedList<string, CGameConsoleCommand> commandsList = new SortedList<string, CGameConsoleCommand>();
    private IMainMenu mainMenu;
    private IInputController inputController;

    private void Start()
    {
        localization = AllServices.Container.Get<ILocalization>();

        for (int i = 0; i < maxMsgList; i++)
        {
            msgList[i] = Instantiate(consoleString, containerTransform);
        }
        AddCommand(new CGameConsoleCommand("help", Help));
        AddCommand(new CGameConsoleCommand("quit", Quit,ELocalStringID.core_quit));
        mainMenu = AllServices.Container.Get<IMainMenu>();
        inputController = AllServices.Container.Get<IInputController>();
    }

    private void OnDestroy()
    {
        commandsList.Clear();
    }

    private void AddMsg(GameObject _msg)
    {
        if (currentMsg == maxMsgList) currentMsg = 0;
        Destroy(msgList[currentMsg]);
        msgList[currentMsg++] = _msg;
    }

    public IGameConsole GetIGameConsole()
    {
        return this;
    }

    public void AddCommand(CGameConsoleCommand _command)
    {
        if (commandsList.ContainsKey(_command.command))
        {
            commandsList.Remove(_command.command);
            Debug.Log($"Command {_command.command} replaced");
        }
        commandsList.Add(_command.command, _command);
    }

    public void RemoveCommand(string _cmdName)
    {
        if (commandsList.ContainsKey(_cmdName))
        {
            commandsList.Remove(_cmdName);
        }
    }

    public void ShowMessage(string _message)
    {
        GameObject newString;
        newString = Instantiate(consoleString, containerTransform);
        newString.GetComponent<Text>().text = _message;
        AddMsg(newString);
        scroll.value = 0.0f;
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void Quit(string _str)
    {
            mainMenu.Quit();
    }

    private void Help(string _str)
    {
        foreach(var cmd in commandsList)
        {
            ShowMessage($"{ cmd.Key} {cmd.Value.strHint}");
        }
    }

    private void DoCommand(string _cmd)
    {
        CGameConsoleCommand gcCommand;

        if (commandsList.TryGetValue(CUtil.GetWord(_cmd), out gcCommand))
        {
            gcCommand.action?.Invoke(_cmd.Substring(gcCommand.command.Length).Trim());
        }
        else ShowMessage(localization.GetString(ELocalStringID.err_noCommnand) + " [" + _cmd + "]");
    }

    public void OnTextEnter()
    {
        string sText = inputText.text;

        if (inputController.IsPressedEnter())
        {
            if (sText.Trim().Length > 0)
            {
                DoCommand(sText.Trim());
                inputText.text = "";
            }
        }
    }

    public void OnButton()
    {
        Hide();
    }
}
