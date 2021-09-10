using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CMainMenu : CMenu
{
    private Button loadButton = null;
    ISaveLoad iSaveLoad;
    void Start()
    {
        InitMenu();
        iSaveLoad = AllServices.Container.Get<ISaveLoad>();
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;

        if (sceneIndex == 0) AddButton(ELocalStringID.core_newGame).onClick.AddListener(NewGame);
        if (iMainMenu.IsGameExist())
            AddButton(ELocalStringID.core_continueGame).onClick.AddListener(ContinueGame);
        AddButton(ELocalStringID.core_loadGame).onClick.AddListener(LoadGame);
        loadButton = LastButton();
        loadButton.interactable = iSaveLoad.IsSavedGameExist();
        AddButton(ELocalStringID.core_settings).onClick.AddListener(SetSettings);
        if (sceneIndex != 0) AddButton(ELocalStringID.core_mainMenu).onClick.AddListener(GoMainMenu);

        AddButton(ELocalStringID.core_quit).onClick.AddListener(QuitGame);
    }
    private void OnEnable()
    {
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
        iMainMenu.GoToMainScene();
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

