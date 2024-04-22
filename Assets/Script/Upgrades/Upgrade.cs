using System;
using UnityEngine;

/// <summary>
/// This base class represents the active state of the upgrade.
/// It is instantiate when bought and save with its data
/// </summary>
public abstract class Upgrade : MonoBehaviour, ISavableLoadable<UpgradeData, UpgradeInformations>
{
    protected const string NEXT_ADVANTAGE_BASE = "[current] -> [next]";
    protected string currentAdv = "_";
    protected string nextAdv;

    protected UpgradeManager m_upgradeManager;
    protected UpgradeInformations m_informations;
    public UpgradeInformations Informations => m_informations;

    protected ushort m_currentLevel = 0;
    public int CurrentLevel => m_currentLevel;
    public int NextLevel => m_currentLevel + 1;
    protected bool m_isActive = true;
    public bool IsActive => m_isActive;

    public void Load(UpgradeData data, UpgradeInformations info)
    {
        m_informations = info;
        
        if (data != null) {
            m_currentLevel = data.CurrentLevel;
            m_isActive = data.IsActive;
        }
    }

    public void Save(ref UpgradeData data)
    {
        data.Type = m_informations.Type;
        data.CurrentLevel = m_currentLevel;
        data.IsActive = m_isActive;
    }

    public virtual void Initialize(UpgradeManager upgradeManager)
    {
        m_upgradeManager = upgradeManager;
        if (!CheckIntegrity()) return;
    }

    protected abstract bool CheckIntegrity();

    public virtual void ToggleActive()
    {
        if (!m_informations.CanBeToggled) return;
        m_isActive = !m_isActive;
        OnToggled();
    }

    public string GetNextAdvantageText()
    {
        return NEXT_ADVANTAGE_BASE.Replace("[current]", currentAdv).Replace("[next]", nextAdv);
    }

    public abstract void OnUpdate();

    public virtual void LevelUp()
    {
        m_currentLevel++;
        Events.Upgrade.FireUpgradeLeveledUp(m_informations.Type, CurrentLevel);
    }

    protected abstract void OnToggled();
}
