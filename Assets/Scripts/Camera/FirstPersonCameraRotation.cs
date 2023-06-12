using UnityEngine;
using UnityEngine.InputSystem;

public class FirstPersonCameraRotation : MonoBehaviour
{
    public float mouseSensitivity = 1f; // Adjust as needed
    public float controllerSensitivity = 100f; // Adjust as needed
    private float xRotation = 0f;
    private float yRotation = 0f;

    public InputAction lookAction; // Mouse Look
    public InputAction rightStickAction; // Controller Look

    void Awake()
    {
        // Mouse look action
        lookAction = new InputAction("Look", binding: "<Mouse>/delta");
        lookAction.performed += ctx => Look(ctx.ReadValue<Vector2>(), mouseSensitivity);
        lookAction.Enable();

        // Controller look action
        rightStickAction = new InputAction("RightStickLook", binding: "<Gamepad>/rightStick");
        rightStickAction.performed += ctx => Look(ctx.ReadValue<Vector2>(), controllerSensitivity);
        rightStickAction.Enable();
    }

    void Look(Vector2 delta, float sensitivity)
    {
        float mouseX = delta.x * sensitivity * Time.deltaTime;
        float mouseY = delta.y * sensitivity * Time.deltaTime;

        yRotation += mouseX;
        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        yRotation = Mathf.Clamp(yRotation, -150f, 150f);

        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }

    void OnDestroy()
    {
        lookAction.Disable(); // always disable input actions when not needed
        rightStickAction.Disable(); // disable the controller look action as well
    }
}
