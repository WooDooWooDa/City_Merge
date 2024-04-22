using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class BuildingStoreOpenInfo : OpenInfo
{

}

public class BuildingStoreScreen : UIScreenBase
{
    [Header("Specific")]
    [SerializeField] private BuildingStoreItem m_storeItem;
    [SerializeField] private Transform m_listParent;

    private BuildingManager m_buildingManager;

    public override UIScreenBase Open(OpenInfo openInfo)
    {
        base.Open(openInfo);

        var data = openInfo as BuildingStoreOpenInfo;
        if (data is BuildingStoreOpenInfo) {

        }

        m_buildingManager = Main.Instance.GetManager<BuildingManager>();
        InitializeBuildingList();

        return this;
    }

    private void InitializeBuildingList()
    {
        BuildingInformations[] buildings = Main.Instance.GlobalConfig.BuildingsConfig.BuildingInformations;
        foreach (var building in buildings) {
            BuildingStoreItem item = Instantiate(m_storeItem, m_listParent);
            item.OnInitialize(m_buildingManager, building, () => m_buildingManager.TryBuyBuilding(building.Key));
        }
    }
}
