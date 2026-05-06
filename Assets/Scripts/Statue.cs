using UnityEngine;

public class Statue : MonoBehaviour, IInteractable
{
    public bool isFixed = false;
    
    public void Interact()
    {
        if (isFixed)
            Debug.Log("Statue is already fixed.");
        else
        {
            Debug.Log("Statue fixed.");
            isFixed = true;
        }
    }
}
