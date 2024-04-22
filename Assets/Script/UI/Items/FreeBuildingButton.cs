using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FreeBuildingButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_currentGoalText;
    [SerializeField] private Button m_button;
    [SerializeField] private Image m_imageFiller;

    private BuildingManager m_manager;
    private BuildingType m_currentBuildingType = BuildingType.Commercial;

    private float m_baseGoal;
    private float m_goal;
    private float m_progess = 0;

    private float m_goOutTime = 1f;
    private float m_goOut = 0;

    private void Awake()
    {
        m_manager = Main.Instance.GetManager<BuildingManager>();
        if (m_manager != null) {
            m_goal = m_baseGoal = m_manager.Data.FreeBuildingCharge;
        }
        if (Main.Instance.GetManager<UpgradeManager>().HasUpgradeActive(UpgradeType.FasterBuild, out Upgrade upgrade)) {
            m_goal = m_baseGoal + ((FasterBuildUpgrade)(upgrade)).CurrentGoalWithUpgrade;
        }

        m_button.onClick.AddListener(OnClick);
    }

    private void Update()
    {
        if (m_goOut > 0 ) {
            m_goOut -= Time.deltaTime;

            if (m_goOut <= 0) {
                m_currentGoalText.alpha = 0;
            }
        }
    }

    private void OnEnable()
    {
        Events.Upgrade.OnUpgradeLeveledUp += ChangeGoal;
    }

    private void OnDisable()
    {
        Events.Upgrade.OnUpgradeLeveledUp -= ChangeGoal;
    }

    private void OnClick()
    {
        m_currentGoalText.alpha = 0.65f;
        m_goOut = m_goOutTime;

        m_progess = Mathf.Min(++m_progess, m_goal);
        m_imageFiller.fillAmount = m_progess / m_goal;
        m_currentGoalText.text = $"{m_progess}/{m_goal}";
        if (m_progess >= m_goal) {
            if (!m_manager.TryConstructBuilding(new BuildingKey { Level = BuildingLevel.One, Type = m_currentBuildingType })) {
                //no space, deactivate
                return;
            }

            m_progess = 0;
        }
        m_imageFiller.fillAmount = m_progess / m_goal;
        m_currentGoalText.text = $"{m_progess}/{m_goal}";
    }

    private void ChangeGoal(UpgradeType type, int newLevel)
    {
        if (UpgradeType.FasterBuild == type) {
            if (Main.Instance.GetManager<UpgradeManager>().HasUpgradeActive(type, out Upgrade upgrade)) {
                m_goal = m_baseGoal + ((FasterBuildUpgrade)(upgrade)).CurrentGoalWithUpgrade;
                m_imageFiller.fillAmount = m_progess / m_goal;
                //Spawn a little animation
                if (m_progess >= m_goal) {
                    if (!m_manager.TryConstructBuilding(new BuildingKey { Level = BuildingLevel.One, Type = m_currentBuildingType })) {
                        return;
                    }
                    m_progess = 0;
                }
                m_imageFiller.fillAmount = m_progess / m_goal;
            }
        }
    }
}
