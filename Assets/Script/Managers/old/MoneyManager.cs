using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class MoneyManager : MonoBehaviour
{
    private float gameMoney;

    private void Awake()
    {
        //loadMoneyData;
        gameMoney = 10000000;
    }

    public float GetMoney()
    {
        return gameMoney;
    }

    public void AddMoney(float amount)
    {
        gameMoney += amount;
    }

    public bool CanBuy(float request)
    {
        return request <= gameMoney;
    }

    public void RemoveMoney(float amount)
    {
        gameMoney -= amount;
    }
}
