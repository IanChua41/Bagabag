using UnityEngine;

public class MonsterDetectionZone : MonoBehaviour
{
    [Header("Detection Settings")]
    public float detectionRadius = 10f;
    public string playerTag = "Player";

    private MonsterBehavior monsterBehavior;
    private bool hasDetectedPlayer = false;

    void Start()
    {
        // Get the MonsterBehavior component on this GameObject
        monsterBehavior = GetComponent<MonsterBehavior>();
        if (monsterBehavior == null)
        {
            Debug.LogError("MonsterDetectionZone requires MonsterBehavior script on the same GameObject.");
        }

        // Create and setup the trigger collider if it doesn't exist
        SetupTriggerCollider();
    }

    void SetupTriggerCollider()
    {
        // Check if a trigger collider already exists
        Collider[] colliders = GetComponents<Collider>();
        bool hasTrigger = false;

        foreach (Collider col in colliders)
        {
            if (col.isTrigger)
            {
                hasTrigger = true;
                break;
            }
        }

        // If no trigger exists, create a sphere collider
        if (!hasTrigger)
        {
            SphereCollider triggerCollider = gameObject.AddComponent<SphereCollider>();
            triggerCollider.radius = detectionRadius;
            triggerCollider.isTrigger = true;
            Debug.Log("Created trigger collider with radius: " + detectionRadius);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag) && !hasDetectedPlayer)
        {
            hasDetectedPlayer = true;
            Debug.Log("Player detected! Monster will now follow.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            hasDetectedPlayer = false;
            Debug.Log("Player left detection zone!");
        }
    }

    public bool HasDetectedPlayer()
    {
        return hasDetectedPlayer;
    }

    // Allow updating the detection radius at runtime if needed
    public void SetDetectionRadius(float newRadius)
    {
        detectionRadius = newRadius;
        SphereCollider triggerCollider = GetComponent<SphereCollider>();
        if (triggerCollider != null)
        {
            triggerCollider.radius = newRadius;
        }
    }
}
