using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildingsConfig", menuName = "Config/Buildings", order = 1)]
public class BuildingsConfig : ScriptableObject
{
    [SerializeField] private Building m_buildingBase;
    public Building BuildingBase => m_buildingBase;
    [SerializeField] private Construction m_construction;
    public Construction Construction => m_construction;
    [SerializeField] private BuildingInformations[] m_buildingInformations;
    public BuildingInformations[] BuildingInformations { get { return m_buildingInformations; } }
}
