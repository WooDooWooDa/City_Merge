using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabsMenu : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> tabs = new List<GameObject>();

    private GameObject selected;

    private void Start()
    {
        selected = tabs[0];
        Select(selected, true);
    }

    public void ToggleTab(int index)
    {
        selected = tabs[index];
        foreach (var tab in tabs) {
            Select(tab, tab == selected);
        }
    }

    private void Select(GameObject tab, bool state)
    {
        tab.GetComponent<Image>().color = new Color(1, 1, 1, state ? 0.5f : 1);
        tab.GetComponent<Button>().interactable = !state;
        //tab.GetComponentInChildren<Tab>().OpenCloseTab(state);
    }
}
