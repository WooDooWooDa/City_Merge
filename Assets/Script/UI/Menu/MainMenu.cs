using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject settingsPage;
    [SerializeField]
    private GameObject buildingsPage;
    [SerializeField]
    private GameObject buttons;

    public void OpenCloseSettings(bool state)
    {
        settingsPage.SetActive(state);
        RemoveOther(state);
    }

    public void OpenCloseBuildingsPage(bool state)
    {
        buildingsPage.SetActive(state);
        RemoveOther(state);
    }

    public void RemoveOther(bool state)
    {
        buttons.SetActive(!state);
        GameManager.BlockInput = state;
    }
}
