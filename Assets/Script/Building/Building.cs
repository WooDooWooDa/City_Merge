using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ProduceMoney))]
public class Building : MonoBehaviour, ISavableLoadable<BuildingData, BuildingInformations>
{
    [SerializeField] private Transform m_modelParent;

    private BuildingInformations m_buildingInformations;
    public BuildingInformations BuildingInformations => m_buildingInformations;

    private BuildingManager m_buildingManager;
    private ProduceMoney m_production;
    private int m_currentPrestigeLevel = 0;
    public int PrestigeLevel => m_currentPrestigeLevel;

    private void Awake()
    {
        m_buildingInformations = default;
        m_production = GetComponent<ProduceMoney>();
        m_buildingManager = Main.Instance.GetManager<BuildingManager>();
    }

    private void OnEnable()
    {
        GetComponent<Animator>().SetTrigger("SpawnMerge");
    }

    public void Prestige(int prestigeLevel)
    {
        m_currentPrestigeLevel = prestigeLevel;
        //animation...?
    }

    public void Display(CurrencyAmount currency)
    {
        m_production.Display(currency);
    }

    public bool IsMergeable(BuildingLevel otherLvl)
    {
        return m_buildingInformations.Level == otherLvl;
    }

    public void Load(BuildingData data, BuildingInformations info)
    {
        m_buildingInformations = info;
        m_production.SetData(m_buildingInformations.Production);
        ChangeModel(info);

        if (data != null ) {
            transform.SetPositionAndRotation(data.Position, data.Rotation);
            m_currentPrestigeLevel = data.Prestige;
        }
    }

    public void Save(ref BuildingData data)
    {
        data.Level = m_buildingInformations.Level;
        data.Type = m_buildingInformations.BuildingType;

        data.Position = transform.position;
        data.Rotation = transform.rotation;
        data.ProductionHistory = m_production.History;
    }

    public void ChangeModel(BuildingInformations info)
    {
        //Destroy(m_modelParent.GetChild(0));
        Instantiate(info.Model, m_modelParent);
    }
}