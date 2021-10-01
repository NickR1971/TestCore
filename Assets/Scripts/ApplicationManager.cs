using System;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ApplicationManager : MonoBehaviour, IMainMenu
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
    private CSaveLoad saveLoadWindow;
	private SaveLoad saveLoad;
	private CSettings settings;
	private IDialog dialog;
	private UImanager uiManager;
	private IGameConsole gameConsole;
	private IGame game;


    private void Awake()
    {
		SettingsData settingsData;

		game = GetComponent<IGame>();
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
		saveLoad = GetComponent<SaveLoad>();
		saveLoad.Init(saveFile);
		saveLoad.SetProfile(settingsData.profileName);

		if (CLocalisation.Init())
			CLocalisation.LoadLocalPrefab(localData[(int)usedLanguage]);

		uiManager = new UImanager();
		AllServices.Container.Register<IUI>(uiManager);
		uiManager.Init();

		dialog = Instantiate(prefabDialogWindow, uiCanvas.transform).GetComponent<IDialog>();
		saveLoadWindow = Instantiate(prefabSaveLoadWindow, uiCanvas.transform).GetComponent<CSaveLoad>();
		settings = Instantiate(prefabSettingsMenu, uiCanvas.transform).GetComponent<CSettings>();

		AllServices.Container.Register<IDialog>(dialog);
		AllServices.Container.Register<ISaveLoad>(saveLoad);

		mainMenu = Instantiate(prefabMainMenu, uiCanvas.transform).GetComponent<CUI>();

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
		uiManager.OpenUI(mainMenu);
		uiManager.OpenUI(uiStart);
	}

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
		data.profileName = saveLoad.GetProfile();
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
		saveLoadWindow.OpenSaveWindow();
	}

	public void Load()
	{
		saveLoadWindow.OpenLoadWindow();
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

}
