using UnityEngine;

interface IInteractable 
{
    public void Interact();
}

public class PlayerInteract : MonoBehaviour
{
    private Transform interactorSource;
    private float interactRange = 10f;

    private void Start()
    {
        interactorSource = GetComponent<Transform>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray r = new Ray(interactorSource.position, interactorSource.forward);
            if (Physics.Raycast(r, out RaycastHit hitInfo, interactRange))
            {
                if (hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactObj))
                {
                    interactObj.Interact();
                }
            }
        }
    }
}
