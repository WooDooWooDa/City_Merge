using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class GridElem : MonoBehaviour
{
    [SerializeField]
    public TileInformations TileInformations;
    [SerializeField]
    private TextMeshProUGUI titleText;
    [SerializeField]
    private TextMeshProUGUI priceText;
    [SerializeField]
    private TextMeshProUGUI unlockAtText;
    [SerializeField]
    private Image sprite;

    private void Start()
    {
        titleText.text = TileInformations.TileName;
        priceText.text = $"$ {TileInformations.BasePrice.Amount.ToString("0.00")}";
        if (TileInformations.sprite != null) sprite.sprite = TileInformations.sprite;
    }

    private void OnEnable()
    {
        InteractableTile(FindObjectOfType<PlayerManager>().playerData.level >= TileInformations.minLevel);
    }

    private void InteractableTile(bool state)
    {
        GetComponentInChildren<Button>().interactable = state;
        GetComponent<Image>().color = new Color(1, 1, 1, state ? 1 : 0.5f);
        sprite.color = new Color(1, 1, 1, state ? 1 : 0.5f);
        titleText.color = new Color(1, 1, 1, state ? 1 : 0.5f);
        priceText.color = new Color(1, 1, 1, state ? 1 : 0.5f);
        unlockAtText.text = state ? "": "Unlock at level " + TileInformations.minLevel;
    }
}
