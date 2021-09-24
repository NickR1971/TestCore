using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoad : MonoBehaviour, ISaveLoad
{
	private IGame game;
	private CSaveFile saveFile;
	private IMainMenu mainMenu;

    void Start()
    {
		game = AllServices.Container.Get<IGame>();
		mainMenu = AllServices.Container.Get<IMainMenu>();
    }

	public void Init(CSaveFile _saveFile)
    {
		saveFile = _saveFile;
    }

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
			mainMenu.GoToMainScene();
		}
		else
			Debug.LogError("There is no save data!");
	}

	public void RemoveSave(string _name)
	{
		saveFile.ResetData(_name);
	}

	public string[] GetSavedList() => saveFile.GetSavedList();

}
