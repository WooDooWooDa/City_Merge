using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class UpgradeStoreOpenInfo : OpenInfo
{
    
}

public class UpgradeStoreScreen : UIScreenBase
{
    [Header("Specific")]
    [SerializeField] private UpgradeStoreItem m_storeItem;
    [SerializeField] private Transform m_listParent;

    private UpgradeManager m_upgradeManager;

    public override UIScreenBase Open(OpenInfo openInfo)
    {
        base.Open(openInfo);

        var data = openInfo as UpgradeStoreOpenInfo;
        if (data is UpgradeStoreOpenInfo) {

        }

        m_upgradeManager = Main.Instance.GetManager<UpgradeManager>();
        InitializeUpgradeList();

        return this;
    }

    private void InitializeUpgradeList()
    {
        UpgradeInformations[] upgrades = Main.Instance.GlobalConfig.UpgradeConfig.UpgradeInformations;
        foreach (var upgrade in upgrades) {
            UpgradeStoreItem item = Instantiate(m_storeItem, m_listParent);
            item.OnInitialize(m_upgradeManager, upgrade, () => m_upgradeManager.TryBuyUpgrade(upgrade.Type));
        }
    }
}
