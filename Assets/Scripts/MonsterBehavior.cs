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

    void Start()
    {
        playerMovement = player.GetComponent<PlayerMovement>();
        if (playerMovement == null)
        {
            Debug.LogError("PlayerMovement script not found on the player GameObject.");
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

}
