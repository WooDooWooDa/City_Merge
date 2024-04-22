using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuildingLevel
{
    One,
    Two,
    Three,
    Four,
    Five,
    Six,
    Seven,
    Eigth,
    Nine,
    Ten,
    Eleven,
    Count
}

public enum BuildingType
{
    Commercial,
    Suburban,
    Count
}

[Serializable]
public struct BuildingKey
{
    public BuildingType Type;
    public BuildingLevel Level;

    public static bool operator ==(BuildingKey x, BuildingKey y)
    {
        return x.Type == y.Type && x.Level == y.Level;
    }

    public static bool operator !=(BuildingKey x, BuildingKey y)
    {
        return x.Type != y.Type || x.Level != y.Level;
    }
}

[CreateAssetMenu(fileName = "NewBuilding", menuName = "Config/Object/Building")]
public class BuildingInformations : ScriptableObject
{
    [Header("Informations")]
    public string Name;
    public Sprite sprite;

    [Header("Building")]
    public BuildingKey BuildingPrerequisite;
    public BuildingLevel Level;
    public BuildingType BuildingType;
    public GameObject Model;
    public int StageToBuild = 2;

    [Header("Production")]
    public ProductionInterval Production;

    [Header("Values")]
    public CurrencyAmount MergeExp;
    public CurrencyAmount Price;
    public CurrencyAmount BaseResellValue;
    public float maxResellValueModifier = 1; //+100%

#region Function Or Parameters

    public float ProductionPerSecond => (Production.Value / Production.Interval);

    public BuildingKey Key => new BuildingKey { Type = BuildingType, Level = Level };
#endregion

}
