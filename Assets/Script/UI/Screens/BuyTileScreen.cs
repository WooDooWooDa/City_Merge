using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyTileScreen : UIScreenBase
{
    [Header("Specific")]
    [SerializeField] private BuyTileItem m_buyTileItemPrefab;
    [Space]
    [SerializeField] private Transform m_optionsListContent;

    protected override void Awake()
    {
        base.Awake();

        InitializeTileList();
    }

    private void InitializeTileList()
    {
        TileInformations[] tiles = Main.Instance.GlobalConfig.TileConfig.TilesInformations;
        foreach (var tile in tiles) {
            if (tile.Type != TileType.Empty) {
                BuyTileItem item = Instantiate(m_buyTileItemPrefab, m_optionsListContent);
                item.OnInitialize(tile, () => BuyTile(tile.Type));
            }
        }
    }

    private void BuyTile(TileType tileType)
    {
        if (!Main.Instance.GetManager<IslandManager>().TryBuyTile(tileType, out string errorMsg)) {
            //Show error message
            Debug.LogWarning(errorMsg);
        }
        this.Close();
    }
}
