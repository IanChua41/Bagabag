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
            flashlight.enabled = !flashlight.enabled;
            isFlashlightOn = flashlight.enabled;

            if (isFlashlightOn)
            {
                ReducePlayerStamina(100); // Reduce stamina by 100 when toggled on
            }
        }

        if (isFlashlightOn)
        {
            DrainStaminaOverTime();
            ConstantRaycast(); // Continuously check for enemies
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
