using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WayPointNavigator : MonoBehaviour
{
    [SerializeField] NavMeshAgent car;
    public bool reachedDestination = false;
    public float stoppingDistance = 2f;
    public Waypoint currentWaypoint;

    private void Start()
    {
        car.SetDestination(currentWaypoint.transform.position);
    }

    private void Update()
    {
        if (Vector3.Distance(car.transform.position, car.destination) < stoppingDistance)
        {
            reachedDestination = true;
        }

        if (reachedDestination)
        {
            if (currentWaypoint.trafficLight != null) // check if the waypoint has a traffic light assigned to it
            {
                if (currentWaypoint.trafficLight.redLight.activeSelf || currentWaypoint.trafficLight.yellowLight.activeSelf)
                {
                    car.isStopped = true;
                }
                else
                {
                    car.isStopped = false;
                    MoveToNextWaypoint();
                }
            }
            else
            {
                car.isStopped = false;
                MoveToNextWaypoint();
            }
        }
    }

    private void MoveToNextWaypoint()
    {
        bool shouldBranch = false;

        if (currentWaypoint.branches != null && currentWaypoint.branches.Count > 0)
        {
            shouldBranch = Random.Range(0f, 1f) <= currentWaypoint.branchRatio ? true : false;
        }

        if (shouldBranch)
        {
            currentWaypoint = currentWaypoint.branches[Random.Range(0, currentWaypoint.branches.Count - 1)];
        }
        else
        {
            currentWaypoint = currentWaypoint.nextWaypoint;
        }

        car.SetDestination(currentWaypoint.transform.position);
        reachedDestination = false;
    }
}
