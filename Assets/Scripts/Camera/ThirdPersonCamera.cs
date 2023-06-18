using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target;
    public float distance = 5.0f;
    public float xSpeed = 120.0f;
    public float ySpeed = 120.0f;
    public float mouseSensitivity = 0.1f; // Add a sensitivity scaling factor for the mouse

    public float yMinLimit = 5f;
    public float yMaxLimit = 80f;

    private float x = 0.0f;
    private float y = 0.0f;

    private InputAction lookAction;
    private InputAction rightStickAction;

    void Awake()
    {
        lookAction = new InputAction("Look", binding: "<Mouse>/delta");
        lookAction.performed += ctx => RotateCamera(ctx.ReadValue<Vector2>() * mouseSensitivity); // Apply the mouse sensitivity scaling factor here
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
        lookAction.Disable();
        rightStickAction.Disable();
    }
}
