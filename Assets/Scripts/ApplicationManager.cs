using System;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ApplicationManager : MonoBehaviour, IMainMenu, ISaveLoad, IGame
{
	[SerializeField] private int sceneID;
	private UsedLocal usedLanguage=UsedLocal.english;
	[SerializeField] Canvas uiCanvas;
	[SerializeField] CUI uiStart;
	[SerializeField] private GameObject prefabMainMenu;
	[SerializeField] private GameObject prefabSettingsMenu;
	[SerializeField] private GameObject prefabDialogWindow;
    [SerializeField] private GameObject prefabSaveLoadWindow;
	[SerializeField] private GameObject prefabGameConsole;
	[SerializeField] private GameObject[] localData=new GameObject[2];
	private CUI mainMenu;
	private CSaveFile saveFile;
    private CSaveLoad saveLoad;
	private CSettings settings;
	private IDialog dialog;
	private UImanager uiManager;
	private IGameConsole gameConsole;
	private IGame game;


    private void Awake()
    {
		SettingsData settingsData;

		game = this;
		SaveData data = game.GetData();

		AllServices.Container.Register<IMainMenu>(this);
		AllServices.Container.Register<IGame>(game);

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

		dialog = Instantiate(prefabDialogWindow, uiCanvas.transform).GetComponent<IDialog>();
		saveLoad = Instantiate(prefabSaveLoadWindow, uiCanvas.transform).GetComponent<CSaveLoad>();
		settings = Instantiate(prefabSettingsMenu, uiCanvas.transform).GetComponent<CSettings>();

		AllServices.Container.Register<IDialog>(dialog);
		AllServices.Container.Register<ISaveLoad>(this);

		mainMenu = Instantiate(prefabMainMenu, uiCanvas.transform).GetComponent<CUI>();

		saveLoad.InittInterface();

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
		uiManager.OpenUI(mainMenu);
		uiManager.OpenUI(uiStart);
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
		game.OnSave();
		saveFile.Save(_name, game.GetData());
	}

	public void Load(string _name)
	{
		if (IsSavedGameExist())
		{
			SaveData data = game.GetData();
			saveFile.Load(_name, out data);
			CGameManager.SetGameData(data); //-----??
			GoToMainScene();
		}
		else
			Debug.LogError("There is no save data!");
	}

	public void RemoveSave(string _name)
	{
		saveFile.ResetData(_name);
	}

	public string[] GetSavedList() => saveFile.GetSavedList();

	//--------------------------------------------------------------
	// IMainMenu interface
	//--------------------------------------------------------------
	public bool IsGameExist() => game.GetData()!=null;

	public void SetLanguage(UsedLocal _language)
    {
		usedLanguage = _language;
		CLocalisation.LoadLocalPrefab(localData[(int)_language]);
    }

	public void OpenStartUI()
    {
		uiManager.OpenUI(uiStart);
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
		SaveData data = game.GetData();
		if (data == null)
		{
			data = new SaveData();
			CGameManager.SetGameData(data); // ??
		}

		data.id = (uint)UnityEngine.Random.Range(100, 10000000);
		GoToMainScene();
	}

	public void GoToMainScene()
	{
		SceneManager.LoadScene("SceneBase");
	}

	public void MainMenuScene()
	{
        game.OnSave();
		SceneManager.LoadScene("SceneLogo");
	}

	public void Save()
	{
		saveLoad.OpenSaveWindow();
	}

	public void Load()
	{
		saveLoad.OpenLoadWindow();
	}

	public void OpenSettings()
    {
		uiManager.OpenUI(settings);
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

	//--------------------------------------------------------------
	// IGame interface
	//--------------------------------------------------------------
	public SaveData GetData()
    {
		return CGameManager.GetData();
    }

    public void OnSave()
    {
		CGameManager.OnSave();
    }

    public void AddOnSaveAction(Action _a)
    {
		CGameManager.onSave += _a;
    }

    public void RemoveOnSaveAction(Action _a)
    {
		CGameManager.onSave -= _a;
    }
}
