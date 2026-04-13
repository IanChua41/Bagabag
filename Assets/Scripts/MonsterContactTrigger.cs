using UnityEngine;

[RequireComponent(typeof(Collider))]
public class MonsterContactTrigger : MonoBehaviour
{
    private MonsterFollow monsterFollow;

    private void Awake()
    {
        monsterFollow = GetComponentInParent<MonsterFollow>();
        if (monsterFollow == null)
        {
            Debug.LogWarning("MonsterContactTrigger requires a MonsterFollow component in a parent GameObject.", this);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (monsterFollow == null)
        {
            return;
        }

        if (other.CompareTag("Player"))
        {
            GameObject playerRoot = GetPlayerRoot(other.transform);
            Debug.Log($"MonsterContactTrigger: triggered by {other.gameObject.name}, resolved player root {playerRoot.name}", this);
            monsterFollow.TriggerCheckpointRespawn(playerRoot);
        }
        else
        {
            Debug.Log($"MonsterContactTrigger: trigger entered by non-player {other.gameObject.name} (tag={other.gameObject.tag})", this);
        }
    }

    private GameObject GetPlayerRoot(Transform transform)
    {
        Transform current = transform;
        GameObject result = null;

        while (current != null)
        {
            if (current.CompareTag("Player"))
            {
                result = current.gameObject;
            }
            current = current.parent;
        }

        if (result != null)
        {
            return result;
        }

        if (transform.root.CompareTag("Player"))
        {
            return transform.root.gameObject;
        }

        return transform.gameObject;
    }
}
