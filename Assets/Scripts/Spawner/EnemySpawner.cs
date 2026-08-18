using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float respawnTime = 5f;

    [Header("Spawn Points")]
    [SerializeField] private Transform[] spawnPoints; // ~ max enemies

    private GameObject[] spawnedEnemies;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        if (spawnPoints.Length == 0)
        {
            Debug.LogWarning("Chua co Spawn Point nao trong danh sach!");
            return;
        }

        // Tạo mảng có cùng số lượng với SpawnPoint
        spawnedEnemies = new GameObject[spawnPoints.Length];

        // Mỗi SpawnPoint spawn đúng 1 enemy
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            SpawnEnemy(i);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    private void SpawnEnemy(int spawnIndex)
    {
        // Kiểm tra index
        if (spawnIndex < 0 || spawnIndex >= spawnPoints.Length)
            return;

        // Nếu SpawnPoint này đang có enemy thì không spawn thêm
        if (spawnedEnemies[spawnIndex] != null)
            return;

        Transform spawnPoint = spawnPoints[spawnIndex];

        GameObject newEnemy = Instantiate(
            enemyPrefab,
            spawnPoint.position,
            Quaternion.identity
        );

        // Lưu enemy vào đúng SpawnPoint
        spawnedEnemies[spawnIndex] = newEnemy;

        // Đăng ký sự kiện chết
        EnemyHealth enemyHealth = newEnemy.GetComponent<EnemyHealth>();

        if (enemyHealth != null)
        {
            int index = spawnIndex;

            enemyHealth.OnEnemyDeath += () => HandleEnemyDeath(index);
        }
        else
        {
            Debug.LogWarning("Enemy prefab khong co EnemyHealth!");
        }
    }

    private void HandleEnemyDeath(int spawnIndex)
    {
        // Xóa reference enemy cũ
        spawnedEnemies[spawnIndex] = null;

        // Chờ respawn
        StartCoroutine(RespawnCoroutine(spawnIndex));
    }

    private IEnumerator RespawnCoroutine(int spawnIndex)
    {
        yield return new WaitForSeconds(respawnTime);

        // Spawn lại đúng SpawnPoint cũ
        SpawnEnemy(spawnIndex);
    }
}
