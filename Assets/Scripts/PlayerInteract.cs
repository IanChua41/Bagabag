using TMPro;
using UnityEngine;

interface IInteractable 
{
    public void Interact();
}

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI tmp;
    
    private Transform interactorSource;
    private float interactRange = 10f;

    private bool isPrompting = false;

    private void Start()
    {
        interactorSource = GetComponent<Transform>();
    }

    private void Update()
    {
        Ray r = new Ray(interactorSource.position, interactorSource.forward);

        IInteractable interactObj = null;
        bool hasInteractable = false;

        if (Physics.Raycast(r, out RaycastHit hitInfo, interactRange))
        {
            hasInteractable = hitInfo.collider.TryGetComponent(out interactObj);
        }

        if (hasInteractable)
        {
            if (!isPrompting)
            {
                tmp.gameObject.SetActive(true);
                tmp.text = "Press E to interact";
                isPrompting = true;
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                interactObj.Interact();
            }
        }
        else if (isPrompting)
        {
            tmp.gameObject.SetActive(false);
            isPrompting = false;
        }
    }
}
