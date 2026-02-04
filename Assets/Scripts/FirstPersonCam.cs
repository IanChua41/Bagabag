using Unity.Cinemachine;
using UnityEngine;

public class FirstPerson : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Transform playerObj;
    public Transform player;

    [Header("Settings")]
    public float mouseSensitivity = 200f;
    public float rotationSpeed = 10f;
    public float maxLookAngle = 80f;

    float xRotation; // vertical look

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnEnable()
    {
        Debug.Log("Switched to First Person POV.");
    }

    private void Update()
    {
        // mouse movement input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        //Debug.Log($"Mouse X: {mouseX}, Mouse Y: {mouseY}");

        // vertical look (up and down)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -maxLookAngle, maxLookAngle);

        // apply vertical rotation to head
        orientation.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        player.transform.Rotate(Vector3.up * mouseX);

        // player model rotation (left and right)
        Vector3 flatForward = player.transform.forward;
        flatForward.y = 0f;

        if (flatForward != Vector3.zero)
        {
            playerObj.forward = Vector3.Slerp(
                playerObj.forward,
                flatForward.normalized,
                Time.deltaTime * rotationSpeed
            );
        }
    }
}

