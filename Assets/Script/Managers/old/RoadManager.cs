using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> roads = new List<GameObject>();

    private void Start()
    {
        //load roads
        //spawn cars if necesseray
    }

    public void AddRoad(GameObject road)
    {
        roads.Add(road);
    }
}
