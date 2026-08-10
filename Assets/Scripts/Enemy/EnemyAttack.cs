using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private float attackCooldown = 1.0f;

    private EnemyStats enemyStats;
    
    private float lastAttackTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        enemyStats = GetComponentInParent<EnemyStats>();
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (Time.time >= lastAttackTime + attackCooldown)
            {
                PlayerStats playerStats = collision.GetComponent<PlayerStats>();

                if (playerStats != null)
                {
                    playerStats.TakeDamage(enemyStats.attack);

                    lastAttackTime = Time.time;
                }
            }
        }
    }
}
