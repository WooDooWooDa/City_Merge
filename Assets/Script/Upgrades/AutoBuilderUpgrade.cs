using System;
using UnityEngine;

public class AutoBuilderUpgrade: Upgrade
{
    [SerializeField] private AutoBuilderItem m_autoBuilderPrefab;
    [Space]
    [SerializeField] private BuildingKey[] m_autoBuildingPerUpgradeLvl;
    private BuildingManager m_buildingManager;

    private AutoBuilderItem m_autoBuilder;

    private float m_baseTime = 10f;
    private float m_time;
    private float m_currentTime = 0f;

    public override void Initialize(UpgradeManager upgradeManager)
    {
        base.Initialize(upgradeManager);

        m_buildingManager = Main.Instance.GetManager<BuildingManager>();

        GameObject btn = GameObject.FindGameObjectWithTag("FreeBuildingBtn");
        if (btn != null) {
            m_autoBuilder = Instantiate(m_autoBuilderPrefab, btn.transform);
            m_autoBuilder.Initialize(this);
            m_autoBuilder.OnToggle(m_isActive);
        } else
            Events.Screen.OnScreenOpened += TryAddItem;

        m_time = m_baseTime - (m_currentLevel - 1);
        currentAdv = $"{m_time}";
        nextAdv = $"{m_baseTime - m_currentLevel} sec";
    }

    public override void LevelUp()
    {
        base.LevelUp();

        m_time = m_baseTime - (m_currentLevel - 1);
        currentAdv = $"{m_time}";
        nextAdv = $"{m_baseTime - m_currentLevel} sec";
    }

    public override void OnUpdate()
    {
        if (m_autoBuilder == null) return;

        m_currentTime += Time.deltaTime;
        if (m_currentTime >= m_time) {
            TryAutoBuild();
        }
        m_autoBuilder.OnChangeTime(Math.Min(m_currentTime, m_time) / m_time);
    }

    protected override bool CheckIntegrity()
    {
        if (m_autoBuildingPerUpgradeLvl.Length < m_informations.MaxLevel) return false;

        if (m_autoBuilder != null) return false;

        return true;
    }

    protected override void OnToggled()
    {
        m_currentTime = 0;
        m_autoBuilder.OnToggle(m_isActive);
    }

    private void TryAddItem(ScreenType type)
    {
        if (m_autoBuilder != null) return;

        if (type == ScreenType.Main) {
            GameObject btn = GameObject.FindGameObjectWithTag("FreeBuildingBtn");
            if (btn != null) {
                m_autoBuilder = Instantiate(m_autoBuilderPrefab, btn.transform);
                m_autoBuilder.Initialize(this);
                m_autoBuilder.OnToggle(m_isActive);
            }
            Events.Screen.OnScreenOpened -= TryAddItem;
        }
    }

    private void TryAutoBuild()
    {
        if (m_buildingManager == null) return;

        if (m_buildingManager.TryConstructBuilding(new BuildingKey { Level = BuildingLevel.One, Type = BuildingType.Commercial})) {
            m_currentTime = 0;
            m_autoBuilder.Pause(false);
        } else {
            m_autoBuilder.Pause(true);
        }
    }
}