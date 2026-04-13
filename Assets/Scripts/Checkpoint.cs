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
        Vector3 destination;

        if (checkpoint == null)
        {
            if (ownerType == OwnerType.Player && hasDefaultPlayerCheckpoint)
            {
                destination = defaultPlayerCheckpointPosition;
            }
            else if (ownerType == OwnerType.Monster && hasDefaultMonsterCheckpoint)
            {
                destination = defaultMonsterCheckpointPosition;
            }
            else
            {
                Debug.LogWarning(ownerType + " checkpoint is not set.");
                return false;
            }

            destination.y += clearance;
            TeleportActor(actor, destination);
            Debug.Log($"{ownerType} checkpoint not set, using default start position {destination}.");
            return true;
        }

        destination = checkpoint.position;
        destination.y += clearance;
        TeleportActor(actor, destination);
        Debug.Log($"Teleported {actor.name} to {ownerType} checkpoint {checkpoint.name} at {destination}.");
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


