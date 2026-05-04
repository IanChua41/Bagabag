using UnityEngine;

public class Statue : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        Debug.Log("Statue fixed.");
    }
}
