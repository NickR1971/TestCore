using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]

public class CProfileView : MonoBehaviour
{
    private Text textField;
    private ISaveLoad saveLoad;

    void Start()
    {
        textField = GetComponent<Text>();
        saveLoad = AllServices.Container.Get<ISaveLoad>();
        textField.text = saveLoad.GetProfile();
    }
}
