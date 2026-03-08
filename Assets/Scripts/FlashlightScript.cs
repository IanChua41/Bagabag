using UnityEngine;

public class FlashlightScript : MonoBehaviour
{
    public KeyCode toggleKey = KeyCode.F; // Key to toggle the flashlight
    private Light flashlight; // Reference to the Light component

    void Start()
    {
        flashlight = GetComponent<Light>();

        if (flashlight == null)
        {
            Debug.LogError("No Light component found on the GameObject. Please attach a Light component.");
        }
    }


    void Update()
    {

        if (Input.GetKeyDown(toggleKey) && flashlight != null)
        {

            flashlight.enabled = !flashlight.enabled;
        }
    }
}
