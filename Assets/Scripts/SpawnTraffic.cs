using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTraffic : MonoBehaviour
{
    [SerializeField] GameObject car;

    [SerializeField] GameObject waypointParent;

    [SerializeField] int trafficDensity;
    [HideInInspector] public int actualTrafficDensity;
    [SerializeField] float spawnDelay;

    private Transform traffic;

    [SerializeField] BoxCollider nearCar;
    [SerializeField] SphereCollider farCar;

    [SerializeField] float rayCastDistance;
    [SerializeField] float overlapBoxSize = 1f;
    [SerializeField] LayerMask spawnLayerMask;

    void Start()
    {
        traffic = this.transform;

        actualTrafficDensity = 0;

        if (actualTrafficDensity < trafficDensity)
        {
            StartCoroutine(SpawnCar());
        }
    }

    IEnumerator SpawnCar()
    {
        if (actualTrafficDensity < trafficDensity)
        {
            Waypoint waypoint = GetWaypoint();

            if (PositionRaycast(waypoint.transform))
            {
                GameObject trafficCar = Instantiate(car, waypoint.transform.position, waypoint.transform.rotation, traffic);

                actualTrafficDensity += 1;

                trafficCar.layer = LayerMask.NameToLayer("Traffic");

                WayPointNavigator navigator = trafficCar.GetComponent<WayPointNavigator>();
                navigator.currentWaypoint = waypoint;

                DespawnTraffic despawnTraffic = trafficCar.GetComponent<DespawnTraffic>();
                despawnTraffic.farCar = farCar;
                despawnTraffic.spawnTraffic = GetComponent<SpawnTraffic>();
            }
        }

        yield return new WaitForSeconds(spawnDelay / (trafficDensity + 1 - actualTrafficDensity));
        StartCoroutine(SpawnCar());

    }

    bool PositionRaycast(Transform waypoint)
    {
        RaycastHit hit;

        if (Physics.Raycast(new Vector3(waypoint.position.x, waypoint.position.y + 1f, waypoint.position.z), Vector3.down, out hit, rayCastDistance))
        {
            Quaternion spawnRotation = Quaternion.FromToRotation(Vector3.up, hit.normal);

            Vector3 overlapTestBoxScale = new Vector3(overlapBoxSize, overlapBoxSize, overlapBoxSize);
            Collider[] collidersInsideOverlapBox = new Collider[1];
            int numberOfCollidersFound = Physics.OverlapBoxNonAlloc(hit.point, overlapTestBoxScale, collidersInsideOverlapBox, spawnRotation, spawnLayerMask);

            if (numberOfCollidersFound == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }

    private Waypoint GetWaypoint()
    {
        Waypoint[] waypoints = waypointParent.GetComponentsInChildren<Waypoint>();
        Waypoint waypoint = waypoints[Random.Range(0, waypoints.Length)];

        while (nearCar.bounds.Contains(waypoint.transform.position) && !farCar.bounds.Contains(waypoint.transform.position))
        {
            waypoints = waypointParent.GetComponentsInChildren<Waypoint>();
            waypoint = waypoints[Random.Range(0, waypoints.Length)];
        }

        return waypoint;
    }
}
