using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ProfileConfig", menuName = "Config/Profile", order = 4)]
public class ProfileConfig : ScriptableObject
{
    [SerializeField] private int m_maxLevel = 10;
    public int MaxLevel => m_maxLevel;
    [SerializeField] private int[] m_nextLevel;
    public int[] NextLevel => m_nextLevel;
    
}
