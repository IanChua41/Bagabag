using Unity.VisualScripting;
using UnityEngine;

public class SwitchPOV : MonoBehaviour
{
    public bool firstPerson = false;

    [SerializeField] GameObject firstPersonCam;
    [SerializeField] GameObject thirdPersonCam;

    private void Start()
    {
        // third person by default on start
        firstPersonCam.SetActive(false);
        thirdPersonCam.SetActive(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T) && !firstPerson) // toggle first person perspective
        {
            firstPerson = true;
            firstPersonCam.SetActive(true);
            thirdPersonCam.SetActive(false);
        }
        else if (Input.GetKeyDown(KeyCode.T) && firstPerson) // toggle third person perspective
        {
            firstPerson = false;
            thirdPersonCam.SetActive(true);
            firstPersonCam.SetActive(false);
        }
    }
}
