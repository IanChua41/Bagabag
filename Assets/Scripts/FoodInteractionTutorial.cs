using UnityEngine;

public class FoodInteractionTutorial : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Yum! Delicious food!");

            // Destroy the food object
            Destroy(gameObject);
        }
    }    
}
