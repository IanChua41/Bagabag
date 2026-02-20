using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    [Header("Stamina Bar Settings")]
    public Image staminaBarImage; // Reference to the UI Image for the stamina bar

    private float maxStamina;

    // Sets the maximum stamina value and initializes the stamina bar
    public void SetMaxStamina(float maxStaminaValue)
    {
        maxStamina = maxStaminaValue;
        UpdateStamina(maxStamina); // Initialize the bar to full
    }

    // Updates the stamina bar UI based on the current stamina value
    public void UpdateStamina(float currentStamina)
    {
        if (staminaBarImage != null)
        {
            staminaBarImage.fillAmount = currentStamina / maxStamina;
        }
    }

    // Called when stamina is depleting
    public void OnStaminaDepleting(float currentStamina)
    {
        UpdateStamina(currentStamina);
    }

    // Called when stamina is recovering
    public void OnStaminaRecovering(float currentStamina)
    {
        UpdateStamina(currentStamina);
    }
}