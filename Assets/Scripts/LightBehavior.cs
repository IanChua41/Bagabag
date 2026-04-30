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
    private bool playerInsideTrigger = false;
    private PlayerMovement cachedPlayerMovement;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInsideTrigger = true;
            inSpotlight = true;
            lightOff = false;
            lightInitialBufferTime = 0.0f;
            Debug.Log("Player has entered the trigger");

            cachedPlayerMovement = other.GetComponent<PlayerMovement>();
            if (cachedPlayerMovement != null)
            {
                cachedPlayerMovement.SetInSpotlight(true);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInsideTrigger = false;
            inSpotlight = false;
            lightOff = true;
            lightInitialTime = 0.0f;
            Debug.Log("Player has exited the trigger");

            // Set the player's inSpotlight status to false
            if (cachedPlayerMovement != null)
            {
                cachedPlayerMovement.SetInSpotlight(false);
            }
            cachedPlayerMovement = null;
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

        if (cachedPlayerMovement != null)
        {
            bool playerInActiveLight = playerInsideTrigger && spotLight.activeSelf && !lightOff;
            cachedPlayerMovement.SetInSpotlight(playerInActiveLight);
        }
    }
}
