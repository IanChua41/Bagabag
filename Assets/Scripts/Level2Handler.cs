using UnityEngine;
using System.Collections;

public class Level2Handler : MonoBehaviour
{
    [Header("Monster Spawn Time")]
    [SerializeField] GameObject monsterPrefab;
    [SerializeField] float seconds = 30f;

    private void Awake()
    {
        StartCoroutine(Start());
    }

    private IEnumerator Start()
    {
        Debug.Log($"Waiting for {seconds} seconds...");

        yield return new WaitForSeconds(seconds);

        Debug.Log($"{seconds} seconds have passed!");

        //monster spawn
        monsterPrefab.SetActive(true);
    }
}
