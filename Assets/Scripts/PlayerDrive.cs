using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDrive : MonoBehaviour
{
    [Header("Drive Settings")]
    public float forwardSpeed = 10f;
    public float reverseSpeed = 4f;
    public float forwardSteering = 120f;
    public float reverseSteering = 50f;

    bool isDriving = false;
    float slowTimer = 0f;

    void Update()
    {
        if (!isDriving)
            return;

        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");

        float currentSpeed = verticalInput >= 0f ? forwardSpeed : reverseSpeed;
        float currentSteering = verticalInput >= 0f ? forwardSteering : reverseSteering;

        float moveAmount = verticalInput * currentSpeed * Time.deltaTime;

        // reverse steering should be opposite when moving backward
        if (Mathf.Abs(verticalInput) > 0.01f && Mathf.Abs(horizontalInput) > 0.01f)
        {
            float steerDirection = verticalInput >= 0f ? 1f : -1f;
            float turnAmount = horizontalInput * currentSteering * Time.deltaTime * steerDirection;
            transform.Rotate(0f, turnAmount, 0f);
        }

        transform.Translate(0f, 0f, moveAmount);

        if (Time.time > slowTimer)
        {
            forwardSpeed = 10f;
            forwardSteering = 120f;
            reverseSpeed = 4f;
            reverseSteering = 50f;
        }
        else
        {
            forwardSpeed = 2f;
            forwardSteering = 50f;
            reverseSpeed = 2f;
            reverseSteering = 25f;
        }
    }

    public void SetToDrive()
    {
        isDriving = true;
    }

    public void StopDrive()
    {
        isDriving = false;
    }
}
