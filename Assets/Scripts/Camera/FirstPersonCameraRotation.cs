using UnityEngine;

public class FirstPersonCameraRotation : MonoBehaviour
{
    public float sensitivity = 400f;
    float xRotation = 0f;
    float yRotation = 0f;

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        yRotation += mouseX;
        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        yRotation = Mathf.Clamp(yRotation, -150f, 150f);

        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }
}