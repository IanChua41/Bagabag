using UnityEngine;

public class LightBehavior : MonoBehaviour
{
    [SerializeField] private GameObject spotLight;
    [SerializeField] private float lightWaitTime = 5.0f;
    private float lightBufferTimer = 5.0f;
    private float lightInitialBufferTime = 0.0f;
    private float lightInitialTime = 0.0f;
    private bool inSpotlight = false;
    private bool lightOff = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            inSpotlight = true;
            Debug.Log("Player has entered the trigger");

            PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                playerMovement.SetInSpotlight(true);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            lightOff = true;
            lightInitialTime = 0.0f;
            Debug.Log("Player has exited the trigger");

            // Set the player's inSpotlight status to false
            PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                playerMovement.SetInSpotlight(false);
            }
        }
    }

    void Update()
    {
        if (inSpotlight)
        {
            lightInitialTime += Time.deltaTime;
            if (lightInitialTime >= lightWaitTime)
            {
                spotLight.SetActive(false);
                inSpotlight = false;
            }
        }

        if (lightOff)
        {
            lightInitialBufferTime += Time.deltaTime;
            if (lightInitialBufferTime >= lightBufferTimer)
            {
                spotLight.SetActive(true);
                lightOff = false;
            }
        }
    }
}
