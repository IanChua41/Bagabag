using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
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

    private Rigidbody rb;
    bool isDriving = false;
    float slowTimer = 0f;
    
    private float cachedVerticalInput = 0f;
    private float cachedHorizontalInput = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        baseForwardSpeed = forwardSpeed;
        baseReverseSpeed = reverseSpeed;
        baseForwardSteering = forwardSteering;
        baseReverseSteering = reverseSteering;
    }

    void Update()
    {
        if (!isDriving)
            return;

        // Cache input for FixedUpdate
        cachedVerticalInput = Input.GetAxis("Vertical");
        cachedHorizontalInput = Input.GetAxis("Horizontal");

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

    void FixedUpdate()
    {
        if (!isDriving)
            return;

        float verticalInput = cachedVerticalInput;
        float horizontalInput = cachedHorizontalInput;

        float currentSpeed = verticalInput >= 0f ? forwardSpeed : reverseSpeed;
        float currentSteering = verticalInput >= 0f ? forwardSteering : reverseSteering;

        // Move using Rigidbody velocity (preserves gravity)
        Vector3 moveDirection = transform.forward * verticalInput * currentSpeed;
        rb.linearVelocity = new Vector3(moveDirection.x, rb.linearVelocity.y, moveDirection.z);

        // reverse steering should be opposite when moving backward
        if (Mathf.Abs(verticalInput) > 0.01f && Mathf.Abs(horizontalInput) > 0.01f)
        {
            float steerDirection = verticalInput >= 0f ? 1f : -1f;
            float turnAmount = horizontalInput * currentSteering * Time.fixedDeltaTime * steerDirection;
            transform.Rotate(0f, turnAmount, 0f);
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

