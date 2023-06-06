using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusController : MonoBehaviour
{
    public WheelCollider[] frontWheels;
    public WheelCollider[] rearWheels;

    [SerializeField] private WheelCollider frontLeftWheelCollider;
    [SerializeField] private WheelCollider frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider;
    [SerializeField] private WheelCollider rearRightWheelCollider;
    public float maxSteerAngle = 30f;
    public float engineTorque = 500f;
    public float brakingTorque = 2000f;
    public float engineBrakingTorque = 600.0f;
    public float currentSpeed;

    private float steering;
    private float throttle;
    public float brake;

    public Vector3 com = new Vector3(0f, 0.35f, -0.5f);
    public Rigidbody body;

    private void Start()
    {
        frontWheels = new WheelCollider[2] { frontLeftWheelCollider, frontRightWheelCollider };
        rearWheels = new WheelCollider[2] { rearLeftWheelCollider, rearRightWheelCollider };

        body = GetComponent<Rigidbody>();
        body.centerOfMass = com;
    }

    void Update()
    {
        // Get input values from player
        steering = Input.GetAxis("Horizontal");
        throttle = Input.GetAxis("Vertical");
        brake = Input.GetKey(KeyCode.Space) ? brakingTorque : 0f;

        float engineBrake = 0f;
        if (throttle == 0f)
        {
            engineBrake = engineBrakingTorque;
        }

        // Set steering angle for front wheels
        foreach (WheelCollider wheel in frontWheels)
        {
            wheel.steerAngle = steering * maxSteerAngle;
        }

        //// Set engine torque for rear wheels
        //foreach (WheelCollider wheel in rearWheels)
        //{
        //    wheel.motorTorque = throttle * engineTorque;
        //}

        // Apply brake torque to all wheels
        foreach (WheelCollider wheel in frontWheels)
        {
            wheel.brakeTorque = brake;
        }
        foreach (WheelCollider wheel in rearWheels)
        {
            wheel.brakeTorque = brake;
        }

        // Set engine torque for rear wheels
        foreach (WheelCollider wheel in rearWheels)
        {
            wheel.motorTorque = throttle * engineTorque;
            wheel.brakeTorque = (brake + engineBrake);// * Time.deltaTime;
        }

        // Calculate current speed of bus
        currentSpeed = GetComponent<Rigidbody>().velocity.magnitude * 3.6f; // Convert from m/s to km/h
    }
}