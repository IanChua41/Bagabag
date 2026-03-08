using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    [Header("Stamina Bar Settings")]
    public Image staminaBarImage; 

    private float maxStamina;


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

    public void OnStaminaDepleting(float currentStamina)
    {
        UpdateStamina(currentStamina);
    }

    public void OnStaminaRecovering(float currentStamina)
    {
        UpdateStamina(currentStamina);
    }
}