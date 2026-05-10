using Unity.VisualScripting;
using UnityEngine;

public class SwitchPOV : MonoBehaviour
{
    bool firstPerson = false;
    bool thirdPerson = false;

    [SerializeField] GameObject firstPersonCam;
    [SerializeField] GameObject thirdPersonCam;
    //[SerializeField] GameObject cinematicCam;

    private void Start()
    {
        // first person by default on start
        firstPersonCam.SetActive(true);
        thirdPersonCam.SetActive(false);
        //cinematicCam.SetActive(false);

        thirdPerson = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T) && !firstPerson) // toggle first person perspective
        {
            SwitchToFirstPerson();
        }
        else if (Input.GetKeyDown(KeyCode.T) && !thirdPerson) // toggle third person perspective
        {
            SwitchToThirdPerson();
        }
        else if (Input.GetKeyDown(KeyCode.V))
        {
            SwitchToCinematic();
        }
    }

    public void SwitchToFirstPerson()
    {
        firstPerson = true;
        thirdPerson = false;
        firstPersonCam.SetActive(true);
        thirdPersonCam.SetActive(false);
        //cinematicCam.SetActive(false);
    }

    public void SwitchToThirdPerson()
    {
        firstPerson = false;
        thirdPerson = true;
        thirdPersonCam.SetActive(true);
        firstPersonCam.SetActive(false);
        //cinematicCam.SetActive(false);
    }

    public void SwitchToCinematic()
    {
        firstPerson = false;
        thirdPerson = false;
        //cinematicCam.SetActive(true);
        firstPersonCam.SetActive(false);
        thirdPersonCam.SetActive(false);
    }
}
