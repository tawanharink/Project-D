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

    public float sensorLength;

    private bool trafficAhead;
    public LayerMask checkedLayer;

    private float maxSpeed;

    private void Start()
    {
        car.SetDestination(currentWaypoint.transform.position);

        trafficAhead = false;

        maxSpeed = car.speed;
    }

    private void Update()
    {
        if (Vector3.Distance(car.transform.position, car.destination) <= car.stoppingDistance)
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

    private void FixedUpdate()
    {
        Sensors();
        Brakes();
    }

    private void Sensors()
    {
        RaycastHit hit;
        Vector3 sensorStartPosition = this.transform.position;
        Vector3 direction = (currentWaypoint.transform.position - this.transform.position).normalized;

        bool frontCheck = Physics.Raycast(sensorStartPosition, transform.forward, out hit, sensorLength, checkedLayer);
        // bool waypointCheck = Physics.Raycast(sensorStartPosition, direction, out hit, sensorLength);

        if (frontCheck)
        {
            trafficAhead = true;        }
        else
        {
            trafficAhead = false;
        }

        Vector3 endPoint = sensorStartPosition + transform.forward * sensorLength;
        Debug.DrawLine(sensorStartPosition, endPoint, Color.cyan);

        // Vector3 endPoint2 = sensorStartPosition + direction * sensorLength;
        // Debug.DrawLine(sensorStartPosition, endPoint2, Color.magenta);
    }

    private void Brakes()
    {
        if (trafficAhead)
        {
            car.speed = car.speed - 1.5f * Time.deltaTime;
        }
        else
        {
            car.speed = maxSpeed;
        }
    }
}
