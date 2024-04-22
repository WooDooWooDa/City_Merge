using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabGroup : MonoBehaviour
{
    [SerializeField] private List<Tab> m_tabs = new List<Tab>();

    private int m_selectedTabIndex = -1;
    private Tab m_selectedTab = null;

    private void OnEnable()
    {
        foreach (var tab in m_tabs) {
            tab.OnClick = OnTabClick;
        }

        //FitAll();
    }

    private void OnTabClick(Tab tabClicked)
    {
        if (m_selectedTab != null) {
            m_selectedTab.Close();
        }
        //only open if tab is different, otherwise just close
        if (m_selectedTab != tabClicked) {
            m_selectedTab = tabClicked;
            m_selectedTab.Open();
        }
    }

    private void FitAll()
    {
        RectTransform groupRect = GetComponent<RectTransform>();
        float groupWidth = groupRect.rect.width;
        
    }
}
