using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CMainMenu : CMenu
{
    private Button loadButton = null;
    private ISaveLoad iSaveLoad;
    private int sceneIndex;

    void Start()
    {
        InitMenu();
        iSaveLoad = AllServices.Container.Get<ISaveLoad>();
        sceneIndex = SceneManager.GetActiveScene().buildIndex;

        if (sceneIndex == 0) AddButton(ELocalStringID.core_newGame).onClick.AddListener(NewGame);
        if (iMainMenu.IsGameExist())
        {
            AddButton(ELocalStringID.core_continueGame).onClick.AddListener(ContinueGame);
            AddButton(ELocalStringID.core_saveGame).onClick.AddListener(SaveGame);
        }
        AddButton(ELocalStringID.core_loadGame).onClick.AddListener(LoadGame);
        loadButton = LastButton();
        loadButton.interactable = iSaveLoad.IsSavedGameExist();
        AddButton(ELocalStringID.core_settings).onClick.AddListener(SetSettings);
        if (sceneIndex != 0) AddButton(ELocalStringID.core_mainMenu).onClick.AddListener(GoMainMenu);

        AddButton(ELocalStringID.core_quit).onClick.AddListener(QuitGame);
    }

    public override void OnOpen()
    {
        base.OnOpen();
        iSaveLoad = AllServices.Container.Get<ISaveLoad>();
        if (loadButton != null)
        {
            loadButton.interactable = iSaveLoad.IsSavedGameExist();
        }
    }

    public void QuitGame()
    {
        iMainMenu.Quit();
    }

    public void NewGame()
    {
        iMainMenu.NewGame();
    }

    public void ContinueGame()
    {
        if (sceneIndex == 0) iMainMenu.GoToMainScene();
        else iMainMenu.OpenStartUI();
    }

    public void SaveGame()
    {
        iMainMenu.Save();
    }

    public void LoadGame()
    {
        iMainMenu.Load();
    }

    public void GoMainMenu()
    {
        iMainMenu.MainMenuScene();
    }

    public void SetSettings()
    {
        iMainMenu.OpenSettings();
    }

    public override void OnCancel()
    {
        // disable close window on ESC by default
    }

}

