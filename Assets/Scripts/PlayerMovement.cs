using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 5f;
    public float sprintSpeed = 8f;
    public float jumpHeight = 2f;
    public float gravity = -9.81f;

    [Header("Stamina Settings")]
    public int maxStamina = 1000; // Maximum stamina points
    public int staminaDepletionRate = 50; // Stamina points depleted per second while sprinting
    public int staminaRecoveryRateWalking = 50; // Stamina points recovered per second while walking
    public int staminaRecoveryRateIdle = 100; // Stamina points recovered per second while idle
    private int currentStamina;

    [Header("References")]
    public StaminaBar staminaBar; // Reference to the stamina bar script

    private CharacterController controller;
    private Vector3 velocity;
    private Vector2 inputDirection;
    private bool isSprinting;
    private bool jumpInput;

    [Header("Character Rotation")]
    public Transform orientation;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        currentStamina = maxStamina; // Initialize stamina to full

        if (staminaBar != null)
        {
            staminaBar.SetMaxStamina(maxStamina);
        }
    }

    void Update()
    {
        MovePlayer();
        HandleJumpAndGravity();
        RecoverStamina();
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

        // Handle sprinting and stamina
        if (isSprinting && inputDirection.magnitude > 0)
        {
            currentStamina -= Mathf.RoundToInt(staminaDepletionRate * Time.deltaTime);
            if (currentStamina <= 0)
            {
                currentStamina = 0;
            }

            // Notify StaminaBar about stamina depletion
            staminaBar?.OnStaminaDepleting(currentStamina);
        }

        // Disable horizontal movement when stamina is depleted
        if (currentStamina > 0)
        {
            float speed = isSprinting ? sprintSpeed : walkSpeed;
            controller.Move(move * speed * Time.deltaTime);
        }
    }

    void HandleJumpAndGravity()
    {
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Small downward force to keep grounded
        }

        if (jumpInput && controller.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            jumpInput = false; // Reset jump input after jumping
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void RecoverStamina()
    {
        // Recover stamina only when not sprinting
        if (!isSprinting)
        {
            int recoveryRate = inputDirection.magnitude > 0 ? staminaRecoveryRateWalking : staminaRecoveryRateIdle;

            currentStamina += Mathf.RoundToInt(recoveryRate * Time.deltaTime);
            if (currentStamina >= maxStamina)
            {
                currentStamina = maxStamina;
            }

            // Notify StaminaBar about stamina recovery
            staminaBar?.OnStaminaRecovering(currentStamina);
        }
    }
}