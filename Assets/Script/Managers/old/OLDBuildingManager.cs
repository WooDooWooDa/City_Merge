using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OLDBuildingManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> BuildingsPrefabs;
    [SerializeField]
    private GameObject ConstructionPrefab;

    private OLDIslandManager island;
    private PlayerManager playerManager;

    private void Start()
    {
        island = FindObjectOfType<OLDIslandManager>();
        playerManager = GetComponent<PlayerManager>();
        //loadBuildingData
    }

    public float GetProductionPerSec()
    {
        Building[] buildings = FindObjectsOfType<Building>();
        float perSec = 0;
        foreach (Building building in buildings) {
            perSec += building.BuildingInformations.ProductionPerSecond;
        }
        return perSec;
    }

    public void QuickBuy()
    {
        BuyBuilding(1);
    }

    public void BuyBuilding(int buildingLevel)
    {
        MoneyManager money = GetComponent<MoneyManager>();
        float price = BuildingsPrefabs[buildingLevel].GetComponent<Building>().BuildingInformations.Price.Amount;
        if (!money.CanBuy(price)) {
            FindObjectOfType<Alerter>().ShowAlert($"You don't have enough money to a new building. {price.ToString("0")} required", 2f);
            return;
        }

        Tile[] tiles = island.GetComponentsInChildren<Tile>();
        List<Tile> gridTiles = new List<Tile>();
        foreach (var tile in tiles) {
            if (tile.TileInformations.Type == TileType.Grid) gridTiles.Add(tile);
        }
        Tile emptyTile = null;
        do {
            int randInd = Random.Range(0, gridTiles.Count);
            if (!gridTiles[randInd].HasABuilding()) {
                emptyTile = gridTiles[randInd];
            } else {
                gridTiles.Remove(gridTiles[randInd]);
            }
        } while (emptyTile == null && gridTiles.Count > 0);

        if (emptyTile != null) {
            //Construction construction = Instantiate(ConstructionPrefab, emptyTile.transform.position + Vector3.up, Quaternion.identity, island.buildingsParent).GetComponent<Construction>();
            //construction.SetBuildingToConstruct(BuildingsPrefabs[buildingLevel]);
            money.RemoveMoney(price);
            playerManager.playerData.nbBoughtPerLevel[(int)BuildingsPrefabs[buildingLevel].GetComponent<Building>().BuildingInformations.Level]++;
        } else {
            FindObjectOfType<Alerter>().ShowAlert("Not enough room to place the building. Merge some or buy new grid tile!", 2f);
        }
    }
}
