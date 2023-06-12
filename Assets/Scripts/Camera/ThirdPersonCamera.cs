using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target; // The vehicle you want the camera to follow
    public float distance = 5.0f; // Distance from the vehicle
    public float xSpeed = 120.0f; // Horizontal rotation speed
    public float ySpeed = 120.0f; // Vertical rotation speed

    public float yMinLimit = -20f; // Minimum vertical angle
    public float yMaxLimit = 80f; // Maximum vertical angle

    private float x = 0.0f;
    private float y = 0.0f;

    private InputAction lookAction; // Mouse Look
    private InputAction rightStickAction; // Controller Look

    void Awake()
    {
        // Mouse look action
        lookAction = new InputAction("Look", binding: "<Mouse>/delta");
        lookAction.performed += ctx => RotateCamera(ctx.ReadValue<Vector2>());
        lookAction.Enable();

        rightStickAction = new InputAction("RightStickLook", binding: "<Gamepad>/rightStick");
        rightStickAction.performed += ctx => RotateCamera(ctx.ReadValue<Vector2>());
        rightStickAction.Enable();
    }

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;

        if (GetComponent<Rigidbody>())
        {
            GetComponent<Rigidbody>().freezeRotation = true;
        }
    }

    void RotateCamera(Vector2 delta)
    {
        if (target)
        {
            x += delta.x * xSpeed * Time.deltaTime;
            y -= delta.y * ySpeed * Time.deltaTime;

            y = ClampAngle(y, yMinLimit, yMaxLimit);

            Quaternion rotation = Quaternion.Euler(y, x, 0);
            Vector3 position = rotation * new Vector3(0.0f, 0.0f, -distance) + target.position;

            transform.rotation = rotation;
            transform.position = position;
        }
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360f)
            angle += 360f;
        if (angle > 360f)
            angle -= 360f;
        return Mathf.Clamp(angle, min, max);
    }

    void OnDestroy()
    {
        lookAction.Disable(); // always disable input actions when not needed
        rightStickAction.Disable(); // disable the controller look action as well
    }
}
