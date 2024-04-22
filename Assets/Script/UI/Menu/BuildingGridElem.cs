using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingGridElem : MonoBehaviour
{
    [SerializeField]
    public BuildingInformations BuildingInformations;
    [SerializeField]
    private TextMeshProUGUI titleText;
    [SerializeField]
    private TextMeshProUGUI priceText;
    [SerializeField]
    private TextMeshProUGUI perSecText;
    [SerializeField]
    private TextMeshProUGUI unlockAtText;
    [SerializeField]
    private Image sprite;

    private PlayerManager playerManager;
    private OLDBuildingManager buildingManager;

    public void ClickTest()
    {
        Debug.Log("CLICK TEST");
    }

    private void Start()
    {
        playerManager = FindObjectOfType<PlayerManager>();
        buildingManager = FindObjectOfType<OLDBuildingManager>();

        titleText.text = BuildingInformations.Name;
        priceText.text = $"$ {(BuildingInformations.Price.Amount * (playerManager.playerData.nbBoughtPerLevel[(int)BuildingInformations.Level] + 1)).ToString("0.00")}";
        perSecText.text = $"$ {BuildingInformations.Price.Amount.ToString("0.00")} / SEC";
        GetComponentInChildren<Button>().onClick.AddListener(BuyTile);
        if (BuildingInformations.sprite != null) sprite.sprite = BuildingInformations.sprite;
    }

    private void OnEnable()
    {
        playerManager = FindObjectOfType<PlayerManager>();
        InteractableTile(playerManager.playerData.maxBuildingLevel >= (int)BuildingInformations.Level + 1);
    }

    private void BuyTile()
    {
        Debug.Log("CLICK");
        buildingManager.BuyBuilding((int)BuildingInformations.Level);
    }

    private void InteractableTile(bool state)
    {
        GetComponentInChildren<Button>().interactable = state;
        GetComponent<Image>().color = new Color(1, 1, 1, state ? 1 : 0.5f);
        sprite.color = new Color(1, 1, 1, state ? 1 : 0.5f);
        titleText.color = new Color(1, 1, 1, state ? 1 : 0.5f);
        priceText.color = new Color(1, 1, 1, state ? 1 : 0.5f);
        perSecText.color = new Color(1, 1, 1, state ? 1 : 0.5f);
        unlockAtText.text = state ? "" : "Unlock at building level " + (BuildingInformations.Level + 1);
    }
}
