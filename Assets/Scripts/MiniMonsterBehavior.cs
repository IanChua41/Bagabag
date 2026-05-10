using UnityEngine;

public class MiniMonsterBehavior : MonoBehaviour
{
    [Header("Monster Settings")]
    public Transform player;
    public float followSpeed = 1f;
    public float retreatDistance = 5f;
    public float retreatSpeed = 5f;

    [Header("Health")]
    [SerializeField] private int maxHealth = 3;

    [Header("Trigger Settings")]
    [SerializeField] private string triggerTag = "Player";
    [SerializeField] private bool stopFollowingOnTriggerExit = true;

    private Vector3 retreatTarget;
    private bool isRetreating = false;
    private bool inSpotlight = false;
    private bool isPlayerInTrigger = false;
    private int currentHealth;

    private PlayerMovement playerMovement;

    void Start()
    {
        currentHealth = maxHealth;

        if (player != null)
        {
            playerMovement = player.GetComponent<PlayerMovement>();
        }

        if (playerMovement == null)
        {
            Debug.LogError("PlayerMovement script not found on the player GameObject.");
        }
    }

    void Update()
    {
        if (player == null)
        {
            return;
        }

        FacePlayer();

        if ((playerMovement != null && playerMovement.IsInSpotlight()) || inSpotlight)
        {
            if (!isRetreating)
            {
                StartRetreat();
            }
            HandleRetreat();
            return;
        }

        if (isPlayerInTrigger)
        {
            FollowPlayer();
        }
    }

    public void StartRetreat()
    {
        Vector3 directionAwayFromPlayer = (transform.position - player.position).normalized;
        retreatTarget = transform.position + directionAwayFromPlayer * retreatDistance;
        isRetreating = true;
    }

    public void HandleRetreat()
    {
        if (isRetreating)
        {
            transform.position = Vector3.MoveTowards(transform.position, retreatTarget, retreatSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, retreatTarget) < 0.1f)
            {
                isRetreating = false;
                inSpotlight = false;
            }
        }
    }

    public void FollowPlayer()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        transform.position += directionToPlayer * followSpeed * Time.deltaTime;
    }

    public void TakeDamage(int damage = 1)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    private void FacePlayer()
    {
        if (player == null)
        {
            return;
        }

        Vector3 directionToPlayer = player.position - transform.position;
        directionToPlayer.y = 0f;

        if (directionToPlayer.sqrMagnitude > 0.0001f)
        {
            transform.rotation = Quaternion.LookRotation(directionToPlayer.normalized);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(triggerTag))
        {
            isPlayerInTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(triggerTag) && stopFollowingOnTriggerExit)
        {
            isPlayerInTrigger = false;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Car"))
        {
            // Teleport both player/car and monster to the latest checkpoint
            Checkpoint.TryTeleportToCheckpoint(other.gameObject, Checkpoint.OwnerType.Player, 0.1f);
            Checkpoint.TryTeleportToCheckpoint(gameObject, Checkpoint.OwnerType.Monster, 0.1f);
            Debug.Log("Player caught! Teleporting to checkpoint...");
        }
    }
}
