using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LoadingScreen : UIScreenBase
{
    [Header("Specific")]
    [SerializeField] private LoadingBar m_loadingBar;
    [SerializeField] private Button m_playButton;

    private int m_managersToInitialize;
    private int m_managerInitalized = 0;

    protected override void Awake()
    {
        base.Awake();

        m_managersToInitialize = Main.Instance.Managers.Count;
        m_loadingBar.Initialize(m_managersToInitialize);

        Events.Loading.OnFinishLoading += OnFinishLoading;
        Events.Loading.OnStartLoadingManager += UpdateLoadingInfo;
        Events.Loading.OnManagerFinishLoading += UpdateLoadingBar;
    }

    private void Play()
    {
        Main.Instance.GetManager<ScreenManager>().CloseScreen(ScreenType.Loading);
        Events.Loading.FireLoadingPageClose();
    }

    private void OnFinishLoading()
    {
        m_loadingBar.UpdateInfo("Press anywhere to start", false);
        m_loadingBar.GetComponentInChildren<Animator>().SetTrigger("Start");

        m_playButton.onClick.AddListener(Play);
        m_playButton.gameObject.SetActive(true);
    }

    public override void OnClose()
    {
        base.OnClose();

        Events.Loading.OnFinishLoading -= OnFinishLoading;
        Events.Loading.OnStartLoadingManager -= UpdateLoadingInfo;
        Events.Loading.OnManagerFinishLoading -= UpdateLoadingBar;
    }

    private void UpdateLoadingInfo(Manager manager)
    {
        m_loadingBar.UpdateInfo(manager.LoadingInfo);
    }

    private void UpdateLoadingBar()
    {
        m_loadingBar.UpdateValue(++m_managerInitalized);
    }
}
