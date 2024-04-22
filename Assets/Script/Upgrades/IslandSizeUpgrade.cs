using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class IslandSizeUpgrade: Upgrade
{
    private IslandManager m_islandManager;

    private Vector2 m_startingSize;

    public override void Initialize(UpgradeManager upgradeManager)
    {
        base.Initialize(upgradeManager);

        m_islandManager = Main.Instance.GetManager<IslandManager>();
        m_startingSize = Main.Instance.GlobalConfig.IslandConfig.StartingSize;

        //TODO change the way those adv are calculated, doesnt work
        currentAdv = $"{Mathf.Pow(m_startingSize.x + m_islandManager.Data.Level, 2)}";
        nextAdv = $"{Mathf.Pow(m_startingSize.x + m_islandManager.Data.Level + 1, 2)} tiles";
    }

    public override void LevelUp()
    {
        if (m_islandManager.Data.Level >= Main.Instance.GlobalConfig.IslandConfig.MaxIslandLevel
            || m_islandManager.Data.Level >= m_informations.MaxLevel) {
            Debug.Log("Your island has reach max level");
            //Alerter.ShowMessage("Your island has reach the maximum level");
            return;
        }

        UpgradeIsland();
        m_islandManager.Data.Level++;

        currentAdv = $"{Mathf.Pow(m_startingSize.x + m_islandManager.Data.Level, 2)}";
        nextAdv = $"{Mathf.Pow(m_startingSize.x + m_islandManager.Data.Level + 1, 2)} tiles";

        base.LevelUp();
    }

    public override void OnUpdate()
    {
    }

    protected override bool CheckIntegrity()
    {
        return true;
    }

    protected override void OnToggled() { }

    private void UpgradeIsland()
    {
        

        uint top = (uint)m_startingSize.x + (2 * m_islandManager.Data.Level + 1);   //3 + 2, 3 + 4, 3 + 6
        for (int i = -(int)(top / 2); i <= (int)(top / 2); i++) {
            m_islandManager.SpawnTile(TileType.Empty, new TileData()
            {
                Position = new Vector3(i, 0, (int)(top / 2))
            });
        }

        uint mid = (uint)m_startingSize.x + (2 * (m_islandManager.Data.Level));
        for (int i = -(int)(mid / 2); i <= (int)(mid / 2); i++) {
            m_islandManager.SpawnTile(TileType.Empty, new TileData()
            {
                Position = new Vector3((int)(top / 2), 0, i)
            });
            m_islandManager.SpawnTile(TileType.Empty, new TileData()
            {
                Position = new Vector3(-(int)(top / 2), 0, i)
            });
        }

        uint bot = (uint)m_startingSize.x + (2 * m_islandManager.Data.Level + 1);   //3 + 2, etc
        for (int i = -(int)(bot / 2); i <= (int)(bot / 2); i++) {
            m_islandManager.SpawnTile(TileType.Empty, new TileData()
            {
                Position = new Vector3(i, 0, -(int)(bot / 2))
            });
        }
    }
}
