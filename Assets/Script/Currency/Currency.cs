using System;
using UnityEngine;

public enum CurrencyType
{
    Exp,
    Dollar,
    Premium, // gems?
    Count
}

[Serializable]
public struct CurrencyAmount
{
    //TODO change float to BigInteger
    public float Amount;
    public CurrencyType CurrencyType;

    public static CurrencyAmount operator +(CurrencyAmount n1, float n2)
    {
        n1.Amount += n2;
        return n1;
    }

    public static CurrencyAmount operator *(CurrencyAmount n1, float n2)
    {
        if (n2 != 0) n1.Amount *= n2;
        return n1;
    }
}

[CreateAssetMenu(fileName = "NewCurrency", menuName = "Config/Object/Currency")]
public class Currency : ScriptableObject
{
    public string Extension;
    public CurrencyType Type;
    public Sprite Sprite;
    public Color Color;
}