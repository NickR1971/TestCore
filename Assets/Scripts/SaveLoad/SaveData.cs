using System;
using UnityEngine;
/*****************************************************************
 * Semantic Versioning expresses versions as MAJOR.MINOR.PATCH,
 * where MAJOR introduces one or more breaking changes,
 * MINOR introduces one or more backward-compatible API changes,
 * and PATCH only introduces bug fixes with no API changes at all.
 * */
[Serializable]
public class SaveData
{
	public uint id;
	public int versionMajor;
	public int versionMinor;
	public int versionPatch;
	public string comment;
	public string data;
}
