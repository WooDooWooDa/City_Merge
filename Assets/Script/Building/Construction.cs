using System;
using UnityEngine;

public class Construction : MonoBehaviour
{
    [SerializeField]
    private GameObject m_stage;
    [SerializeField]
    private Transform m_crane;

    public Func<Tile, BuildingKey, BuildingData, Building> OnBuild;
    public Action<Building> OnDone;

    private BuildingKey m_buildingToBuild;
    private Tile m_tileDest;

    private bool m_startedConstruction = false;

    private float m_stepsToGo = 0.4f;
    private int m_stageToBuild;
    private int m_stageBuilt = 0;
    private float m_timeElapsed;

    private void Update()
    {
        if (!m_startedConstruction) return;

        if (m_stageBuilt < m_stageToBuild && m_timeElapsed >= 1f) {
            m_timeElapsed = 0;
            ConstructStage();
            m_stageBuilt++;
        } else if (m_stageBuilt == m_stageToBuild) {
            ConstructBuilding();
        } else {
            m_timeElapsed += Time.deltaTime;
        }
    }

    public void StartConstruction()
    {
        m_startedConstruction = true;
    }

    public void SetBuildingToConstruct(Tile dest, BuildingInformations info)
    {
        m_tileDest = dest;
        m_buildingToBuild = info.Key;
        m_stageToBuild = info.StageToBuild;
    }

    private void ConstructStage()
    {
        Instantiate(m_stage, transform.position + Vector3.up * m_stageBuilt * 0.4f, Quaternion.identity, transform);
        m_crane.position += Vector3.up * m_stepsToGo;
    }

    private void ConstructBuilding()
    {
        OnBuild?.Invoke(m_tileDest, m_buildingToBuild, null);
        //OnDone?.Invoke();
        Destroy(gameObject);
    }
}
