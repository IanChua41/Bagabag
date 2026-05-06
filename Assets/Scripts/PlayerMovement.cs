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
    public int startingStamina = 100;
    public int staminaDepletionRate = 50; // Stamina points depleted per second while sprinting
    public int staminaRecoveryRateWalking = 25; // Stamina points recovered per second while walking
    public int staminaRecoveryRateIdle = 50; // Stamina points recovered per second while idle
    private int currentStamina;

    [Header("References")]
    public StaminaBar staminaBar; // Reference to the stamina bar script

    private CharacterController controller;
    private Animator animator;
    private Vector3 velocity;
    private Vector2 inputDirection;
    private bool isSprinting;
    private bool jumpInput;
    private bool inSpotlight;

    [Header("Character Rotation")]
    public Transform orientation;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        currentStamina = startingStamina;

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
        UpdateAnimationState();
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
            velocity.y = -0.5f; // Small downward force to keep grounded without sinking into the floor
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

    void UpdateAnimationState()
    {
        if (animator == null)
        {
            return;
        }

        bool hasMoveInput = inputDirection.sqrMagnitude > 0.01f;
        bool canMove = currentStamina > 0;
        bool isRunning = isSprinting && hasMoveInput && canMove;
        bool isWalking = hasMoveInput && canMove && !isRunning;
        bool isIdle = !hasMoveInput || !canMove;

        animator.SetBool("isWalking", isWalking);
        animator.SetBool("isRunning", isRunning);
        animator.SetBool("isIdle", isIdle);
    }

    public void AddStamina(int amount)
    {
        // Add the amount to the player's actual stamina pool
        currentStamina += amount;

        // Clamp the stamina so it doesn't exceed the maximum
        if (currentStamina >= maxStamina)
        {
            currentStamina = maxStamina;
        }

        // Notify the StaminaBar to update the UI
        if (staminaBar != null)
        {
            staminaBar.OnStaminaRecovering(currentStamina);
        }
    }

    public void ReduceStamina(int amount)
    {
        currentStamina = Mathf.Clamp(currentStamina - amount, 0, maxStamina);
    }

    public void SetInSpotlight(bool state)
    {
        inSpotlight = state;
    }

    public bool IsInSpotlight()
    {
        return inSpotlight;
    }
}