using System;

[Serializable]
public class SettingsData
{
    public string profileName;
    public UsedLocal selected;

    public SettingsData()
    {
        profileName = "Player";
        selected = UsedLocal.english;
    }
}
