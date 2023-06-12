using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class WayPointNavigator : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    public bool reachedDestination = false;
    public float stoppingDistance = 2f;
    public Waypoint currentWaypoint;

    public float sensorLength;

    private bool trafficAhead;
    public LayerMask checkedLayer;
    private float carDistance;

    [SerializeField] private float maxSpeed = 5;
    [SerializeField] private float steeringSpeed;
    private float speed;
    private Vector3 angleVelocity;

    [SerializeField] private Vector2 movementVector;
    private Vector3 destination;
    private bool isStopped;

    [SerializeField] WheelCollider frontRight;
    [SerializeField] WheelCollider frontLeft;
    [SerializeField] WheelCollider backRight;
    [SerializeField] WheelCollider backLeft;

    public float acceleration = 500f;
    public float brakingForce = 300f;

    private float currentAcceleration = 0f;
    private float currentBrakeForce = 0f;

    private bool turning;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        destination = currentWaypoint.transform.position;

        trafficAhead = false;

        angleVelocity = new Vector3(0, 30, 0);
    }

    private void Update()
    {
        if (Vector3.Distance(this.transform.position, destination) <= stoppingDistance)
        {
            reachedDestination = true;
        }

        if (reachedDestination)
        {
            if (currentWaypoint.trafficLight != null) // check if the waypoint has a traffic light assigned to it
            {
                if (currentWaypoint.trafficLight.redLight.activeSelf || currentWaypoint.trafficLight.yellowLight.activeSelf)
                {
                    isStopped = true;
                }
                else
                {
                    isStopped = false;
                    MoveToNextWaypoint();
                }
            }
            else
            {
                isStopped = false;
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

        destination = currentWaypoint.transform.position;
        reachedDestination = false;
    }

    private void FixedUpdate()
    {
        Drive();
        Sensors();
        Steer();
    }

    public void Drive()
    {
        if (isStopped || trafficAhead)
        {
            if (trafficAhead)
            {
                currentBrakeForce = brakingForce / (carDistance / sensorLength);
            }
            else
            {
                currentBrakeForce = brakingForce;
            }
            currentAcceleration = 0f;
            speed = 0f;
        }
        else if (turning)
        {
            speed = maxSpeed / 2;
            float speedDif = speed - rb.velocity.magnitude;
            if (speedDif < 0) 
            { 
                speedDif = 0;
                currentBrakeForce = brakingForce;
                currentAcceleration = 0;
            }
            else
            {
                currentBrakeForce = 0f;
                currentAcceleration = (speedDif * acceleration) / 2;
            }
        }
        else
        {
            speed = maxSpeed;
            currentBrakeForce = 0f;
            float speedDif = speed - rb.velocity.magnitude;
            currentAcceleration = speedDif * acceleration;
        }

        // Apply acceleration to front wheels
        frontRight.motorTorque = currentAcceleration;
        frontLeft.motorTorque = currentAcceleration;

        // Apply braking force to all wheels
        frontRight.brakeTorque = currentBrakeForce;
        frontLeft.brakeTorque = currentBrakeForce;
        backRight.brakeTorque = currentBrakeForce;
        backLeft.brakeTorque = currentBrakeForce;
    }

    private void Sensors()
    {
        RaycastHit hit;
        Vector3 sensorStartPosition = this.transform.position;
        sensorStartPosition.y = sensorStartPosition.y + .25f;

        bool frontCheck = Physics.Raycast(sensorStartPosition, transform.forward, out hit, sensorLength, checkedLayer);

        if (frontCheck)
        {
            carDistance = hit.distance;
            trafficAhead = true;
        }
        else
        {
            trafficAhead = false;
        }

        Vector3 endPoint = sensorStartPosition + transform.forward * sensorLength;
        Debug.DrawLine(sensorStartPosition, endPoint, Color.cyan);
    }

    private void Steer()
    {
        Vector3 direction = (destination - this.transform.position).normalized;
        Quaternion steeringDirection = Quaternion.LookRotation(direction);

        if (Vector3.Dot(this.transform.forward, direction) < 0.8f)
        {
            turning = true;
        }
        else
        {
            turning = false;
        }

        if (rb.velocity.magnitude >= 0.15f && !isStopped)
        {
            Quaternion newRotation = Quaternion.RotateTowards(this.transform.rotation, steeringDirection, steeringSpeed * Time.deltaTime);
            rb.MoveRotation(newRotation);
        }
    }
}
