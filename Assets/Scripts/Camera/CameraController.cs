using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public bool isFirstPerson = true;

    public Camera mainCamera;
    public CinemachineVirtualCamera thirdPersonCamera;

    private CinemachineBrain cinemachineBrain;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        cinemachineBrain = mainCamera.GetComponent<CinemachineBrain>();
        if (cinemachineBrain == null)
        {
            cinemachineBrain = mainCamera.gameObject.AddComponent<CinemachineBrain>();
        }
        cinemachineBrain.m_UpdateMethod = CinemachineBrain.UpdateMethod.LateUpdate;

        thirdPersonCamera.Priority = isFirstPerson ? 0 : 1;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            isFirstPerson = !isFirstPerson;
            thirdPersonCamera.Priority = isFirstPerson ? 0 : 1;

            // Enable or disable the FirstPersonCameraRotation script based on the current camera mode
            FirstPersonCameraRotation firstPersonCameraRotation = mainCamera.GetComponent<FirstPersonCameraRotation>();
            if (firstPersonCameraRotation != null)
            {
                firstPersonCameraRotation.enabled = isFirstPerson;
            }
        }
    }
}