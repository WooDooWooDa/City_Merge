using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TileInfoOpenInfo: OpenInfo
{
    public Tile SelectedTile;
    public TileInformations TileInformations;
}

public class TileInfoScreen : UIScreenBase
{
    [Header("Specific")]
    [SerializeField] private TextMeshProUGUI m_tileTitleText;
    [SerializeField] private TextMeshProUGUI m_tileDescriptionText;
    [SerializeField] private Image m_tileSprite;
    [SerializeField] private Button m_button;

    private TileInformations m_currentTileInformations;

    public override UIScreenBase Open(OpenInfo openInfo)
    {
        base.Open(openInfo);

        var data = openInfo as TileInfoOpenInfo;
        if (data is TileInfoOpenInfo) {
            m_tileTitleText.text = data.TileInformations.TileName;
            m_tileDescriptionText.text = data.TileInformations.Description;
            m_tileSprite.sprite = data.TileInformations.sprite;

            m_button.gameObject.SetActive(data.TileInformations.Type != TileType.Empty);
            m_button.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = GetSellPriceString(data.TileInformations.ResellValue);
            m_button.onClick.AddListener(SellTile);
            m_currentTileInformations = data.TileInformations;
        }

        return this;
    }

    private void SellTile()
    {
        if (!Main.Instance.GetManager<IslandManager>().TrySellTile(out string errorMsg)) {
            //Show error message
            Debug.LogWarning(errorMsg);
        }
        this.Close();
    }

    private string GetSellPriceString(CurrencyAmount price)
    {
        //Get sell price with bonuses from island manager
        return $"Sell ({price.Amount})";
    }
}
