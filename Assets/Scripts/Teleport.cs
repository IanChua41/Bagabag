using UnityEngine;

public class Teleport : MonoBehaviour
{
    [SerializeField] Transform destination;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<CharacterController>().enabled = false;
            other.transform.position = destination.position;
            other.transform.rotation = destination.rotation;
            other.GetComponent<CharacterController>().enabled = true;
        }
            
    }
}
