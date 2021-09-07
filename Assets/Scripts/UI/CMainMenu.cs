using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CMainMenu : CMenu
{
    private Button loadButton = null;
    ISaveLoad iSaveLoad;
    void Start()
    {
        InitMenu();
        iSaveLoad = AllServices.Container.Get<ISaveLoad>();

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

}

