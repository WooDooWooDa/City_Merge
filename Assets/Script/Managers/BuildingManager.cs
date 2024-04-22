using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public partial class BuildingManager : Manager
{
    private static readonly string GO_NAME = "BuildingBase";
    public override string LoadingInfo => "Building your city";

    private BuildingsConfig m_config;

    private Transform m_buildingBaseGO;
    private Dictionary<BuildingKey, BuildingInformations> m_buildingsInformations = new Dictionary<BuildingKey, BuildingInformations>();
    private List<Building> m_currentBuildings = new List<Building>();

    private IslandManager m_islandManager;
    private ProfileManager m_profileManager;
    private CollectionManager m_collection;

    public override void Initialize()
    {
        base.Initialize();

        m_buildingBaseGO = new GameObject(GO_NAME).transform;
        m_config = Main.Instance.GlobalConfig.BuildingsConfig;
        //buildings dictionnary initialization
        if (m_config.BuildingInformations != null) {
            foreach (var buildingInfo in m_config.BuildingInformations) {
                m_buildingsInformations.Add(new BuildingKey { Type = buildingInfo.BuildingType, Level = buildingInfo.Level }, buildingInfo);
            }
        }

        m_islandManager = Main.Instance.GetManager<IslandManager>();
        m_profileManager = Main.Instance.GetManager<ProfileManager>();
        m_collection = Main.Instance.GetManager<CollectionManager>();

        IsInitialize = true;
    }

    #region Build

    public bool TryBuyBuilding(BuildingKey key)
    {
        if (m_buildingsInformations.TryGetValue(key, out BuildingInformations info)) {
            debugger.Log("Trying to buy building : " + info.Name);
            if (m_profileManager.HasEnough(GetPriceOf(key))) {
                if (TryConstructBuilding(key)) {
                    m_profileManager.RemoveCurrency(GetPriceOf(key));
                    m_data.AddToBought(key);
                    Events.Building.FireBought(key);
                    return true;
                }
                return false;
            }
            debugger.Log("Not enough to buy this building");
        }
        return false;
    }

    public bool TryMerge(Building mergeTo, Building other)
    {
        if (!other.IsMergeable(mergeTo.BuildingInformations.Level)) return false;

        BuildingLevel newLevel = mergeTo.BuildingInformations.Level;
        ++newLevel;

        bool hasToPrestige = false;
        int nextPrestigeLevel = 0;
        if (newLevel == BuildingLevel.Count) {
            if (m_data.FirstTimePrestige) {
                m_data.FirstTimePrestige = false;
                Main.Instance.GetManager<ScreenManager>().OpenScreen(ScreenType.InfoPopup, new InformationPopupOpenInfo
                {
                    PopupInformations = Resources.Load<PopupInformations>("UI/Popup/PrestigeInfo")
                });
            }
            hasToPrestige = true;
            nextPrestigeLevel = mergeTo.PrestigeLevel + 1;
            newLevel = BuildingLevel.One;
        }

        Building newBuilding = BuildBuilding(m_islandManager.GetTileUnder(mergeTo), new BuildingKey()
        {
            Level = newLevel,
            Type = mergeTo.BuildingInformations.BuildingType
        });

        m_profileManager.AddExperiencePoint(mergeTo.BuildingInformations.MergeExp);
        newBuilding.Display(mergeTo.BuildingInformations.MergeExp);
        if (hasToPrestige) newBuilding.Prestige(nextPrestigeLevel);

        //Event
        Events.Building.FireMerge(mergeTo.BuildingInformations.Key);

        //remove from building list
        m_currentBuildings.Remove(mergeTo);
        m_currentBuildings.Remove(other);
        GameObject.Destroy(mergeTo.gameObject);
        GameObject.Destroy(other.gameObject);
        return true;
    }

    public bool TryConstructBuilding(BuildingKey buildingKey)
    {
        Tile gridTile = GetNextAvailableGridTile(m_islandManager.GetTilesOfType(TileType.Grid));
        if (gridTile) {
            if (m_buildingsInformations.TryGetValue(buildingKey, out BuildingInformations info)) {
                ConstructBuilding(gridTile, info);
                return true;
            }
            debugger.LogWarning($"Couldn't spawn a building of type {buildingKey.Type} and/or level {buildingKey.Level}");
            return false;
        }
        debugger.LogWarning("No space available to spawn a new building");
        return false;
    }

    private Building BuildBuilding(Tile tile, BuildingKey key, BuildingData data = null)
    {
        if (m_buildingsInformations.TryGetValue(key, out BuildingInformations info)) {
            Building newBuilding = GameObject.Instantiate(m_config.BuildingBase, tile.transform.position + Vector3.up, Helper.RandomRotation());
            newBuilding.Load(data, info);
            tile.PutDownBuilding(newBuilding);
            AddToBuildings(newBuilding);
            //Adding the building to the collection
            if (m_collection.AddToCollection(CollectionType.Building, (int)key.Level)) {
                debugger.Log("TODO : Popup - You have unlocked building level : " + key.Level);
                var openInfo = new InformationPopupOpenInfo
                {
                    PopupInformations = Resources.Load<PopupInformations>("UI/Popup/NewBuildingUnlockInfo"),
                    PositiveAction = () => Main.Instance.GetManager<ScreenManager>().CloseScreen(ScreenType.InfoPopup)
                };
                //
                openInfo.PopupInformations.Icon = info.sprite;
                //
                Main.Instance.GetManager<ScreenManager>().OpenScreen(ScreenType.InfoPopup, openInfo);
            }
            return newBuilding;
        } else {
            debugger.LogWarning($"Couldn't spawn a building of type {key.Type} and/or level {key.Level}");
            return null;
        }
    }

    private void ConstructBuilding(Tile tile, BuildingInformations info, bool start = true) 
    {
        Construction construction = GameObject.Instantiate(m_config.Construction, tile.transform.position + Vector3.up, Helper.RandomRotation(), m_buildingBaseGO);
        construction.SetBuildingToConstruct(tile, info);
        //add the construction to the tile so it is "occupied" and cant be spawned on
        tile.PutDownConstruction(construction);

        construction.OnBuild += BuildBuilding;

        if (start)
            construction.StartConstruction();
    }

    private void AddToBuildings(Building newBuilding)
    {
        m_currentBuildings.Add(newBuilding);
        newBuilding.transform.parent = m_buildingBaseGO;
    }

    private Tile GetNextAvailableGridTile(List<Tile> tiles)
    {
        //tiles.Randomize();
        return tiles.Find(x => !x.HasABuilding() && !x.HasAConstruction());
    }

    #endregion

    #region Production

    public float GetProductionPerSecond()
    {
        var total = 0f;
        foreach (var building in m_currentBuildings) {
            total += building.BuildingInformations.ProductionPerSecond;
        }
        return total;
    }

    public CurrencyAmount GetPriceOf(BuildingKey buildingKey)
    {
        if (m_buildingsInformations.TryGetValue(buildingKey, out BuildingInformations info)) {
            return info.Price * m_data.BuildingPerLevelBought[(int)buildingKey.Type, (int)buildingKey.Level];
        }
        return default;
    }

    public uint GetBoughtPerKey(BuildingKey buildingKey)
    {
        return m_data.BuildingPerLevelBought[(int)buildingKey.Type, (int)buildingKey.Level];
    }

#endregion
}