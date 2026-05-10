using UnityEngine;
using UnityEngine.SceneManagement;

public class Level2Reload : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ReloadLevelScene();
        }
    }

    public void ReloadLevelScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
