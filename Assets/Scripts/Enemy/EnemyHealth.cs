using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    private EnemyStats stats;

    private int currentHp;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        stats = GetComponent<EnemyStats>();

        currentHp = stats.maxHP;
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        currentHp -= damage;

        if (currentHp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        PlayerStats player = FindFirstObjectByType<PlayerStats>();

        if (player != null)
        {
            player.GainExp(stats.expReward);
        }

        if (stats.dropPrefab != null)
        {
            Instantiate(stats.dropPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}
