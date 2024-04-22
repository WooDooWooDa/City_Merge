using UnityEngine;

[CreateAssetMenu(fileName = "GlobalConfig", menuName = "Config/Global", order = 0)]
public class GlobalConfig : ScriptableObject
{
    public string SaveName = "";

    [Header("Configs")]
    public ProfileConfig ProfileConfig;
    public IslandConfig IslandConfig;
    public BuildingsConfig BuildingsConfig;
    public TileConfig TileConfig;
    public UpgradeConfig UpgradeConfig;
    public ScreenConfig ScreenConfig;
    
    [Header("Definitions")]
    public LayerMask TileLayers;
}