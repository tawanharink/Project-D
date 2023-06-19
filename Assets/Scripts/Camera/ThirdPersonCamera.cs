using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target; // Het object waar de camera naar kijkt
    public float distance = 5.0f; // Afstand van de camera tot het target
    public float xSpeed = 120.0f; // Snelheid waarmee de camera om het target heen draait
    public float ySpeed = 120.0f; 
    public float mouseSensitivity = 0.1f; // Factor waarmee de muis input wordt vermenigvuldigd (voor sensitivity)

    public float yMinLimit = 5f; // Minimale hoek waarmee de camera om het target heen kan draaien (verticaal)
    public float yMaxLimit = 80f; // Maximale hoek waarmee de camera om het target heen kan draaien

    private float x = 0.0f; // Huidige rotatie van de camera, gebruikt om de camera te laten roteren
    private float y = 0.0f;

    private InputAction lookAction; // Input action voor muis
    private InputAction rightStickAction; // Input action voor controller

    void Awake()
    {
        lookAction = new InputAction("Look", binding: "<Mouse>/delta");
        lookAction.performed += ctx => RotateCamera(ctx.ReadValue<Vector2>() * mouseSensitivity);
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
