using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tab : MonoBehaviour
{
    [SerializeField] private ScreenType m_openScreenType;
    [SerializeField] private Button m_button;

    public Action<Tab> OnClick;

    private ScreenManager m_screenManager = null;

    private void Awake()
    {
        m_button.onClick.AddListener(Click);

        m_screenManager = Main.Instance.GetManager<ScreenManager>();
    }

    public void Open()
    {
        m_screenManager.OpenScreen(m_openScreenType);
    }

    public void Close()
    {
        m_screenManager.CloseScreen(m_openScreenType);
    }

    private void Click()
    {
        OnClick?.Invoke(this);
    }

    
}
