using System;
using UnityEngine;
using UnityEngine.UI;

public class MainScreen: UIScreenBase
{
    [Header("Specific")]
    [SerializeField] private Button m_tempCollectionButton;
    [SerializeField] private Button m_buildingStoreBtn;
    [SerializeField] private Button m_upgradeStoreBtn;
    [SerializeField] private ProfileUI m_playerUI;

    protected override void SoftEnable(bool show)
    {
        base.SoftEnable(show);

        //m_bottomButtons.gameObject.SetActive(show);
    }

    public override UIScreenBase Open(OpenInfo openInfo)
    {
        base.Open(openInfo);

        m_buildingStoreBtn.onClick.AddListener(() =>
            Main.Instance.GetManager<ScreenManager>().ToggleScreen(ScreenType.BuildingStore));

        m_upgradeStoreBtn.onClick.AddListener(() => 
            Main.Instance.GetManager<ScreenManager>().ToggleScreen(ScreenType.UpgradeStore));

        m_tempCollectionButton.onClick.AddListener(() =>
            Main.Instance.GetManager<ScreenManager>().ToggleScreen(ScreenType.CollectionMenu));

        return this;
    }
}
