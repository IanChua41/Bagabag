using UnityEngine;
using UnityEngine.SceneManagement; 

public class OfficeDone : MonoBehaviour
{
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Load scene 3
            SceneManager.LoadScene(5);
        }
    }
}
