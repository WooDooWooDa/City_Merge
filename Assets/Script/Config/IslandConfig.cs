using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IslandConfig", menuName = "Config/Island", order = 3)]
public class IslandConfig : ScriptableObject
{
    public GameObject IslandBase => m_islandBase;
    [SerializeField] private GameObject m_islandBase;
    public int MaxIslandLevel => m_maxIslandLevel;
    [SerializeField] private int m_maxIslandLevel = 10;

    public Vector2 StartingSize => m_startingSize;
    [Header("Starting Island")]
    [SerializeField] private Vector2 m_startingSize;

}
