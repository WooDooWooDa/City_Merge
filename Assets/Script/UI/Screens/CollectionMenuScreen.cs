using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

class CollectionMenuScreen : UIScreenBase
{
    [Header("Specific")]
    [SerializeField] private Button m_buildingCollection;
    [SerializeField] private Button m_stats;
    [SerializeField] private Button m_trophy;

    protected override void Awake()
    {
        base.Awake();
    }

    public override UIScreenBase Open(OpenInfo openInfo)
    {
        base.Open(openInfo);

        m_buildingCollection.onClick.AddListener(() =>
            Main.Instance.GetManager<ScreenManager>().OpenScreen(ScreenType.Collection, new OpenInfo { HasBack = true }));

        m_stats.onClick.AddListener(() =>
            Main.Instance.GetManager<ScreenManager>().OpenScreen(ScreenType.Statistic, new OpenInfo { HasBack = true }));

        m_trophy.onClick.AddListener(() =>
            Main.Instance.GetManager<ScreenManager>().OpenScreen(ScreenType.Trophy, new OpenInfo { HasBack = true }));

        return this;
    }
}
