using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveLoad : IService
{
	string GetProfile();
	bool SetProfile(string _name);
	bool IsSavedGameExist();
	bool IsSavedGameExist(string _name);
	void Save(string _name);
	void Load(string _name);
	void RemoveSave(string _name);
	string[] GetSavedList();
	string[] GetCommentList();
}
