using UnityEngine;
using UnityEngine.SceneManagement; 

public class OfficeTransition : MonoBehaviour
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
            // Load the "Office Level" scene
            SceneManager.LoadScene(3);
        }
    }
}
