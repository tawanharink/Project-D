using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BusController : MonoBehaviour
{
    public WheelCollider[] frontWheels;
    public WheelCollider[] rearWheels;

    [SerializeField] private WheelCollider frontLeftWheelCollider;
    [SerializeField] private WheelCollider frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider;
    [SerializeField] private WheelCollider rearRightWheelCollider;
    public float maxSteerAngle = 30f;
    public float engineTorque = 2000f;
    public float brakingTorque = 2000f;
    public float engineBrakingTorque = 600.0f;
    public float currentSpeed;

    private float steering;
    private float throttle;
    public float brake;

    public Vector3 com = new Vector3(0f, 0.35f, -0.5f);
    public Rigidbody body;

    public InputAction controls;
    public InputAction steeringAction;
    public InputAction throttleAction;
    public InputAction brakeAction;

    private void OnEnable()
    {
        steeringAction.Enable();
        steeringAction.performed += OnSteering;
        steeringAction.canceled += OnSteering;

        throttleAction.Enable();
        throttleAction.performed += OnThrottle;
        throttleAction.canceled += OnThrottle;

        brakeAction.Enable();
        brakeAction.performed += OnBrake;
        brakeAction.canceled += OnBrake;
    }

    private void OnDisable()
    {
        steeringAction.Disable();
        steeringAction.performed -= OnSteering;
        steeringAction.canceled -= OnSteering;

        throttleAction.Disable();
        throttleAction.performed -= OnThrottle;
        throttleAction.canceled -= OnThrottle;

        brakeAction.Disable();
        brakeAction.performed -= OnBrake;
        brakeAction.canceled -= OnBrake;
    }

    private void Start()
    {
        frontWheels = new WheelCollider[2] { frontLeftWheelCollider, frontRightWheelCollider };
        rearWheels = new WheelCollider[2] { rearLeftWheelCollider, rearRightWheelCollider };

        body = GetComponent<Rigidbody>();
        body.centerOfMass = com;
    }

    private void OnSteering(InputAction.CallbackContext context)
    {
        steering = context.ReadValue<float>();
    }

    private void OnThrottle(InputAction.CallbackContext context)
    {
        throttle = context.ReadValue<float>();
    }

    private void OnBrake(InputAction.CallbackContext context)
    {
        brake = context.ReadValue<float>() > 0.0f ? brakingTorque : 0f;
    }

    //void Update()
    //{
    //    // Get input values from player
    //    steering = Input.GetAxis("Horizontal");
    //    throttle = Input.GetAxis("Vertical");
    //    brake = Input.GetKey(KeyCode.Space) ? brakingTorque : 0f;

    //    float engineBrake = 0f;
    //    if (throttle == 0f)
    //    {
    //        engineBrake = engineBrakingTorque;
    //    }

    //    // Set steering angle for front wheels
    //    foreach (WheelCollider wheel in frontWheels)
    //    {
    //        wheel.steerAngle = steering * maxSteerAngle;
    //    }

    //    // Apply brake torque to all wheels
    //    foreach (WheelCollider wheel in frontWheels)
    //    {
    //        wheel.brakeTorque = brake;
    //    }
    //    foreach (WheelCollider wheel in rearWheels)
    //    {
    //        wheel.brakeTorque = brake;
    //    }

    //    // Set engine torque for rear wheels
    //    foreach (WheelCollider wheel in rearWheels)
    //    {
    //        wheel.motorTorque = throttle * engineTorque;
    //        wheel.brakeTorque = (brake + engineBrake);// * Time.deltaTime;
    //    }

    //    // Calculate current speed of bus
    //    currentSpeed = GetComponent<Rigidbody>().velocity.magnitude * 3.6f; // Convert from m/s to km/h
    //}

    void FixedUpdate()
    {
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