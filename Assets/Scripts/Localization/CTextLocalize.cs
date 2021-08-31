using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]

public class CTextLocalize : MonoBehaviour
{
    private Text textField;
    [SerializeField] public string strID;

    private void Start()
    {
        textField = GetComponent<Text>();
        RefreshText();
        CLocalisation.reloadText += RefreshText;
    }

    private void OnDestroy()
    {
        CLocalisation.reloadText -= RefreshText;
    }

    public void RefreshText()
    {
        textField.text = CLocalisation.GetString(strID);
    }
}
