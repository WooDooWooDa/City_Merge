using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class BuyTileItem : MonoBehaviour
{
    [SerializeField] private Image m_tileImage;
    [SerializeField] private TextMeshProUGUI m_tileName;
    [SerializeField] private TextMeshProUGUI m_tilePrice;
    [SerializeField] private Button m_button;

    private TileInformations m_currentTileInformations;
    private ProfileManager m_profile;
    private IslandManager m_island;

    public void OnInitialize(TileInformations tile, UnityAction clickAction)
    {
        if (tile != null) {
            m_tileImage.sprite = tile.sprite;
            m_tileName.text = tile.TileName;
            m_currentTileInformations = tile;
        }

        if (clickAction != null) {
            m_button.onClick.AddListener(clickAction);
        }

        m_profile = Main.Instance.GetManager<ProfileManager>();
        m_island = Main.Instance.GetManager<IslandManager>();
    }

    private void Update()
    {
        m_button.interactable = m_currentTileInformations.minLevel <= m_profile.PlayerLevel &&
            m_profile.HasEnough(m_island.GetPriceOf(m_currentTileInformations.Type));

        m_tilePrice.text = m_island.GetPriceOf(m_currentTileInformations.Type).Amount.ToString();
    }
}
