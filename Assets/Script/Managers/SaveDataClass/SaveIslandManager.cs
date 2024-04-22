using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public partial class IslandManager : Manager
{
    private IslandSaveData m_data = new IslandSaveData();
    public IslandSaveData Data => m_data;

    public override void FirstInitialization()
    {
        base.FirstInitialization();

        Vector2 startingSize = Main.Instance.GlobalConfig.IslandConfig.StartingSize;
        for (int i = -(int)(startingSize.x / 3); i <= (int)(startingSize.x / 3); i++) {
            for (int j = -(int)(startingSize.y / 3); j <= (int)(startingSize.y / 3); j++) {
                SpawnTile(TileType.Empty, new TileData()
                {
                    Position = new Vector3(i, 0, j)
                });
            }
        }

        m_data.FirstTime = false;
    }

    public override bool Load(ref SaveData data)
    {
        m_data = data.IslandSaveData;

        if (m_data.FirstTime) {
            FirstInitialization();
        } else {
            foreach (TileData tileData in m_data.tileDatas) {
                SpawnTile(tileData.Type, tileData);
            }
        }

        return true;
    }

    public override bool Save(ref SaveData data)
    {
        m_data.tileDatas = new TileData[m_currentTiles.Count];
        for (int i = 0; i < m_currentTiles.Count; i++) {
            m_data.tileDatas[i] = new TileData();
            m_currentTiles[i].Save(ref m_data.tileDatas[i]);
        }

        data.IslandSaveData = m_data;
        return true;
    }
}

[System.Serializable]
public class IslandSaveData
{
    public bool FirstTime = true;

    public uint Level = 0;
    public ushort Prestige = 0; 
    public TileData[] tileDatas = new TileData[] { };
    public uint[] TilePerTypeBought = new uint[(int)TileType.Count];
}

[System.Serializable]
public class TileData
{
    public Vector3 Position = Vector3.zero;
    public TileType Type;
    public int CurrentPrefabIndex = 0;
    public int GrassTypeIndex = 0;
}
