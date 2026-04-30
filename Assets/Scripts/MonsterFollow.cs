using UnityEngine;

[RequireComponent(typeof(Collider))]
public class MonsterFollow : MonoBehaviour
{
    public float followSpeed = 4f;
    public float checkpointClearance = 0.1f;

    private Transform playerTransform;

    void Start()
    {
        FindPlayer();

        if (playerTransform != null)
        {
            Checkpoint.RegisterDefaultCheckpointPosition(Checkpoint.OwnerType.Player, playerTransform.position);
        }

        Checkpoint.RegisterDefaultCheckpointPosition(Checkpoint.OwnerType.Monster, transform.position);
    }

    void Update()
    {
        if (playerTransform == null)
        {
            FindPlayer();
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, followSpeed * Time.deltaTime);
    }

    private void FindPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogWarning("MonsterFollow: No GameObject found with tag 'Player'.", this);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            TriggerCheckpointRespawn(collision.gameObject);   
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<CharacterController>().enabled = false; // disable player control during respawn
            TriggerCheckpointRespawn(other.gameObject);
            other.GetComponent<CharacterController>().enabled = true; // re-enable player control after respawn
        }
    }

    public void TriggerCheckpointRespawn(GameObject playerObject)
    {
        Debug.Log($"MonsterFollow: respawn requested for player object {playerObject.name}", this);

        bool playerTeleported = Checkpoint.TryTeleportToCheckpoint(playerObject, Checkpoint.OwnerType.Player, checkpointClearance);
        bool monsterTeleported = Checkpoint.TryTeleportToCheckpoint(gameObject, Checkpoint.OwnerType.Monster, checkpointClearance);

        if (!playerTeleported)
        {
            Debug.LogWarning($"Player did not teleport: player checkpoint is not set or invalid. playerObject={playerObject.name}", this);
        }

        if (!monsterTeleported)
        {
            Debug.LogWarning("Monster did not teleport: monster checkpoint is not set or invalid.", this);
        }

        if (playerTeleported && monsterTeleported)
        {
            Debug.Log("Player and Monster returned to checkpoints.", this);
        }
    }
}

