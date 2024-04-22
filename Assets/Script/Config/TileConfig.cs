using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "TilesConfig", menuName = "Config/Tiles", order = 2)]
public class TileConfig : ScriptableObject
{
    [SerializeField] private GameObject m_emptyGrassTile;
    public GameObject EmptyTile => m_emptyGrassTile;
    [SerializeField] private TileInformations[] m_tilesInformations;
    public TileInformations[] TilesInformations => m_tilesInformations;

    public MaterialsPalette GrassPalette => m_grassPalette;
    [SerializeField] private MaterialsPalette m_grassPalette;
}
