using UnityEngine;

public class Statue : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject statue;
    [SerializeField] GameObject baseLight;
    [SerializeField] bool isFixed = false;

    private void Awake()
    {
        statue.SetActive(isFixed);
    }

    public void Interact()
    {
        if (!isFixed)
        {
            statue.SetActive(true);
            // baseLight.SetActive(false);

            Debug.Log("Statue fixed.");
            isFixed = true;

            GetComponent<Statue>().enabled = false;
        }
        else
        {
            Debug.Log("Follow the statue.");
        }
    }
}
