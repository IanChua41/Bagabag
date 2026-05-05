using UnityEngine;

public class MonsterBehavior : MonoBehaviour
{
    [Header("Monster Settings")]
    public Transform player;
    public float followSpeed = 1f;
    public float retreatDistance = 5f;
    public float retreatSpeed = 5f;

    private Vector3 retreatTarget;
    private bool isRetreating = false;
    private bool inSpotlight = false;

    private PlayerMovement playerMovement;
    private Rigidbody rb;

    void Start()
    {
        playerMovement = player.GetComponent<PlayerMovement>();
        if (playerMovement == null)
        {
            Debug.LogError("PlayerMovement script not found on the player GameObject.");
        }

        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogWarning("Rigidbody not found on monster. Adding one for proper collision detection.");
            rb = gameObject.AddComponent<Rigidbody>();
            rb.isKinematic = true;
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }
    }

    void Update()
    {
        if ((playerMovement != null && playerMovement.IsInSpotlight()) || inSpotlight)
        {
            if (!isRetreating)
            {
                StartRetreat();
            }
            HandleRetreat();
        }
        else
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
            Vector3 direction = (retreatTarget - transform.position).normalized;
            rb.MovePosition(transform.position + direction * retreatSpeed * Time.deltaTime);

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
        rb.MovePosition(transform.position + directionToPlayer * followSpeed * Time.deltaTime);
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
