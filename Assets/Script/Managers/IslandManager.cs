using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public partial class IslandManager : Manager
{
    private static readonly string GO_NAME = "IslandBase";
    public override string LoadingInfo => "Preparing the island";

    private Transform m_islandBaseGO;
    private Transform m_centerGO;
    private SelectTile m_selectedTile = null;
    public Transform Center => m_centerGO;
    private Dictionary<TileType, TileInformations> m_tilesInformations = new Dictionary<TileType, TileInformations>();
    private List<Tile> m_currentTiles = new List<Tile>();

#region Initialization
    public override void Initialize()
    {
        base.Initialize();

        m_islandBaseGO = new GameObject(GO_NAME).transform;
        m_islandBaseGO.gameObject.AddComponent<ScrollAndPinch>();
        m_selectedTile = m_islandBaseGO.gameObject.AddComponent<SelectTile>();
        m_centerGO = new GameObject("IslandCenter").transform;
        m_centerGO.transform.parent = m_islandBaseGO;
        m_centerGO.transform.position = new Vector3(0, 1, 0);

        //tile dictionnary initialization
        if (Main.Instance.GlobalConfig.TileConfig.TilesInformations != null) {
            foreach (var tileInfo in Main.Instance.GlobalConfig.TileConfig.TilesInformations) {
                m_tilesInformations.Add(tileInfo.Type, tileInfo);
            }
        }

        IsInitialize = true;
    }
#endregion

#region Upgrades
    public CurrencyAmount GetPriceOf(TileType tileType)
    {
        if (m_tilesInformations.TryGetValue(tileType, out var informations)) {
            return informations.BasePrice + (m_data.TilePerTypeBought[(int)tileType] * informations.PricePerPrevious);
        }
        return default;
    }
    #endregion

#region Tiles

    public bool TryBuyTile(TileType tileType, out string errorMsg)
    {
        errorMsg = "";
        if (m_tilesInformations.TryGetValue(tileType, out var informations)) {
            var manager = Main.Instance.GetManager<ProfileManager>();
            CurrencyAmount amount = GetPriceOf(tileType);
            if (manager.HasEnough(amount)) {
                manager.RemoveCurrency(amount);
                if (m_tilesInformations.TryGetValue(informations.Type, out var newInformations)) {
                    m_selectedTile.GetSelected().ChangeTileType(newInformations);
                    m_data.TilePerTypeBought[(int)tileType]++;
                    return true;
                }
            }
            errorMsg = $"You don't have enough X to buy this tile.";
        }
        return false;
    }

    public bool TrySellTile(out string errorMsg)
    {
        errorMsg = "";
        if (m_selectedTile.GetSelected() != null) {
            Tile tile = m_selectedTile.GetSelected();
            if (tile.HasABuilding() || tile.HasAConstruction()) {
                errorMsg = "This tile has a building on it, you cannot currently sell it.";
                return false;
            }
            Main.Instance.GetManager<ProfileManager>().AddCurrency(tile.TileInformations.ResellValue);
            if (m_tilesInformations.TryGetValue(TileType.Empty, out var informations)) {
                tile.ChangeTileType(informations);
            }
            return true;
        }
        errorMsg = "An error happened, unable to sell thius tile just yet.";
        return false;
    }

    /// <summary>
    ///  Return true if the building was on another tile before
    /// </summary>
    public bool MoveObjectTo(Tile tile, Building building)
    {

        Tile tileFrom = GetTileUnder(building);
        tile.PutDownBuilding(building);
        if (tileFrom != null) {
            tileFrom.PutDownBuilding(null);
            return true;
        }
        return false;
    }

    public Tile GetTileUnder(Building building)
    {
        return m_currentTiles.Find(x => x.BuildingOnTile == building);
    }

    public Tile GetTileAt(Vector3 pos)
    {
        return m_currentTiles.Find(x => x.transform.position == pos);
    }

    public List<Tile> GetTilesOfType(TileType tileType)
    {
        return m_currentTiles.FindAll(x => x.TileInformations.Type == tileType);
    }

    public Tile SpawnTile(TileType tileType, TileData data = default)
    {
        if (m_tilesInformations.TryGetValue(tileType, out TileInformations tileInfo)) {
            Tile newtile = GameObject.Instantiate(Main.Instance.GlobalConfig.TileConfig.EmptyTile, m_islandBaseGO).GetComponent<Tile>();
            newtile.Load(data, tileInfo);
            //Main.Instance.GlobalConfig.TileConfig.GrassPalette.ChangeMaterialAt(newtile.GetComponentInChildren<MeshRenderer>(), 
            //data.Position.x * -1, data.Position.y - 1);
            m_currentTiles.Add(newtile);
            return newtile;
        }
        return null;
    }

#endregion Tiles

}
