
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class ProfileManager : Manager
{
    public override string LoadingInfo => "Preparing your profile";

    #region Initialization

    public override void Initialize()
    {
        base.Initialize();

        Currency[] currencies = Resources.LoadAll<Currency>("Currencies");
        foreach (var currency in currencies) {
            m_currenciesData.Add(currency.Type, currency);
        }

        Events.Loading.OnLoadingPageClose += TryOnBoarding;
        IsInitialize = true;
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        Events.Loading.OnLoadingPageClose -= TryOnBoarding;
    }

    public static string RandomString(int length)
    {
        System.Random random = new System.Random();
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    #endregion

    #region OnBoarding

    public void TryOnBoarding()
    {
        if (!Data.OnBoardingCompleted) {
            Debug.LogWarning("Start OnBoarding sequence...");
        }
    }

    #endregion

    #region Currency

    private Dictionary<CurrencyType, Currency> m_currenciesData = new Dictionary<CurrencyType, Currency>();
    public Currency GetCurrencyData(CurrencyType type) {
        if (type == CurrencyType.Count) return null;
        return m_currenciesData[type];
    }

    public CurrencyAmount GetCurrencyOf(CurrencyType type)
    {
        if (type == CurrencyType.Count) return default;
        return m_data.Currencies[(int)type];
    }

    public bool HasEnough(CurrencyAmount value)
    {
        if (value.CurrencyType == CurrencyType.Count) return false;
        return m_data.Currencies[(int)value.CurrencyType].Amount >= value.Amount;
    }

    public void AddCurrency(CurrencyAmount received)
    {
        m_data.Currencies[(int)received.CurrencyType].Amount += received.Amount;
        if (received.CurrencyType == CurrencyType.Dollar) 
            Events.Profile.FireMoneyChanged();
    }

    public void RemoveCurrency(CurrencyAmount removed)
    {
        m_data.Currencies[(int)removed.CurrencyType].Amount = Mathf.Max(0, m_data.Currencies[(int)removed.CurrencyType].Amount - removed.Amount);
        if (removed.CurrencyType == CurrencyType.Dollar)
            Events.Profile.FireMoneyChanged();
    }

    #endregion

    #region Experience
    public int PlayerLevel => m_data.Level;
    public int NextLevelExp => Main.Instance.GlobalConfig.ProfileConfig.NextLevel[PlayerLevel - 1];
    public float ExpPoint => GetCurrencyOf(CurrencyType.Exp).Amount;

    public void AddExperiencePoint(CurrencyAmount exp)
    {
        if (exp.CurrencyType != CurrencyType.Exp) return;

        AddCurrency(exp);
        //TODO Change the nextLevel to a ratio based on the level
        if (ExpPoint >= Main.Instance.GlobalConfig.ProfileConfig.NextLevel[PlayerLevel - 1]) {
            m_data.Currencies[(int)CurrencyType.Exp].Amount = 0;
            m_data.Level++;
            Events.Profile.FireLevelUp(PlayerLevel);
        }
    }
    #endregion
}
