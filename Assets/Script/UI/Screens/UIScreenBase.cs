using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class OpenInfo
{
    public ScreenInformations Informations;
    public ScreenAnimationBase ScreenAnimationOverride = null;
    public bool HasBack = false;
    public bool BeingToggled = false;

    //Events
    public Action OnCloseScreen;
    public Action OnBackAction;
}

public class UIScreenBase: MonoBehaviour
{
    [Header("Base")]
    [SerializeField] private Button m_backButton;
    [SerializeField] private Button m_closeButton;

    protected ScreenInformations m_informations;
    protected OpenInfo m_openInfo = null;
    private bool m_backPressed = false;
    protected bool m_needsRefresh = false;
    private ScreenAnimationBase m_screenAnimation;

    protected virtual void Awake()
    {
        m_backButton?.onClick.AddListener(Back);
        m_closeButton?.onClick.AddListener(Close);
    }

    public void Enable(bool show)
    {
        if (m_informations.StaticScreen) {
            SoftEnable(show);
            return;
        }

        gameObject.SetActive(show);
    }

    protected virtual void SoftEnable(bool show)
    {

    }

    public virtual UIScreenBase Open(OpenInfo openInfo)
    {
        m_informations = openInfo.Informations;
        if (m_informations.ScreenLayer == ScreenLayer.Popup) {
            transform.parent.gameObject.SetActive(true);
        }
        m_openInfo = openInfo;
        m_screenAnimation = m_informations.m_screenAnimation;
        if (openInfo != null && openInfo.ScreenAnimationOverride != null) {
            m_screenAnimation = openInfo.ScreenAnimationOverride;
        }

        m_backButton?.gameObject.SetActive(openInfo.HasBack);

        m_screenAnimation?.OpenAnimation(gameObject);

        return this;
    }

    public virtual void OnBack()
    {

    }

    public virtual void OnClose()
    {
        if (m_informations.ScreenLayer == ScreenLayer.Popup) {
            transform.parent.gameObject.SetActive(false);
        }
        if (m_backPressed) {
            m_openInfo?.OnBackAction?.Invoke();
        } else {
            m_openInfo?.OnCloseScreen?.Invoke();
        }

        if (m_screenAnimation != null)
            m_screenAnimation.CloseAnimation(gameObject);
    }

    protected void Back()
    {
        if (m_openInfo.HasBack) {
            m_backPressed = true;
            Main.Instance.GetManager<ScreenManager>().Back();
        }
    }

    protected void Close()
    {
        Main.Instance.GetManager<ScreenManager>().CloseScreen(m_informations.ScreenType);
    }

    public void RefreshLayoutGroupsImmediateAndRecursive(GameObject root)
    {
        
        foreach (var layoutGroup in root.GetComponentsInChildren<LayoutGroup>()) {
            Debug.Log("Refresing " + layoutGroup);
            StartCoroutine(UpdateLayoutGroup(layoutGroup));
        }
    }

    IEnumerator UpdateLayoutGroup(LayoutGroup layoutGroupComponent)
    {
        yield return new WaitForEndOfFrame();

        layoutGroupComponent.enabled = false;

        layoutGroupComponent.CalculateLayoutInputVertical();
        layoutGroupComponent.CalculateLayoutInputHorizontal();

        LayoutRebuilder.ForceRebuildLayoutImmediate(layoutGroupComponent.GetComponent<RectTransform>());

        layoutGroupComponent.enabled = true;
    }
}