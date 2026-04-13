using UnityEngine;

public class Car : MonoBehaviour
{
    [SerializeField] GameObject driverSeat;
    [SerializeField] Transform carExit;
    [SerializeField] GameObject rearviewMirror;
    [SerializeField] float enterRange = 3f;

    private GameObject player;
    private PlayerDrive drive;
    private bool inCar = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        drive = GetComponent<PlayerDrive>();

        if (rearviewMirror != null)
        {
            rearviewMirror.SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) && !inCar)
        {
            float dist = Vector3.Distance(player.transform.position, transform.position);
            if (dist <= enterRange)
            {
                GetInCar();
            }
            else
            {
                Debug.Log("You are too far away from the car to enter.");
            }
        }
        else if (Input.GetKeyDown(KeyCode.C) && inCar)
        {
            ExitCar();
        }
    }

    public void GetInCar()
    {
        player.GetComponent<SwitchPOV>().SwitchToThirdPerson(); // switch perspective 

        player.GetComponent<CharacterController>().gameObject.SetActive(false); // disable default player control

        player.transform.position = driverSeat.transform.position;
        player.transform.SetParent(driverSeat.transform, true);
        inCar = true;

        if (rearviewMirror != null)
        {
            rearviewMirror.SetActive(true);
        }

        drive.SetToDrive(); // enable driving controls
    }

    public void ExitCar()
    {
        drive.StopDrive(); // disable driving controls
        
        inCar = false;
        player.transform.SetParent(null, true);
        player.transform.position = carExit.transform.position;

        player.GetComponent<CharacterController>().gameObject.SetActive(true); // enable default player control

        player.GetComponent<SwitchPOV>().SwitchToFirstPerson(); // switch perspective

        if (rearviewMirror != null)
        {
            rearviewMirror.SetActive(false);
        }
    }
}
