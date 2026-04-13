using UnityEngine;

public class FoodReplenishment : MonoBehaviour
{
    // Set to 1000 to completely fill the bar
    public int replenishmentRate = 1000;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Get the PlayerMovement script attached to the Player
            PlayerMovement player = collision.GetComponent<PlayerMovement>();

            if (player != null)
            {
                Debug.Log("Yum! Stamina recovered!");
                
                // Add the stamina
                player.AddStamina(replenishmentRate);
                
                // Destroy the food object
                Destroy(gameObject);
            }
        }
    }    
}