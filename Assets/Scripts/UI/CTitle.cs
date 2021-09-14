using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CTitle : CUI
{
    private void Start()
    {
        InitUI();
    }
    public void ObButton()
    {
        uiManager.CloseUI();
    }
}
