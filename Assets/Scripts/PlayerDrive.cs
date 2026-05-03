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

    private float baseForwardSpeed;
    private float baseReverseSpeed;
    private float baseForwardSteering;
    private float baseReverseSteering;

    bool isDriving = false;
    float slowTimer = 0f;

    void Start()
    {
        baseForwardSpeed = forwardSpeed;
        baseReverseSpeed = reverseSpeed;
        baseForwardSteering = forwardSteering;
        baseReverseSteering = reverseSteering;
    }

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
            forwardSpeed = baseForwardSpeed;
            forwardSteering = baseForwardSteering;
            reverseSpeed = baseReverseSpeed;
            reverseSteering = baseReverseSteering;
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

    public void SetSpeeds(float fwd, float rev, float fwdSteer, float revSteer)
    {
        forwardSpeed = fwd;
        reverseSpeed = rev;
        forwardSteering = fwdSteer;
        reverseSteering = revSteer;
        baseForwardSpeed = fwd;
        baseReverseSpeed = rev;
        baseForwardSteering = fwdSteer;
        baseReverseSteering = revSteer;
    }
}

