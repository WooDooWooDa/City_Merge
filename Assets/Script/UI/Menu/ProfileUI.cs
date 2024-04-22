using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProfileUI : MonoBehaviour
{
    [Header("Money")]
    [SerializeField]
    private TextMeshProUGUI moneyText;
    [SerializeField]
    private TextMeshProUGUI perSecText;

    [Header("Level")]
    [SerializeField]
    private TextMeshProUGUI levelText;
    [SerializeField]
    private Slider levelSlider;

    private ProfileManager profile;
    private BuildingManager buildingManager;

    private float lastUpdatedMoney;
    private int lastUpdatedLevel;
    private float lastUpdatedExp;
    private float lastUpdatedMaxExp;

    void Start()
    {
        profile = Main.Instance.GetManager<ProfileManager>();
        buildingManager = Main.Instance.GetManager<BuildingManager>();
    }

    void Update()
    {
        UpdateMoney();
        UpdateLevel();
        UpdateExp();
    }

    private void UpdateMoney()
    {
        CurrencyAmount dollars = profile.GetCurrencyOf(CurrencyType.Dollar);
        if (lastUpdatedMoney != dollars.Amount) {
            moneyText.text = $"$ {dollars.Amount}";
            perSecText.text = $"$ {buildingManager.GetProductionPerSecond()} / SEC";
            lastUpdatedMoney = dollars.Amount;
        }
    }

    private void UpdateLevel()
    {
        int level = profile.PlayerLevel;
        if (lastUpdatedLevel != level) {
            levelText.text = level.ToString();
            lastUpdatedLevel = level;
        }
    }

    private void UpdateExp()
    {
        var expPoints = profile.ExpPoint;
        if (lastUpdatedExp != expPoints) {
            levelSlider.value = expPoints;
            lastUpdatedExp = expPoints;
        }
        var maxExpPoint = profile.NextLevelExp;
        if (lastUpdatedMaxExp != maxExpPoint) {
            levelSlider.maxValue = maxExpPoint;
            lastUpdatedMaxExp = maxExpPoint;
        }
    }

}
