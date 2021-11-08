using UnityEngine;

public class CUI : MonoBehaviour
{
    protected IUI uiManager;
    protected IInputController inputController;
    protected ILocalization localization;
 
    protected void InitUI()
    {
        uiManager = AllServices.Container.Get<IUI>();
        inputController = AllServices.Container.Get<IInputController>();
        localization = AllServices.Container.Get<ILocalization>();
    }

    public void Hide() => gameObject.SetActive(false);
    public void Show() => gameObject.SetActive(true);
    public bool IsActive() => gameObject.activeSelf;

    public virtual void OnOpen() { }
    public virtual void OnClose() { }
    public virtual void OnYes() { }
    public virtual void OnNo() { }
    public virtual void OnCancel() 
    {
        uiManager.CloseUI();
    }

    private void Update()
    {
        if (inputController.IsPressedEnter())
        {
            OnYes();
            return;
        }
        if (inputController.IsPressedEscape())
        {
            OnCancel();
            return;
        }
    }
}
