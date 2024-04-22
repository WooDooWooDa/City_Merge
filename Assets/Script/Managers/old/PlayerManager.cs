using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public PlayerData playerData;

    private void Start()
    {
        //loadPlayerData;
        playerData = new PlayerData //TEMP
        {
            level = 1,
            currentExp = 0,
            maxExp = 200,
            maxBuildingLevel = 2,
            nbBoughtPerLevel = new int[] {0,0,0,0,0,0,0,0,0,0,0}
        };
    }

    public void AddExp(float exp)
    {
        playerData.currentExp += exp;
        if (playerData.CanLevelUp()) {
            playerData.LevelUp();
        }
    }

    public struct PlayerData
    {
        public int level;
        public float currentExp;
        public float maxExp;

        public int maxBuildingLevel;
        public int[] nbBoughtPerLevel;

        public bool CanLevelUp()
        {
            return currentExp >= maxExp;
        }

        public void LevelUp()
        {
            level++;
            float left = currentExp - maxExp;
            currentExp = left;
        }
    }
}

[System.Serializable]
public class PlayerData
{

}
