using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    [SerializeField]
    public Waypoint previousWaypoint;
    [SerializeField]
    public Waypoint nextWaypoint;
    [SerializeField]
    public Waypoint detourWaypoint;
    [SerializeField]
    [Range(0f, 5f)]
    public float width = 0.2f;
    [SerializeField]
    public bool connectToNext = false;
    [SerializeField]
    public List<Waypoint> branches = new List<Waypoint>();
    [SerializeField]
    [Range(0f, 1f)]
    public float branchRatio = 0.5f;
    [SerializeField]
    public LayerMask waypointLayer;

    public Vector3 GetPosition()
    {
        Vector3 minBound = transform.position + transform.right * width / 2f;
        Vector3 maxBound = transform.position - transform.right * width / 2f;

        return Vector3.Lerp(minBound, maxBound, Random.Range(0f, 1f));
    }

    public bool Connect()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 0.1f, waypointLayer);
        if (colliders.Length > 0) {
            Waypoint nextWaypoint = colliders[0].GetComponent<Waypoint>();
            this.nextWaypoint = nextWaypoint;
            nextWaypoint.previousWaypoint = this.GetComponent<Waypoint>();
            return true;
        }
        return false;
    }
}
