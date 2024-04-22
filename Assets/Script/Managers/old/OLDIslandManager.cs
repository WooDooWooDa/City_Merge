using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OLDIslandManager : MonoBehaviour
{
    [Header("Island")]
    [SerializeField]
    public Transform tileParent;
    [SerializeField]
    public Transform buildingsParent;

    [Header("Blocks")]
    [SerializeField]
    private GameObject grassBlock;
    [SerializeField]
    private GameObject gridTile;
    [SerializeField]
    private GameObject roadBlock;
    [SerializeField]
    private GameObject greenBlock;

    private GameManager gameManager;
    private Dictionary<TileType, GameObject> tilePrefabDic;

    void Awake()
    {
        tilePrefabDic = new Dictionary<TileType, GameObject>() {
            { TileType.Empty, grassBlock},
            { TileType.Grid, gridTile},
            { TileType.Road, roadBlock},
            { TileType.Green, null}
        };
    }

    private void Start()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
    }

    public void BuyTile(int tileValue)
    {
        GameObject newtile = tileValue switch
        {
            0 => gridTile,
            1 => greenBlock,
            2 => roadBlock,
            _ => null,
        };
        if (newtile == null) return;

        MoneyManager money = gameManager.GetComponent<MoneyManager>();
        float price = newtile.GetComponent<Tile>().TileInformations.BasePrice.Amount;
        if (money.CanBuy(price)) {
            GameObject tileToReplace = GetComponent<SelectTile>().GetSelected().gameObject;
            Vector3 pos = tileToReplace.transform.position;

            if (newtile != null) {
                Tile newTile = Instantiate(newtile, pos, Quaternion.identity, tileParent).GetComponent<Tile>();
                newTile.AnimateSpawn();
                Destroy(tileToReplace);
                GetComponent<SelectTile>().CancelSelection();
                money.RemoveMoney(price);
            }
        } else {
            FindObjectOfType<Alerter>().ShowAlert($"You don't have enough money to buy this tile. {price.ToString("0")} requiered", 2f);
        }
    }

    public void ReplaceTile(Tile tileToReplace, TileType newTileType)
    {
        Vector3 pos = tileToReplace.transform.position;
        if (tilePrefabDic.TryGetValue(newTileType, out GameObject tile)) {
            Tile newTile = Instantiate(tile, pos, Quaternion.identity, tileParent).GetComponent<Tile>();
            newTile.AnimateSpawn();
            Destroy(tileToReplace.gameObject);
        }
    }
}
