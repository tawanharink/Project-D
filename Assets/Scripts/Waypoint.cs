using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public Waypoint previousWaypoint;
    public Waypoint nextWaypoint;

    public List<Waypoint> branches;

    [Range(0f, 1f)]
    public float branchRatio = 0.5f;

    public TrafficLight trafficLight; // reference to the traffic light object
}