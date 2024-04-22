using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public partial class BuildingManager : Manager
{
    private BuildingSaveData m_data = new BuildingSaveData();
    public BuildingSaveData Data { get { return m_data; } set { m_data = value; } }

    public override bool Load(ref SaveData data)
    {
        m_data = data.BuildingSaveData;

        //tiles gameobject initialization
        IslandManager island = Main.Instance.GetManager<IslandManager>();
        foreach (BuildingData buildingData in m_data.buildingDatas) {
            BuildBuilding(island.GetTileAt(buildingData.Position + Vector3.down), 
                new BuildingKey { Type = buildingData.Type, Level = buildingData.Level }, buildingData);
        }

        return true;
    }

    public override bool Save(ref SaveData data)
    {
        m_data.buildingDatas = new BuildingData[m_currentBuildings.Count];
        for (int i = 0; i < m_currentBuildings.Count; i++) {
            m_data.buildingDatas[i] = new BuildingData();
            m_currentBuildings[i].Save(ref m_data.buildingDatas[i]);
        }

        data.BuildingSaveData = m_data;
        return true;
    }
}

[System.Serializable]
public class BuildingSaveData
{
    public bool FirstTimePrestige = true;
    //TODO move this parameter to an upgrade
    public int FreeBuildingCharge = 10;

    public BuildingData[] buildingDatas = new BuildingData[] { };
    public uint[,] BuildingPerLevelBought = new uint[(int)BuildingType.Count, (int)BuildingLevel.Count];

    public void AddToBought(BuildingKey key)
    {
        BuildingPerLevelBought[(int)key.Type, (int)key.Level]++;
    }
}

[System.Serializable]
public class BuildingData
{
    public ushort Prestige = 0;
    public BuildingLevel Level;
    public BuildingType Type;

    public Vector3 Position;
    public Quaternion Rotation;

    public float CurrentProductionInterval; 
    public float ProductionHistory;
}
