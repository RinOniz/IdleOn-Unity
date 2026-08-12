using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float respawnTime = 5f;
    [SerializeField] private int maxEnemies = 5;

    [Header("Spawn Points")]
    [SerializeField] private Transform[] spawnPoints;

    private int currentEnemyCount = 0;  

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        for (int i = 0; i < maxEnemies; i++)
        {
            SpawnEnemy();
        }
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    private void SpawnEnemy()
    {
        if (spawnPoints.Length == 0)
        {
            Debug.LogWarning("Chua co Spawn Point nao trong danh sach!");
            return;
        }

        Transform randomPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        GameObject newEnemy = Instantiate(enemyPrefab, randomPoint.position, Quaternion.identity);
        currentEnemyCount++;

        EnemyHealth enemyHealth = newEnemy.GetComponent<EnemyHealth>();

        if (enemyHealth != null)
        {
            enemyHealth.OnEnemyDeath += HandleEnemyDeath;
        }
    }

    private void HandleEnemyDeath()
    {
        currentEnemyCount--;
        StartCoroutine(RespawnCoroutine());
    }

    private IEnumerator RespawnCoroutine()
    {
        yield return new WaitForSeconds(respawnTime);

        if (currentEnemyCount < maxEnemies)
        {
            SpawnEnemy();
        }
    }
}
