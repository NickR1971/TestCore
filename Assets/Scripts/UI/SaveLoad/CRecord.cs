using UnityEngine;
using UnityEngine.UI;

public class CRecord : MonoBehaviour
{
    [SerializeField] private Text saveName;
    [SerializeField] private CSaveLoad manager;
    [SerializeField] private CTextLocalize buttonText;
    [SerializeField] private Button ActionButton;
    [SerializeField] private Button DeleteButton;
    private IDialog dialog;
    private ISaveLoad saveLoad;
    private string strName;

    private void Start()
    {
        dialog = AllServices.Container.Get<IDialog>();
        saveLoad = AllServices.Container.Get<ISaveLoad>();
    }
    public void InitZero()
    {
        saveName.text = CLocalisation.GetString(ELocalStringID.core_newSave);
        buttonText.strID = ELocalStringID.core_saveGame.ToString();
        ActionButton.onClick.AddListener(OnNewSave);
        DeleteButton.gameObject.SetActive(false);
    }

    public void InitSave(string _name)
    {
        saveName.text = _name;
        buttonText.strID = ELocalStringID.core_saveGame.ToString();
        ActionButton.onClick.AddListener(OnSaveOK);
    }

    public void InitLoad(string _name)
    {
        saveName.text = _name;
        buttonText.strID = ELocalStringID.core_loadGame.ToString();
        ActionButton.onClick.AddListener(OnLoadOK);
    }
    public void ResetTemplate()
    {
        ActionButton.onClick.RemoveAllListeners();
        DeleteButton.gameObject.SetActive(true);
    }

    private void NewSave(string _name)
    {
        if (CUtil.CheckNameForSave(_name)) OnSaveCheck(_name.Replace('.','_'));
        else
        {
            dialog.OpenDialog(EDialog.Error, CLocalisation.GetString(ELocalStringID.core_empty) + " " + _name); //err_invalidName
        }
    }
    public void OnNewSave()
    {
        dialog.SetOnInput(NewSave);
        dialog.OpenDialog(EDialog.Input, CLocalisation.GetString(ELocalStringID.core_newSave));
    }

    private void OnSaveOK()
    {
        OnSaveCheck(saveName.text);
    }

    private void DoSave()
    {
        manager.Save(strName);
        strName = null;
    }

    private void OnSaveCheck(string _name)
    {
        strName = _name;
        if (saveLoad.IsSavedGameExist(_name))
        {
            dialog.OpenDialog(EDialog.Question, CLocalisation.GetString(ELocalStringID.core_overwrite) + " " + strName + "?", DoSave);
        }
        else DoSave();
    }
 
    public void OnLoadOK()
    {
        manager.Load(saveName.text);
    }
    private void DeleteOk()
    {
        manager.Remove(saveName.text);
    }
    public void OnDelete()
    {
        dialog.OpenDialog(EDialog.Question, CLocalisation.GetString(ELocalStringID.msg_remove) + " " + saveName.text + "?", DeleteOk);
    }
}
