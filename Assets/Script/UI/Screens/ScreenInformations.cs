using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public enum ScreenType
{
    Loading,
    Main,
    BuyTile,
    TileInfo,
    Temp1,
    BuildingStore,
    UpgradeStore,
    InfoPopup,
    Unlock,
    CollectionMenu,
    Collection,
    Statistic,
    Trophy
}

[CreateAssetMenu(fileName = "NewScreen", menuName = "Config/Screen/ScreenInfo")]
public class ScreenInformations : ScriptableObject
{
    public ScreenType ScreenType;
    public ScreenLayer ScreenLayer;
    [SerializeField] private UIScreenBase m_screen;
    public UIScreenBase Screen => m_screen;
    [Space]
    public bool StaticScreen = false;
    public ScreenAnimationBase m_screenAnimation = null;

}
