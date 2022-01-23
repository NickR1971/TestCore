using UnityEngine;

public class CSaveLoad : CUI
{
    [SerializeField] private CRecordContainer container;
    private ISaveLoad iSaveLoad;

    public void InittInterface()
    {
        InitUI();
        iSaveLoad = AllServices.Container.Get<ISaveLoad>();
    }

    private void OpenSaveLoadWindow()
    {
        uiManager.OpenUI(this);
    }
    
    public void OpenSaveWindow()
    {
        container.CreateListSave(iSaveLoad.GetSavedList(), iSaveLoad.GetCommentList());
        OpenSaveLoadWindow();
    }

    public void OpenLoadWindow()
    {
        container.CreateListLoad(iSaveLoad.GetSavedList(), iSaveLoad.GetCommentList());
        OpenSaveLoadWindow();
    }

    public void CloseWindow()
    {
        container.DestroyRecords();
        uiManager.CloseUI();
    }

    public void Save(string _name)
    {
        CloseWindow();
        iSaveLoad.Save(_name);
    }

    public void Load(string _name)
    {
        CloseWindow();
        iSaveLoad.Load(_name);
    }

    public void Remove(string _name)
    {
        CloseWindow();
        iSaveLoad.RemoveSave(_name);
    }
}
