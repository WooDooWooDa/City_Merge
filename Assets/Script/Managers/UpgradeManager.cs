using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class UpgradeManager : Manager
{
    private Dictionary<UpgradeType, UpgradeInformations> m_upgradesInformations = new Dictionary<UpgradeType, UpgradeInformations>();
    private Dictionary<UpgradeType, Upgrade> m_currentActiveUpgrade = new Dictionary<UpgradeType, Upgrade>();

    private UpgradeConfig m_config;

    private ProfileManager m_profileManager;
    private CollectionManager m_collection;

    public override void Initialize()
    {
        base.Initialize();

        m_config = Main.Instance.GlobalConfig.UpgradeConfig;

        if (m_config.UpgradeInformations != null) {
            foreach (var upgradeInfo in m_config.UpgradeInformations) {
                if (upgradeInfo.Price.Length != upgradeInfo.MaxLevel)
                    debugger.LogWarning("Upgrade of type " + upgradeInfo.Type + " doesn't have an equal amount of prices to it's max level");
                m_upgradesInformations.Add(upgradeInfo.Type, upgradeInfo);
            }
        }

        m_profileManager = Main.Instance.GetManager<ProfileManager>();
        m_collection = Main.Instance.GetManager<CollectionManager>();

        IsInitialize = true;
    }

    public override void OnUpdate(float delta)
    {
        foreach (Upgrade upgrade in m_currentActiveUpgrade.Values) {
            if (upgrade.Informations.HasUpdateActions && upgrade.IsActive)
                upgrade.OnUpdate();
        }
    }

    public bool TryBuyUpgrade(UpgradeType type)
    {
        
        if (m_upgradesInformations.TryGetValue(type, out UpgradeInformations info)) {
            if (HasUpgradeActive(type, out Upgrade upgrade)) {
                debugger.Log("Trying to level up upgrade of type : " + type);
                if (upgrade.CurrentLevel != info.MaxLevel && m_profileManager.HasEnough(info.GetPrice(upgrade.CurrentLevel))) {
                    m_profileManager.RemoveCurrency(info.GetPrice(upgrade.CurrentLevel));
                    upgrade.LevelUp();
                    return true;
                }
            } else {
                debugger.Log("Trying to buy up upgrade of type : " + type);
                if (info.UpgradePrefab == null) {
                    debugger.LogWarning("This upgrade doesnt have a prefab and won't be purchased");
                    return false;
                }
                if (m_profileManager.HasEnough(info.Price[0]) && m_profileManager.PlayerLevel >= info.MinPlayerLevel) {
                    Upgrade newUpgrade = CreateUpgrade(type);
                    m_collection.AddToCollection(CollectionType.Upgrade, (int)type);
                    m_profileManager.RemoveCurrency(info.Price[0]);
                    newUpgrade.LevelUp();
                    Events.Upgrade.FireUpgradeBought(type);
                    return true;
                } 
            }            
        }
        return false;
    }

    private Upgrade CreateUpgrade(UpgradeType type, UpgradeData data = null)
    {
        if (m_upgradesInformations.TryGetValue(type, out UpgradeInformations info)) {
            if (info.UpgradePrefab == null) return null;

            Upgrade newUpgrade = Instantiate(info.UpgradePrefab, transform);
            newUpgrade.Load(data, info);
            newUpgrade.Initialize(this);
            AddToActive(newUpgrade);
            return newUpgrade;
        }
        return null;
    }

    private void AddToActive(Upgrade newUpgrade)
    {
        if (!m_currentActiveUpgrade.ContainsKey(newUpgrade.Informations.Type))
            m_currentActiveUpgrade.Add(newUpgrade.Informations.Type, newUpgrade);
    }

    public bool HasUpgradeActive(UpgradeType type, out Upgrade outUpgrade)
    {
        outUpgrade = null;
        foreach (Upgrade upgrade in m_currentActiveUpgrade.Values) {
            if (upgrade.Informations.Type == type) {
                outUpgrade = upgrade;
                return true;

            }
        }
        return false;
    }
}
