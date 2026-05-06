using UnityEngine;

public class Car : MonoBehaviour
{
    [SerializeField] GameObject driverSeat;
    [SerializeField] Transform carExit;
    [SerializeField] GameObject rearviewMirror;
    [SerializeField] float enterRange = 3f;

    [Header("Movement Settings")]
    [SerializeField] public float forwardSpeed = 10f;
    [SerializeField] public float reverseSpeed = 4f;
    [SerializeField] public float forwardSteering = 120f;
    [SerializeField] public float reverseSteering = 50f;

    [Header("Sound Cues")]
    [SerializeField] private AudioClip exteriorOpenSound;
    [SerializeField] private AudioClip exteriorCloseSound;
    [SerializeField] private AudioClip interiorOpenSound;
    [SerializeField] private AudioClip interiorCloseSound;
    [SerializeField] private AudioClip engineStartSound;
    [SerializeField] private AudioClip engineCloseSound;
    [SerializeField] private AudioSource audioSource;

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
        drive.SetSpeeds(forwardSpeed, reverseSpeed, forwardSteering, reverseSteering);

        PlayEngineStart();
        PlayExteriorOpen();
        Invoke(nameof(PlayInteriorClose), 0.5f); // Delay interior close sound by 0.5 seconds
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

        PlayInteriorOpen();
        Invoke(nameof(PlayExteriorClose), 0.5f); // Delay exterior close sound by 0.5 seconds
        PlayEngineClose();
    }

    public void PlayExteriorOpen()
    {
        if (audioSource != null && exteriorOpenSound != null)
        {
            audioSource.PlayOneShot(exteriorOpenSound);
        }
        Debug.Log("Playing exterior open sound cue.");
    }

    public void PlayInteriorClose()
    {
        if (audioSource != null && interiorCloseSound != null)
        {
            audioSource.PlayOneShot(interiorCloseSound);
        }
        Debug.Log("Playing interior close sound cue.");
    }

    public void PlayInteriorOpen()
    {
        if (audioSource != null && interiorOpenSound != null)
        {
            audioSource.PlayOneShot(interiorOpenSound);
        }
        Debug.Log("Playing interior open sound cue.");
    }

    public void PlayExteriorClose()
    {
        if (audioSource != null && exteriorCloseSound != null)
        {
            audioSource.PlayOneShot(exteriorCloseSound);
        }
        Debug.Log("Playing exterior close sound cue.");
    }

    public void PlayEngineStart()
    {
        if (audioSource != null && engineStartSound != null)
        {
            audioSource.clip = engineStartSound;
            audioSource.loop = true; // Enable looping
            audioSource.Play();
            Debug.Log("Playing engine start sound cue.");
        }
    }

    public void PlayEngineClose()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.loop = false; // Disable looping
            audioSource.Stop();
            Debug.Log("Stopping engine sound.");
        }
    }
}
