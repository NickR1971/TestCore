using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CMenu : CUI
{
    protected SortedList<ELocalStringID, Button> buttons = new SortedList<ELocalStringID, Button>();
    protected IMainMenu iMainMenu;

    [SerializeField] private Button btnPrefab;
    private Button lastButton;

    private void OnDestroy()
    {
        localization.RemoveOnChange(RefreshText);
    }

    protected void InitMenu()
    {
        InitUI();
        iMainMenu = AllServices.Container.Get<IMainMenu>();
        lastButton = null;
        localization.SetOnChange(RefreshText);
    }

    public int GetNumButtons() => buttons.Count;

    private void SetLastButtonText(ELocalStringID _id)
    {
        lastButton.transform.GetChild(0).GetComponent<Text>().text = localization.GetString(_id);
    }

    protected Button AddButton(ELocalStringID _id)
    {
        lastButton = Instantiate(btnPrefab, Vector3.zero, Quaternion.identity, transform);
        buttons.Add(_id,lastButton);
        SetLastButtonText(_id);

        return lastButton;
    }

    public void RefreshText()
    {
        int i;

        for (i = 0; i < buttons.Count; i++)
        {
            foreach (KeyValuePair<ELocalStringID, Button> kvp in buttons)
            {
                lastButton = kvp.Value;
                SetLastButtonText(kvp.Key);
            }
        }
    }

    protected Button LastButton() => lastButton;
}
