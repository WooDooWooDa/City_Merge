using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectTile : MonoBehaviour
{
    private Tile selectedTile;
    private InputManager m_inputManager = null;
    private ScreenManager m_screenManager = null;

    private void Start()
    {
        m_screenManager = Main.Instance.GetManager<ScreenManager>();
    }

    private void OnDestroy()
    {
        if (m_inputManager != null)
            m_inputManager.OnTap -= CheckSelection;
    }

    private void Update()
    {
        if (m_inputManager == null && Main.Instance.IsInitialized) {
            m_inputManager = Main.Instance.GetManager<InputManager>();
            if (m_inputManager != null)
                m_inputManager.OnTap += CheckSelection;
        }
    }

    private void CheckSelection(InputType inputType, Vector3 pos)
    {
        if (selectedTile != null) {
            m_screenManager.Back();
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(pos);
        if (Physics.Raycast(ray, out RaycastHit hit)) {
            int tileLayers = Main.Instance.GlobalConfig.TileLayers;
            if (tileLayers == (tileLayers | (1 << hit.transform.gameObject.layer))) {
                Tile tile = hit.transform.gameObject.GetComponentInParent<Tile>();
                tile.Select(true);
                selectedTile = tile;

                if (tile.TileInformations != null) {
                    if (tile.TileInformations.Type == TileType.Empty) {
                        m_screenManager.OpenScreen(ScreenType.BuyTile, new OpenInfo() 
                        {
                            OnCloseScreen = CancelSelection
                        });
                    } else {
                        m_screenManager.OpenScreen(ScreenType.TileInfo, new TileInfoOpenInfo()
                        {
                            TileInformations = tile.TileInformations,
                            OnCloseScreen = CancelSelection
                        });
                    }
                }
            }
        }
    }

    public Tile GetSelected()
    {
        return selectedTile;
    }

    public void CancelSelection()
    {
        GameManager.BlockInput = false;
        if (selectedTile != null) {
            selectedTile.Select(false);
            selectedTile = null;
        }
    }
}
