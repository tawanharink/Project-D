using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public GameObject firstPersonCam;
    public GameObject thirdPersonCam;

    private int activeCam; // 0 = FirstPersonCam, 1 = ThirdPersonCam

    void Start()
    {
        // Set the initial active camera
        activeCam = 0;
        firstPersonCam.SetActive(true);
        thirdPersonCam.SetActive(false);
    }

    void Update()
    {
        // Check for input to switch cameras (e.g., pressing the 'C' key)
        if (Input.GetKeyDown(KeyCode.C))
        {
            SwitchCamera();
        }
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
}
