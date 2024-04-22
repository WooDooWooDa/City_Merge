using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

//TODO generalize this class to reuse in building store page
public class UpgradeStoreItem : MonoBehaviour
{
    [Header("Left")]
    [SerializeField] private Image m_icon;
    [SerializeField] private TextMeshProUGUI m_name;
    [SerializeField] private TextMeshProUGUI m_buyLevelUp;
    [SerializeField] private TextMeshProUGUI m_price;
    [SerializeField] private TextMeshProUGUI m_upgradeCurrentNext;
    [SerializeField] private TextMeshProUGUI m_description;
    [Header("Right")]
    [SerializeField] private GameObject m_maxLevel;
    [SerializeField] private Button m_buyButton;
    [Header("Levels")]
    [SerializeField] private Transform m_levelSliderParent;
    [SerializeField] private Slider m_levelSlider;
    [SerializeField] private TextMeshProUGUI m_level;

    private UpgradeInformations m_informations;
    private ProfileManager m_profile;
    private UpgradeManager m_upgradeManager;
    private Upgrade m_currentUpgrade;
    private Currency m_currencyData;

    [Serializable]
    private struct State
    {
        public bool IsAvailable;
        public bool HasUpgrade;
        public bool MaxLevel;
    }
    private State m_state;

    public void OnInitialize(UpgradeManager manager, UpgradeInformations info, Action clickAction)
    {
        m_upgradeManager = manager;
        if (info == null) return;
        
        m_informations = info;

        if (clickAction != null) {
            m_buyButton.onClick.AddListener(new UnityAction(clickAction));
        }

        if (m_upgradeManager.HasUpgradeActive(info.Type, out m_currentUpgrade)) {
            m_state.HasUpgrade = true;
            m_state.MaxLevel = m_currentUpgrade.CurrentLevel == m_informations.MaxLevel;
            if (!m_state.MaxLevel) Events.Upgrade.OnUpgradeLeveledUp += UpgradeLeveledUp;
        } else {
            m_state.HasUpgrade = false;
            Events.Upgrade.OnUpgradeBought += UpgradeBought;
        }
        m_upgradeCurrentNext.transform.gameObject.SetActive(m_state.HasUpgrade);

        m_profile = Main.Instance.GetManager<ProfileManager>();
        m_state.IsAvailable = m_profile.PlayerLevel >= info.MinPlayerLevel;
        if (!m_state.IsAvailable) Events.Profile.OnLevelUp += OnPlayerLvlUp;
        Events.Profile.OnMoneyChanged += OnMoneyChanged;

        UpdateState();
    }

    private void OnMoneyChanged()
    {
        m_buyButton.interactable = m_profile.HasEnough(m_state.HasUpgrade 
            ? m_informations.GetPrice(m_currentUpgrade.CurrentLevel)
            : m_informations.GetPrice(0));
    }

    private void UpdateState()
    {
        m_description.transform.gameObject.SetActive(m_state.IsAvailable);
        if (m_state.IsAvailable) {
            m_icon.sprite = m_informations.Icon;
            m_name.text = m_informations.Name;
            m_description.text = m_informations.Description;
            m_buyButton.gameObject.SetActive(true);
            m_price.text = m_informations.GetPrice(0).Amount.ToString();
            OnMoneyChanged();
            if (m_currentUpgrade != null) {
                CurrencyAmount price = m_informations.GetPrice(m_currentUpgrade.CurrentLevel);
                m_currencyData = Main.Instance.GetManager<ProfileManager>().GetCurrencyData(price.CurrencyType);
                if (m_currentUpgrade.CurrentLevel != m_informations.MaxLevel && m_currencyData != null) {
                    m_buyLevelUp.text = m_state.HasUpgrade ? "Level up" : "Buy";
                    m_price.text = $"{price.Amount}{m_currencyData.Extension}";
                }
                m_levelSlider.value = (float)m_currentUpgrade.CurrentLevel / m_informations.MaxLevel;
                m_level.text = $"{m_currentUpgrade.CurrentLevel}/{m_informations.MaxLevel}";
                m_upgradeCurrentNext.text = m_currentUpgrade.GetNextAdvantageText();
            }
            m_upgradeCurrentNext.transform.gameObject.SetActive(m_state.HasUpgrade);
            m_levelSliderParent.gameObject.SetActive(m_state.HasUpgrade);
            if (m_state.MaxLevel) ChangeButtonToMaxLvl();
        } else {
            m_buyButton.gameObject.SetActive(false);
            m_levelSliderParent.gameObject.SetActive(false);
            m_icon.sprite = Main.Instance.GlobalConfig.UpgradeConfig.LockIcon;
            m_name.text = $"Upgrade Locked (Unlocked at lvl {m_informations.MinPlayerLevel})";
        }
    }

    private void OnPlayerLvlUp(int newLevel)
    {
        if (!m_state.IsAvailable && newLevel >= m_informations.MinPlayerLevel) {
            m_state.IsAvailable = true;
            Events.Upgrade.OnUpgradeBought += UpgradeBought;
            Events.Profile.OnLevelUp -= OnPlayerLvlUp;
            UpdateState();
        }
    }

    private void UpgradeLeveledUp(UpgradeType type, int newLevel)
    {
        if (!m_state.MaxLevel && type == m_informations.Type) {
            m_state.MaxLevel = newLevel == m_informations.MaxLevel;
            UpdateState();
        }
    }

    private void UpgradeBought(UpgradeType type)
    {
        if (m_state.IsAvailable && type == m_informations.Type) {
            m_state.HasUpgrade = true;
            Events.Upgrade.OnUpgradeBought -= UpgradeBought;
            Events.Upgrade.OnUpgradeLeveledUp += UpgradeLeveledUp;
            if (m_upgradeManager.HasUpgradeActive(type, out m_currentUpgrade)) {
                Events.Profile.OnMoneyChanged += OnMoneyChanged;
            }
            UpdateState();
        }
    }

    private void ChangeButtonToMaxLvl()
    {
        m_buyButton.gameObject.SetActive(false);
        m_maxLevel.SetActive(true);
        m_levelSliderParent.gameObject.SetActive(false);
        m_upgradeCurrentNext.transform.gameObject.SetActive(false);
        Events.Upgrade.OnUpgradeLeveledUp -= UpgradeLeveledUp;
    }
}
