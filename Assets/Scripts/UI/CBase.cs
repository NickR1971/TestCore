using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CBase : CUI
{
    private void Start()
    {
        InitUI();
    }

    public void ToBattle()
    {
        SceneManager.LoadScene("SceneBattle");
    }
}
