using UnityEngine;

[RequireComponent(typeof(Light))]
[RequireComponent(typeof(Collider))]
public class FlashlightScript : MonoBehaviour
{
    public KeyCode toggleKey = KeyCode.F;
    private Light flashlight;
    private bool isFlashlightOn = false;

    void Start()
    {
        flashlight = GetComponent<Light>();

        if (flashlight == null)
        {
            Debug.LogError("No Light component found on the GameObject. Please attach a Light component.");
        }

        Collider collider = GetComponent<Collider>();
        if (collider != null && !collider.isTrigger)
        {
            collider.isTrigger = true;
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
}
