using UnityEngine;

public class Cabinet : MonoBehaviour
{
    AudioPlayer audioPlayer;

    void Awake() 
    {
        audioPlayer = FindFirstObjectByType<AudioPlayer>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Need to wear something else?");

            // Destroy the food object
            audioPlayer.PlayCabinetClip();
        }
    }    
}
