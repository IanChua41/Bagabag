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
    public int checkpointIndex; // 0, 1, or 2 for checkpoints 1, 2, 3
    public bool registerAsDefaultOnStart = true;
    public float teleportClearance = 0.1f;

    private static Transform[] playerCheckpoints = new Transform[3];
    private static Transform[] monsterCheckpoints = new Transform[3];
    private static Vector3[] defaultPlayerCheckpointPositions = new Vector3[3];
    private static Vector3[] defaultMonsterCheckpointPositions = new Vector3[3];
    private static bool[] hasDefaultPlayerCheckpoints = new bool[3];
    private static bool[] hasDefaultMonsterCheckpoints = new bool[3];

    public static Transform[] PlayerCheckpoints => playerCheckpoints;
    public static Transform[] MonsterCheckpoints => monsterCheckpoints;

    private void Start()
    {
        if (registerAsDefaultOnStart)
        {
            RegisterDefaultCheckpoint();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (owner == OwnerType.Player && (other.CompareTag("Player") || other.CompareTag("Car")))
        {
            playerCheckpoints[checkpointIndex] = transform;
            Debug.Log("Player checkpoint " + (checkpointIndex + 1) + " updated: " + name, this);
            Debug.Log($"Player is now registered at checkpoint {(checkpointIndex + 1)} position: {transform.position}");
        }
        else if (owner == OwnerType.Monster && other.CompareTag("Monster"))
        {
            monsterCheckpoints[checkpointIndex] = transform;
            Debug.Log("Monster checkpoint " + (checkpointIndex + 1) + " updated: " + name, this);
            Debug.Log($"Monster is now registered at checkpoint {(checkpointIndex + 1)} position: {transform.position}");
        }
    }

    private void RegisterDefaultCheckpoint()
    {
        if (owner == OwnerType.Player && playerCheckpoints[checkpointIndex] == null)
        {
            playerCheckpoints[checkpointIndex] = transform;
        }
        else if (owner == OwnerType.Monster && monsterCheckpoints[checkpointIndex] == null)
        {
            monsterCheckpoints[checkpointIndex] = transform;
        }
    }

    public static void RegisterDefaultCheckpointPosition(OwnerType ownerType, int checkpointIndex, Vector3 position)
    {
        if (checkpointIndex < 0 || checkpointIndex >= 3)
        {
            Debug.LogWarning("Checkpoint index out of range: " + checkpointIndex);
            return;
        }

        if (ownerType == OwnerType.Player)
        {
            defaultPlayerCheckpointPositions[checkpointIndex] = position;
            hasDefaultPlayerCheckpoints[checkpointIndex] = true;
        }
        else
        {
            defaultMonsterCheckpointPositions[checkpointIndex] = position;
            hasDefaultMonsterCheckpoints[checkpointIndex] = true;
        }
    }

    /// <summary>
    /// Gets the latest checkpoint index reached by either player or monster.
    /// </summary>
    private static int GetLatestCheckpointIndex()
    {
        for (int i = 2; i >= 0; i--)
        {
            if (playerCheckpoints[i] != null || monsterCheckpoints[i] != null)
            {
                Debug.Log("Latest checkpoint index found: " + (i + 1));
                return i;
            }
        }
        Debug.Log("No checkpoints reached, defaulting to checkpoint 1");
        return 0; // Default to first checkpoint
    }

    public static bool TryTeleportToCheckpoint(GameObject actor, OwnerType ownerType, float clearance)
    {
        int checkpointIndex = GetLatestCheckpointIndex();
        Debug.Log($"TryTeleportToCheckpoint called for {actor.name} ({ownerType}) - targeting checkpoint {(checkpointIndex + 1)}");
        
        Transform checkpointPlayer = playerCheckpoints[checkpointIndex];
        Transform checkpointMonster = monsterCheckpoints[checkpointIndex];

        if (checkpointPlayer == null || checkpointMonster == null)
        {
            // Try to use default positions if checkpoint is not set
            Vector3 playerDestination = Vector3.zero;
            Vector3 monsterDestination = Vector3.zero;
            bool canTeleport = true;

            if (checkpointPlayer == null)
            {
                if (hasDefaultPlayerCheckpoints[checkpointIndex])
                {
                    playerDestination = defaultPlayerCheckpointPositions[checkpointIndex];
                }
                else
                {
                    Debug.LogWarning("Player checkpoint " + (checkpointIndex + 1) + " is not set.");
                    canTeleport = false;
                }
            }
            else
            {
                playerDestination = checkpointPlayer.position;
            }

            if (checkpointMonster == null)
            {
                if (hasDefaultMonsterCheckpoints[checkpointIndex])
                {
                    monsterDestination = defaultMonsterCheckpointPositions[checkpointIndex];
                }
                else
                {
                    Debug.LogWarning("Monster checkpoint " + (checkpointIndex + 1) + " is not set.");
                    canTeleport = false;
                }
            }
            else
            {
                monsterDestination = checkpointMonster.position;
            }

            if (!canTeleport)
                return false;
        }

        // Teleport the actor to the latest checkpoint
        Transform targetCheckpoint = (ownerType == OwnerType.Player) ? playerCheckpoints[checkpointIndex] : monsterCheckpoints[checkpointIndex];
        if (targetCheckpoint == null)
        {
            Vector3 defaultPos = (ownerType == OwnerType.Player) 
                ? defaultPlayerCheckpointPositions[checkpointIndex] 
                : defaultMonsterCheckpointPositions[checkpointIndex];
            defaultPos.y += clearance;
            TeleportActor(actor, defaultPos);
            Debug.Log($"Teleported {actor.name} to default checkpoint {(checkpointIndex + 1)} position: {defaultPos}");
        }
        else
        {
            Vector3 destination = targetCheckpoint.position;
            destination.y += clearance;
            TeleportActor(actor, destination);
            Debug.Log($"Teleported {actor.name} to checkpoint {(checkpointIndex + 1)} ({targetCheckpoint.name}) at position: {destination}");
        }

        Debug.Log($"Teleported {actor.name} to checkpoint {(checkpointIndex + 1)} at latest checkpoint.");
        return true;
    }

    private static void TeleportActor(GameObject actor, Vector3 destination)
    {
        CharacterController controller = actor.GetComponent<CharacterController>();
        bool hadController = controller != null;

        if (hadController)
        {
            controller.enabled = false;
        }

        actor.transform.position = destination;

        if (hadController)
        {
            controller.enabled = true;
        }
    }
}


