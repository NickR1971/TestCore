using UnityEngine;

public class CUI : MonoBehaviour
{
    protected IUI uiManager;
 
    protected void InitUI()
    {
        uiManager = AllServices.Container.Get<IUI>();
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
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            OnYes();
            return;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnCancel();
            return;
        }
    }
}
