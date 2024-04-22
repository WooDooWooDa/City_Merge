using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointNavigator : MonoBehaviour
{
    [SerializeField]
    private CarController controller;
    [SerializeField]
    private Waypoint currentWaypoint;

    private void Awake()
    {
        controller = GetComponent<CarController>();
    }

    private void Start()
    {
        controller.SetDestination(currentWaypoint.GetPosition());
    }

    void Update()
    {
        
        if (controller.hasReachedDestination) {
            bool shouldBranch = false;
            
            if (currentWaypoint != null && currentWaypoint.branches != null && currentWaypoint.branches.Count > 0) {
                shouldBranch = Random.Range(0f, 1f) <= currentWaypoint.branchRatio;
            }
            if (shouldBranch) {
                Waypoint branchTo = currentWaypoint.branches[Random.Range(0, currentWaypoint.branches.Count)];
                currentWaypoint = branchTo;
            } else {
                if (currentWaypoint.nextWaypoint == null) {
                    currentWaypoint.nextWaypoint = currentWaypoint.detourWaypoint;
                }
                currentWaypoint = currentWaypoint.nextWaypoint;
            }

            if (currentWaypoint == null) {
                currentWaypoint = FindClosestWaypoint();
            }

            controller.SetDestination(currentWaypoint.GetPosition());
        }
    }

    private Waypoint FindClosestWaypoint()
    {
        //Debug.Log("Finding closest...");
        Waypoint closest = null;
        float maxDist = Mathf.Infinity;
        Waypoint[] waypoints = FindObjectsOfType<Waypoint>();
        foreach (Waypoint waypoint in waypoints) {
            float dist = Vector3.Distance(transform.position, waypoint.transform.position);
            if (dist < maxDist && waypoint.nextWaypoint != null) {
                maxDist = dist;
                closest = waypoint;
            }
        }
        return closest;
    }
}
