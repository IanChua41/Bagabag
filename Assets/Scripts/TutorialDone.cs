using UnityEngine;
using UnityEngine.SceneManagement; 

public class TutorialDone : MonoBehaviour
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
        if (other.CompareTag("Car"))
        {
            // Load the "Office Level" scene
            SceneManager.LoadScene(2);
        }
    }
}
