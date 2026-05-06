using UnityEngine;

[RequireComponent(typeof(Light))]
public class FlashlightScript : MonoBehaviour
{
    public KeyCode toggleKey = KeyCode.F;
    private Light flashlight;
    private bool isFlashlightOn = false;

    [Header("Raycast Settings")]
    public Camera fpsCamera; // Reference to the player's camera
    public float range = 20f; // Maximum range of the raycast
    public LayerMask Enemy; // Layer mask to detect enemies

    [Header("Sound Settings")]
    [SerializeField] private AudioClip flashlightToggleSound;
    [SerializeField] private AudioSource audioSource;

    [Header("Flashlight Settings")]
    [SerializeField] private float flashlightToggleDelay = 1.0f; // Delay in seconds before the flashlight turns on

    void Start()
    {
        flashlight = GetComponent<Light>();

        if (flashlight == null)
        {
            Debug.LogError("No Light component found on the GameObject. Please attach a Light component.");
        }

        if (fpsCamera == null)
        {
            Debug.LogError("No Camera assigned to FlashlightScript. Please assign the player's camera in the Inspector.");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(toggleKey) && flashlight != null)
        {
            if (!isFlashlightOn)
            {
                PlayFlashlightToggleSound();
                Invoke(nameof(ToggleFlashlight), flashlightToggleDelay); // Delay flashlight toggle by 1 second
            }
            else
            {
                ToggleFlashlight(); // Turn off immediately
            }
        }

        if (isFlashlightOn)
        {
            DrainStaminaOverTime();
            ConstantRaycast(); // Continuously check for enemies
        }
    }

    void ToggleFlashlight()
    {
        flashlight.enabled = !flashlight.enabled;
        isFlashlightOn = flashlight.enabled;

        if (isFlashlightOn)
        {
            ReducePlayerStamina(100); // Reduce stamina by 100 when toggled on
        }
    }

    void PlayFlashlightToggleSound()
    {
        if (audioSource != null && flashlightToggleSound != null)
        {
            audioSource.PlayOneShot(flashlightToggleSound);
        }
        else
        {
            Debug.LogWarning("AudioSource or FlashlightToggleSound is not assigned.");
        }
    }

    void ReducePlayerStamina(int amount)
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                playerMovement.ReduceStamina(amount);
            }
        }
    }

    void DrainStaminaOverTime()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                playerMovement.ReduceStamina(Mathf.RoundToInt(5 * Time.deltaTime)); // Drain 5 stamina per second
            }
        }
    }

    void ConstantRaycast()
    {
        if (fpsCamera == null)
        {
            Debug.LogError("No Camera assigned to FlashlightScript. Please assign the player's camera in the Inspector.");
            return;
        }

        Ray ray = new Ray(fpsCamera.transform.position, fpsCamera.transform.forward);
        RaycastHit hit;

        // Visualize the raycast in the Scene view
        Debug.DrawRay(fpsCamera.transform.position, fpsCamera.transform.forward * range, Color.red);

        if (Physics.Raycast(ray, out hit, range, Enemy))
        {
            Debug.Log($"Raycast hit: {hit.transform.name}");

            MonsterBehavior monster = hit.collider.GetComponent<MonsterBehavior>();
            if (monster != null)
            {
                monster.HandleRetreat(); 
                Debug.Log($"Monster {hit.transform.name} is retreating.");
            }
        }
        else
        {
            Debug.Log("Raycast did not hit any object.");
        }
    }
}
