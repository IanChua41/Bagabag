using UnityEngine;
using UnityEngine.SceneManagement;

public class Totem : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        Debug.Log("Level finished.");

        SceneManager.LoadScene("Level3");
    }
}
