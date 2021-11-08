using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]

public class CTextLocalize : MonoBehaviour
{
    private Text textField;
    [SerializeField] public string strID;
    private ILocalization localization;

    private void Start()
    {
        localization = AllServices.Container.Get<ILocalization>();
        textField = GetComponent<Text>();
        RefreshText();
        localization.SetOnChange(RefreshText);
    }

    private void OnDestroy()
    {
        localization.RemoveOnChange(RefreshText);
    }

    public void RefreshText()
    {
        textField.text = localization.GetString(strID);
    }
}
