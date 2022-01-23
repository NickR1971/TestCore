using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSaveFile
{
	private string settingsFileName;
	private string profileName;
	private ProfileData profileData;

	public CSaveFile()
    {
		SetProfile("Default");
		settingsFileName = Application.persistentDataPath + "/SettingsData.dat";
	}

	private string CreateProfileName(string _name) => Application.persistentDataPath + "/" + _name.Trim() + ".dat";

	private void SaveProfile()
    {
		string fileName = CreateProfileName(profileName);
		SaveFile<ProfileData>(profileData, fileName);
    }

	private bool LoadProfile(string _name)
    {
		if (!CUtil.CheckNameForSave(_name)) return false;
		string fileName = CreateProfileName(_name);
		if (File.Exists(fileName))
        {
			LoadFile<ProfileData>(out profileData, fileName);
        }
		else
        {
			profileData = new ProfileData();
			SaveFile<ProfileData>(profileData, fileName);
        }
		return true;
    }

	public bool SetProfile(string _name)
    {
		if ( ! LoadProfile(_name)) return false;

		profileName = _name;

		return true;
    }

	public string GetProfile() => profileName;

	public bool IsSavedFileExist() => profileData.IsSaveExist();

	public bool IsSavedFileExist(string _name)
    {
		return File.Exists(CreateSaveFileName(_name));
    }

	private void SaveFile<T>(T _data, string _name)
    {
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(_name);
		bf.Serialize(file, _data);
		file.Close();
    }

	public void Save(string _name, SaveData _data)
    {
		profileData.RemoveSave(_name);
		profileData.AddSave(_name, _data.comment);
		SaveProfile();
		SaveFile(_data, CreateSaveFileName(_name));
    }

	private string CreateSaveFileName(string _name) => Application.persistentDataPath + "/" + profileName.Trim() + "_" + _name.Trim() + "_save.dat";
	private void LoadFile<T>(out T _data, string _name)
    {
		if (File.Exists(_name))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(_name, FileMode.Open);
			_data = (T)bf.Deserialize(file);
			file.Close();
		}
		else _data = default;// (T);
    }

	public void Load(string _name,out SaveData _data)
    {
		SaveData data;
		LoadFile(out data, CreateSaveFileName(_name));
		_data = data;
		if (_data==null)
		{
			Debug.LogError("There is no save data!");
		}
    }

	public void LoadSettings(out SettingsData _data)
    {
		SettingsData data;
		LoadFile<SettingsData>(out data, settingsFileName);
		_data = data;
    }

	public void SaveSettings(SettingsData _data)
    {
		SaveFile<SettingsData>(_data, settingsFileName);
    }

	private void RemoveFile(string _name)
    {
		if (File.Exists(_name))
		{
			File.Delete(_name);
		}
		else
			Debug.LogError("No save data to delete.");
    }

	public void ResetData(string _name)
	{
		if (profileData.RemoveSave(_name))
        {
			SaveProfile();
			RemoveFile(CreateSaveFileName(_name));
        }
	}
	public string[] GetSavedList() => profileData.GetSavedList();
	public string[] GetCommentList() => profileData.GetCommentList();
}
