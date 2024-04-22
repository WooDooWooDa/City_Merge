using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public enum TileType
{
    Empty,
    Grid,
    Road,
    Green,
    Lake,
    Aeroport,
    Count
}

[CreateAssetMenu(fileName = "NewTile", menuName = "Config/Object/Tile")]
public class TileInformations : ScriptableObject
{
    public string TileName;
    public string Description;
    public string Special;
    public Sprite sprite;
    public TileType Type;
    public GameObject[] TilePrefab;
    public bool isEditable = false;
    public int minLevel = 1;

    [Header("Value")]
    public CurrencyAmount BasePrice;
    public float PricePerPrevious;
    public CurrencyAmount ResellValue;
}
