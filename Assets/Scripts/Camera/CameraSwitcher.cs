using UnityEngine;
using UnityEngine.InputSystem;

public class CameraSwitcher : MonoBehaviour
{
    public GameObject firstPersonCam;
    public GameObject thirdPersonCam;

    private int activeCam; // 0 = FirstPersonCam, 1 = ThirdPersonCam
    public InputAction switchCameraAction;

    void Awake()
    {
        // Initialize new input system
        // switchCameraAction = new InputAction("SwitchCamera", binding: "<keyboard>/c");
        switchCameraAction.performed += _ => SwitchCamera(); // trigger switch camera on performed
        switchCameraAction.Enable(); // enable the input action
    }

    void Start()
    {
        // Set the initial active camera
        activeCam = 0;
        firstPersonCam.SetActive(true);
        thirdPersonCam.SetActive(false);
    }

    void SwitchCamera()
    {
        activeCam = (activeCam + 1) % 2; // Toggle between 0 and 1

        if (activeCam == 0)
        {
            firstPersonCam.SetActive(true);
            thirdPersonCam.SetActive(false);
        }
        else
        {
            firstPersonCam.SetActive(false);
            thirdPersonCam.SetActive(true);
        }
    }

    void OnDestroy()
    {
        switchCameraAction.Disable(); // always disable input actions when not needed
    }
}
