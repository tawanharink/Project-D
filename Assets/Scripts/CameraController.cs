using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float sensitivity = 400f;
    float xRotation = 0f;
    float yRotation = 0f;

    public bool isFirstPerson = true;

    public Camera mainCamera;
    public CinemachineVirtualCamera thirdPersonCamera;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Set the correct initial state for the cameras
        mainCamera.enabled = isFirstPerson;
        thirdPersonCamera.enabled = !isFirstPerson;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            isFirstPerson = !isFirstPerson;

            // Switch the camera's active state when toggling between first and third person
            mainCamera.enabled = isFirstPerson;
            thirdPersonCamera.enabled = !isFirstPerson;
        }

        if (isFirstPerson)
        {
            FirstPerson();
        }
    }

    void FirstPerson()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        Debug.Log($"x: {mouseX}\nY: {mouseY}");

        yRotation += mouseX;
        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        yRotation = Mathf.Clamp(yRotation, -150f, 150f);

        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }
}