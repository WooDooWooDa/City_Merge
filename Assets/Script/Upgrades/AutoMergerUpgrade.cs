using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class AutoMergerUpgrade : Upgrade
{
    private BuildingManager m_buildingManager;

    private float m_baseTime = 10f;
    private float m_time;
    private float m_currentTime = 0f;

    public override void Initialize(UpgradeManager upgradeManager)
    {
        base.Initialize(upgradeManager);

        m_buildingManager = Main.Instance.GetManager<BuildingManager>();

        m_time = m_baseTime - (m_currentLevel - 1);
    }

    public override void LevelUp()
    {
        base.LevelUp();

        m_time = m_baseTime - (m_currentLevel - 1);
    }

    public override void OnUpdate()
    {
        m_currentTime += Time.deltaTime;
        if (m_currentTime >= m_time) {
            TryAutoMerge();
        }
    }

    protected override bool CheckIntegrity()
    {
        return true;
    }

    protected override void OnToggled()
    {
        m_currentTime = 0;
    }

    private void TryAutoMerge()
    {
        if (m_buildingManager == null) return;

        //TODO compare all building

        //if (m_buildingManager.TryMerge()) {
        m_currentTime = 0;
        //}
    }
}
