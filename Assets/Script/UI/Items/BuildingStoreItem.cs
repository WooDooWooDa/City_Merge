using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BuildingStoreItem : MonoBehaviour
{
    [SerializeField] private Image m_icon;
    [SerializeField] private TextMeshProUGUI m_name;
    [SerializeField] private TextMeshProUGUI m_price;
    [SerializeField] private TextMeshProUGUI m_production;
    [SerializeField] private TextMeshProUGUI m_history;
    [SerializeField] private Button m_buyButton;

    private BuildingInformations m_informations;
    private CurrencyAmount m_currentPrice;

    private ProfileManager m_profile;
    private BuildingManager m_buildingManager;

    public void OnInitialize(BuildingManager manager, BuildingInformations info, Action clickAction)
    {
        if (info == null) return;

        m_informations = info;
        m_buildingManager = manager;

        if (clickAction != null) {
            m_buyButton.onClick.AddListener(new UnityAction(clickAction));
        }

        m_name.text = m_informations.Name;
        m_icon.sprite = m_informations.sprite;
        m_currentPrice = m_buildingManager.GetPriceOf(m_informations.Key);
        m_production.text = $"{m_informations.Production.Value} {m_informations.Production.Currency.Extension} " +
            $"/ {m_informations.Production.Interval} sec";

        //move this to event onNew
        uint bought = m_buildingManager.GetBoughtPerKey(m_informations.Key);
        m_history.transform.gameObject.SetActive(bought > 0);
        m_history.text = $"{bought} Bought";

        m_profile = Main.Instance.GetManager<ProfileManager>();
        Events.Profile.OnMoneyChanged += OnMoneyChanged;
        Events.Building.OnBought += OnBought;

        Events.Collection.OnNewInCollection += OnNew;

        OnMoneyChanged();
        UpdateState();
    }

    private void OnNew(CollectionType collection, int newValue)
    {

    }

    private void OnBought(BuildingKey key)
    {
        m_currentPrice = m_buildingManager.GetPriceOf(m_informations.Key);
        if (m_informations.Key == key)
            m_history.text = $"{m_buildingManager.GetBoughtPerKey(m_informations.Key)} Bought";

        UpdateState();
    }

    private void UpdateState()
    {
        m_price.text = $"{m_currentPrice.Amount}$";
    }

    private void OnMoneyChanged()
    {
        if (m_buyButton != null)
            m_buyButton.interactable = m_profile.HasEnough(m_currentPrice);
    }
}
