using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class UpgradeManager : Manager
{
    public override string LoadingInfo => "Upgrading your stuff";

    private UpgradeSaveData m_data = new UpgradeSaveData();
    public UpgradeSaveData Data { get { return m_data; } set { m_data = value; } }

    public override bool Load(ref SaveData data)
    {
        m_data = data.UpgradeSaveData;

        //Instantiate the upgrade object for each saved/bought upgrades
        foreach (UpgradeData upgradeData in m_data.UpgradeDatas) {
            CreateUpgrade(upgradeData.Type, upgradeData);
        }

        return true;
    }

    public override bool Save(ref SaveData data)
    {
        m_data.UpgradeDatas = new UpgradeData[m_currentActiveUpgrade.Count];
        uint i = 0;
        foreach (Upgrade upgrade in m_currentActiveUpgrade.Values) {
            m_data.UpgradeDatas[i] = new UpgradeData();
            upgrade.Save(ref m_data.UpgradeDatas[i]);
            i++;
        }

        data.UpgradeSaveData = m_data;
        return true;
    }
}

[System.Serializable]
public class UpgradeSaveData
{
    public UpgradeData[] UpgradeDatas = new UpgradeData[] { };
}

[System.Serializable]
public class UpgradeData
{
    public UpgradeType Type;
    public ushort CurrentLevel = 0;
    public bool IsActive;
}
