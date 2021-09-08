using System;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ApplicationManager : MonoBehaviour, IMainMenu, ISaveLoad
{
	private static uint gameID;
	//private static CGameManager gameManager = null;

	[SerializeField] private int sceneID;
	private UsedLocal usedLanguage=UsedLocal.english;
	[SerializeField] Canvas uiCanvas;
	[SerializeField] private GameObject startUIobject;
	[SerializeField] private GameObject settingsMenu;
	[SerializeField] private CDialog dialog;
    [SerializeField] private CSaveLoad saveLoadWindow;
	[SerializeField] private GameObject prefabGameConsole;
	[SerializeField] private GameObject[] localData=new GameObject[2];
	private CUI startUI;
	private CSaveFile saveFile;
	private UImanager uiManager;
	private IGameConsole gameConsole;
	//private IGame game;


    private void Awake()
    {
		SettingsData settingsData;
/*
		game = GetComponent<IGame>();
		if (gameManager == null) gameManager = new CGameManager();
		game.Init(gameManager);
		SaveData data = game.GetData();
*/		
		AllServices.Container.Register<IDialog>(dialog);
		AllServices.Container.Register<IMainMenu>(this);
		AllServices.Container.Register<ISaveLoad>(this);
//		AllServices.Container.Register<IGame>(game);

		/*if (data == null)
		{
			Debug.LogError("Save data not created");
		}*/
		saveFile = new CSaveFile();
		saveFile.LoadSettings(out settingsData);
		if(settingsData==null)
        {
			settingsData = new SettingsData();
        }
		usedLanguage = settingsData.selected;
		SetProfile(settingsData.profileName);

		if (CLocalisation.Init())
			CLocalisation.LoadLocalPrefab(localData[(int)usedLanguage]);

		uiManager = new UImanager();
		AllServices.Container.Register<IUI>(uiManager);
		uiManager.Init();

		startUI = startUIobject.GetComponent<CUI>();
		saveLoadWindow.InittInterface();

		GameObject vGameConsole = Instantiate(prefabGameConsole, uiCanvas.transform);
		gameConsole = vGameConsole.GetComponent<CGameConsole>().GetIGameConsole();
		AllServices.Container.Register<IGameConsole>(gameConsole);
	}

	private void OnDestroy()
    {
		uiManager.CloseUI();
    }

    private void Start()
    {
		uiManager.OpenUI(startUI);
    }

	//-----------------------------------------------------
	// ISaveLoad
	//-----------------------------------------------------
	public string GetProfile() => saveFile.GetProfile();

	public bool SetProfile(string _name)
    {
		return saveFile.SetProfile(_name);
    }

	public bool IsSavedGameExist() => saveFile.IsSavedFileExist();
	public bool IsSavedGameExist(string _name) => saveFile.IsSavedFileExist(_name);

	public void Save(string _name)
	{
		//game.OnSave();
		//saveFile.Save(_name, game.GetData());
	}

	public void Load(string _name)
	{
		/*if (IsSavedGameExist())
		{
			SaveData data = game.GetData();
			saveFile.Load(_name, out data);
			gameID = data.id;
			gameManager.SetGameData(data); //-----??
			GoToMainScene();
		}
		else
			Debug.LogError("There is no save data!");*/
	}

	public void RemoveSave(string _name)
	{
		saveFile.ResetData(_name);
	}

	public string[] GetSavedList() => saveFile.GetSavedList();

	//--------------------------------------------------------------
	// IMainMenu interface
	//--------------------------------------------------------------
	public bool IsGameExist() => gameID > 0;

	public void SetLanguage(UsedLocal _language)
    {
		usedLanguage = _language;
		CLocalisation.LoadLocalPrefab(localData[(int)_language]);
    }

	public void SaveSettings()
    {
		SettingsData data = new SettingsData();
		data.profileName = GetProfile();
		data.selected = usedLanguage;
		saveFile.SaveSettings(data);
		uiManager.CloseUI();
    }

	public void NewGame()
	{
/*		SaveData data = game.GetData();
		gameID = (uint)UnityEngine.Random.Range(100, 10000000);
		if (data == null)
		{
			data = new SaveData();
			gameManager.SetGameData(data); // ??
		}

		data.id = gameID;
		data.SetColor(Color.gray);
		GoToMainScene();*/
	}

	public void GoToMainScene()
	{
		//SceneManager.LoadScene("MainScene");
	}

	public void MainMenuScene()
	{
        //game.OnSave();
		//SceneManager.LoadScene("LogoScene");
	}

	public void Save()
	{
		saveLoadWindow.OpenSaveWindow();
	}

	public void Load()
	{
		saveLoadWindow.OpenLoadWindow();
	}

	public void OpenSettings()
    {
		uiManager.OpenUI(settingsMenu.GetComponent<CUI>());
    }

	public void Quit()
    {
		dialog.OpenDialog(EDialog.Question, CLocalisation.GetString(ELocalStringID.core_quit) + "\n" + CLocalisation.GetString(ELocalStringID.msg_areYouSure), OnQuit);
	}

	private void OnQuit () 
	{
		#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
		#else
		Application.Quit();
		#endif
	}
}
