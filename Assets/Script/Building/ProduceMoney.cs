using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct ProductionInterval
{
    public float Value;
    public float Interval;
    public Currency Currency;
}

public class ProduceMoney : MonoBehaviour
{
    [SerializeField] private CurrencyDisplayer m_displayer;

    private ProfileManager m_profile;

    private WaitForSeconds m_waitFor;
    private ProductionInterval m_production;
    public float History { get; set; }

    private void Start()
    {
        m_profile = Main.Instance.GetManager<ProfileManager>();
    }

    public void Display(CurrencyAmount currency)
    {
        m_displayer.SetData(currency);
    }

    public void SetData(ProductionInterval production)
    {
        m_production = production;
        m_waitFor = new WaitForSeconds(m_production.Interval);
    }

    private void OnEnable()
    {
        StartCoroutine(StartProducing());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator StartProducing()
    {
        while (true) {
            yield return m_waitFor;
            Produce();
        }
    }

    private void Produce()
    {
        float amountGained = m_production.Value; //* PlayerBonus.ProductionBonus;
        History += amountGained;
        CurrencyAmount currencyAmount = new CurrencyAmount() { Amount = amountGained, CurrencyType = m_production.Currency.Type };
        m_displayer.SetData(currencyAmount);
        m_profile.AddCurrency(currencyAmount);
    }
}
