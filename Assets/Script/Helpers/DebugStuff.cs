using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DebugStuff : MonoBehaviour
{
    private Dictionary<string, Action> actions = new Dictionary<string, Action>();

    private void Start()
    {
        actions.Add(nameof(Add1000Dollar), Add1000Dollar);
        actions.Add(nameof(OpenInfoScreen), OpenInfoScreen);
        actions.Add(nameof(BuyIslandSizeUpgrade), BuyIslandSizeUpgrade);
    }

    public void BuyIslandSizeUpgrade()
    {
        Main.Instance.GetManager<UpgradeManager>().TryBuyUpgrade(UpgradeType.IslandSize);
    }

    public void Add1000Dollar()
    {
        Main.Instance.GetManager<ProfileManager>().AddCurrency(new CurrencyAmount() { Amount = 1000, CurrencyType = CurrencyType.Dollar });
    }

    public void GetAvailableTiles()
    {
        List<Tile> tiles = Main.Instance.GetManager<IslandManager>().GetTilesOfType(TileType.Grid);
        foreach (var tile in tiles) {
            if (!tile.HasABuilding()) {
                Debug.Log(tile + " is available");
            } else {
                Debug.Log(tile + " is NOT available");
            }
        }
    }

    public void OpenInfoScreen()
    {
        Main.Instance.GetManager<ScreenManager>().OpenScreen(ScreenType.InfoPopup, new InformationPopupOpenInfo() {
            PopupInformations = Resources.Load<PopupInformations>("UI/Popup/PrestigeInfo"),
            PositiveAction = Add1000Dollar
        });
    }

    private void OnGUI()
    {
        var i = 0;
        foreach (var action in actions) {
            if (GUI.Button(new Rect(10, 10 + (50 * i), 150, 30), action.Key))
                action.Value?.Invoke();
            i++;
        }
    }
}
 
