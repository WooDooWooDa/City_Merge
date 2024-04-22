using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour, ISavableLoadable<TileData, TileInformations>
{
    private TileInformations m_tileInformations;
    public TileInformations TileInformations => m_tileInformations;

    [SerializeField]
    private GameObject border;
    [SerializeField]
    private Transform m_tilePrefabTransform;

    public Building BuildingOnTile => m_buildingOnTile;
    private Building m_buildingOnTile;
    private Construction m_construction;

    private bool isSelected;
    private int m_currentPrefabIndex = 0;

    void Awake()
    {
        m_tileInformations = default;
    }

    public void AnimateSpawn()
    {
        GetComponent<Animator>().SetTrigger("Spawn");
    }

    public void Select(bool state)
    {
        isSelected = state;
        border.SetActive(state);
    }

    #region Building

    public void PutDownBuilding(Building building)
    {
        m_buildingOnTile = building;
        m_construction = null;
    }

    public void PutDownConstruction(Construction construction)
    {
        m_construction = construction;
    }

    public bool HasABuilding()
    {
        return m_buildingOnTile != null || Physics.CheckSphere(transform.localPosition + (Vector3.up * 0.5f), 0.5f, LayerMask.NameToLayer("Building"));
    }

    public bool HasAConstruction()
    {
        return m_construction != null;
    }

    #endregion Building

    #region Tile Type

    public void ChangeTileType(TileInformations newTileInfo)
    {
        m_tileInformations = newTileInfo;
        gameObject.name = m_tileInformations.Type.ToString();
        SpawnTypeObj();
    }

    private void SpawnTypeObj()
    {
        if (m_tilePrefabTransform.childCount >= 1) {
            Destroy(m_tilePrefabTransform.GetChild(0).gameObject);
        }

        if (m_tileInformations.TilePrefab.Length > 0) {
            var obj = Instantiate(m_tileInformations.TilePrefab[m_currentPrefabIndex], m_tilePrefabTransform);
        }
    }

    #endregion Tile Type

    #region Load And Save
    public void Load(TileData data, TileInformations info)
    {
        ChangeTileType(info);
        transform.position = data.Position;
        m_currentPrefabIndex = data.CurrentPrefabIndex;
    }

    public void Save(ref TileData data)
    {
        data.Position = transform.position;
        data.Type = m_tileInformations.Type;
        data.GrassTypeIndex = m_currentPrefabIndex;
    }
#endregion

}
