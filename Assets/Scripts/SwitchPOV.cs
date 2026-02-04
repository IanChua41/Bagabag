using Unity.VisualScripting;
using UnityEngine;

public class SwitchPOV : MonoBehaviour
{
    public bool firstPerson = false;

    [SerializeField] GameObject firstPersonCam;
    [SerializeField] GameObject thirdPersonCam;

    private void Start()
    {
        firstPersonCam.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            firstPerson = !firstPerson;

            firstPersonCam.SetActive(firstPerson);
        }
    }
}
