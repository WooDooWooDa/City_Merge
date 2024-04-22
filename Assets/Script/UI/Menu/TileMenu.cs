using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class TileMenu : MonoBehaviour
{
    [SerializeField]
    private LayerMask UILayer;

    [Header("UI")]
    [SerializeField]
    private TextMeshProUGUI tileName;
    [SerializeField]
    private TextMeshProUGUI description;
    [SerializeField]
    private TextMeshProUGUI sellPriceText;

    public void SetInformations(TileInformations tile)
    {
        tileName.text = tile.TileName;
        description.text = tile.Description;
        sellPriceText.text = $"{tile.ResellValue.Amount.ToString("0.00")} $";
    }
}
