using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float respawnTime = 5f;

    private GameObject currentEnemy;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        SpawnEnemy();
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    private void SpawnEnemy()
    {
        currentEnemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);

        EnemyHealth enemyHealth = currentEnemy.GetComponent<EnemyHealth>();

        enemyHealth.OnEnemyDeath += HandleEnemyDeath;
    }

    private void HandleEnemyDeath()
    {
        StartCoroutine(RespawnCoroutine());
    }

    private IEnumerator RespawnCoroutine()
    {
        yield return new WaitForSeconds(respawnTime);

        SpawnEnemy();
    }
}
