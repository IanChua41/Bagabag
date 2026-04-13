using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public enum OwnerType
    {
        Player,
        Monster
    }

    [Header("Checkpoint Settings")]
    public OwnerType owner;
    public bool registerAsDefaultOnStart = true;
    public float teleportClearance = 0.1f;

    private static Transform playerCheckpoint;
    private static Transform monsterCheckpoint;
    private static Vector3 defaultPlayerCheckpointPosition;
    private static Vector3 defaultMonsterCheckpointPosition;
    private static bool hasDefaultPlayerCheckpoint;
    private static bool hasDefaultMonsterCheckpoint;

    public static Transform PlayerCheckpoint => playerCheckpoint;
    public static Transform MonsterCheckpoint => monsterCheckpoint;

    private void Start()
    {
        if (registerAsDefaultOnStart)
        {
            RegisterDefaultCheckpoint();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (owner == OwnerType.Player && other.CompareTag("Player"))
        {
            playerCheckpoint = transform;
            Debug.Log("Player checkpoint updated: " + name, this);
        }
        else if (owner == OwnerType.Monster && other.CompareTag("Monster"))
        {
            monsterCheckpoint = transform;
            Debug.Log("Monster checkpoint updated: " + name, this);
        }
    }

    private void RegisterDefaultCheckpoint()
    {
        if (owner == OwnerType.Player && playerCheckpoint == null)
        {
            playerCheckpoint = transform;
        }
        else if (owner == OwnerType.Monster && monsterCheckpoint == null)
        {
            monsterCheckpoint = transform;
        }
    }

    public static void RegisterDefaultCheckpointPosition(OwnerType ownerType, Vector3 position)
    {
        if (ownerType == OwnerType.Player)
        {
            defaultPlayerCheckpointPosition = position;
            hasDefaultPlayerCheckpoint = true;
        }
        else
        {
            defaultMonsterCheckpointPosition = position;
            hasDefaultMonsterCheckpoint = true;
        }
    }

    public static bool TryTeleportToCheckpoint(GameObject actor, OwnerType ownerType, float clearance)
    {
        Transform checkpoint = ownerType == OwnerType.Player ? playerCheckpoint : monsterCheckpoint;
        if (checkpoint == null)
        {
            if (ownerType == OwnerType.Player && hasDefaultPlayerCheckpoint)
            {
                Vector3 destination = defaultPlayerCheckpointPosition;
                destination.y += clearance;
                actor.transform.position = destination;
                Debug.Log($"Player checkpoint not set, using default start position {destination}.");
                return true;
            }
            else if (ownerType == OwnerType.Monster && hasDefaultMonsterCheckpoint)
            {
                Vector3 destination = defaultMonsterCheckpointPosition;
                destination.y += clearance;
                actor.transform.position = destination;
                Debug.Log($"Monster checkpoint not set, using default start position {destination}.");
                return true;
            }

            Debug.LogWarning(ownerType + " checkpoint is not set.");
            return false;
        }

        Vector3 checkpointDestination = checkpoint.position;
        checkpointDestination.y += clearance;
        actor.transform.position = checkpointDestination;
        Debug.Log($"Teleported {actor.name} to {ownerType} checkpoint {checkpoint.name} at {checkpointDestination}.");
        return true;
    }
}

