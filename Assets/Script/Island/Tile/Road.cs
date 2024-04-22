using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour
{
    [SerializeField]
    private LayerMask roadLayer;
    [SerializeField]
    private List<Waypoint> waypointsToConnectToNext = new List<Waypoint>();
    [SerializeField]
    private List<Waypoint> waypointsToConnectFromPrevious = new List<Waypoint>();

    void Start()
    {
        ReconnectThisRoad();
        FindObjectOfType<RoadManager>().AddRoad(this.gameObject);
    }

    public void ReconnectThisRoad()
    {
        foreach (Waypoint waypoint in waypointsToConnectFromPrevious) {
            waypoint.previousWaypoint = null;
        }
        ConnectThisRoad();
        //ReconnectRoadsAround();
        ReconnectAllRoads();
    }

    public void ConnectThisRoad()
    {
        foreach (Waypoint waypoint in waypointsToConnectToNext) {
            if (!waypoint.Connect()) {
                waypoint.nextWaypoint = waypoint.detourWaypoint;
                waypoint.detourWaypoint.previousWaypoint = waypoint;
            }
        }
    }

    private void ReconnectAllRoads()
    {
        Road[] connecters = FindObjectsOfType<Road>();
        foreach (Road connecter in connecters) {
            connecter.ConnectThisRoad();
        }
    }

    private void ReconnectRoadsAround()
    {
        Collider[] waypointColliders = Physics.OverlapSphere(transform.position, 1.5f, roadLayer);
        foreach (Collider collider in waypointColliders) {
            //Debug.Log(collider.name);
        }
    }
}
