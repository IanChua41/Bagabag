using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 5f;
    public float sprintSpeed = 8f;
    public float jumpHeight = 2f;
    public float gravity = -9.81f;

    private CharacterController controller;
    private Vector3 velocity;
    private Vector2 inputDirection;
    private bool isSprinting;
    private bool jumpInput;

    [Header("Character Rotation")]
    public Transform orientation;

    [Header("Stamina Settings")]
    public int maxStamina = 1000; // Maximum stamina
    public int staminaRecoveryRate = 20; // Stamina recovered per second
    public int sprintStaminaCost = 10; // Stamina cost per second while sprinting
    public int jumpStaminaCost = 50; // Stamina cost for jumping
    private int currentStamina;
    private bool canMove = true;

    [Header("References")]
    public StaminaBar staminaBar; // Reference to the stamina bar script

    private bool isRecoveringStamina;
    private bool inSpotlight; // Spotlight status

    void Start()
    {
        controller = GetComponent<CharacterController>();
        currentStamina = maxStamina;

        if (staminaBar != null)
        {
            staminaBar.SetMaxStamina(maxStamina);
            staminaBar.UpdateStamina(currentStamina);
        }
    }

    void Update()
    {
        if (canMove)
        {
            MovePlayer();
            HandleJump();
        }

        HandleStaminaRecovery();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        inputDirection = context.ReadValue<Vector2>();
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        isSprinting = context.ReadValueAsButton();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            jumpInput = true;
        }
    }

    void MovePlayer()
    {
        Vector3 move = orientation.right * inputDirection.x + orientation.forward * inputDirection.y;

        float speed = isSprinting && inputDirection.magnitude > 0 ? sprintSpeed : walkSpeed;

        if (isSprinting && inputDirection.magnitude > 0)
        {
            ReduceStamina(Mathf.RoundToInt(sprintStaminaCost * Time.deltaTime));
        }

        if (currentStamina <= 0)
        {
            canMove = false;
            return;
        }

        controller.Move(move * speed * Time.deltaTime);

        if (controller.isGrounded)
        {
            velocity.y = -2f;
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void HandleJump()
    {
        if (jumpInput && controller.isGrounded && currentStamina > 0)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            ReduceStamina(jumpStaminaCost);
            jumpInput = false;
        }
    }

    void HandleStaminaRecovery()
    {
        if (!isSprinting && currentStamina < maxStamina)
        {
            currentStamina += Mathf.RoundToInt(staminaRecoveryRate * Time.deltaTime);
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);

            if (staminaBar != null)
            {
                staminaBar.UpdateStamina(currentStamina);
            }

            if (currentStamina > 0)
            {
                canMove = true;
            }
        }
    }

    public void ReduceStamina(int amount)
    {
        currentStamina -= amount;
        currentStamina = Mathf.Max(currentStamina, 0);

        if (staminaBar != null)
        {
            staminaBar.UpdateStamina(currentStamina);
        }

        if (currentStamina <= 0)
        {
            canMove = false;
        }
    }

    public void SetInSpotlight(bool status)
    {
        inSpotlight = status;
        Debug.Log($"Player spotlight status set to: {inSpotlight}");
    }

    public bool IsInSpotlight()
    {
        return inSpotlight;
    }
}