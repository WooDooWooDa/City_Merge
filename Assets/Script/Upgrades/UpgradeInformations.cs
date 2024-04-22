using System;
using UnityEngine;

public enum UpgradeType
{
    IslandSize,
    FasterBuild,
    AutoMerger,
    AutoBuilder,
    OfflineCollect,
    Count
}

[Serializable]
public struct UpgradePrerequisite
{
    public UpgradeType Type;
    public int MinLevel;
}

[CreateAssetMenu(fileName = "NewUpgrade", menuName = "Config/Object/Upgrade")]
public class UpgradeInformations: ScriptableObject
{
    public string Name;
    public string Description;
    public Sprite Icon;
    public UpgradeType Type;
    public int MaxLevel;
    public int MinPlayerLevel = 1;
    public CurrencyAmount[] Price;
    public Upgrade UpgradePrefab;
    [Header("Action")]
    public bool HasUpdateActions = false;
    public bool CanBeToggled = false;
    [Space]
    public UpgradePrerequisite[] upgradesPrerequisite;

    public CurrencyAmount GetPrice(int level)
    {
        if (level == MaxLevel) return new CurrencyAmount { CurrencyType = CurrencyType.Count };
        return Price[level];
    }
}
