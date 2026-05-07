using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField, Min(1)] private int maxSpawn = 3;
    [SerializeField] private float spawnInterval = 0f; // 0 = spawn all at Start

    private int spawnedCount = 0;

    private void Start()
    {
        if (enemyPrefab == null)
        {
            Debug.LogWarning("Enemy prefab not assigned.", this);
            return;
        }

        if (spawnInterval <= 0f)
        {
            for (int i = 0; i < maxSpawn; i++)
                Spawn();
        }
        else
        {
            StartCoroutine(SpawnRoutine());
        }
    }

    private System.Collections.IEnumerator SpawnRoutine()
    {
        while (spawnedCount < maxSpawn)
        {
            Spawn();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    public void Spawn()
    {
        if (spawnedCount >= maxSpawn) return;
        Instantiate(enemyPrefab, transform.position, Quaternion.identity);
        spawnedCount++;
    }
}
