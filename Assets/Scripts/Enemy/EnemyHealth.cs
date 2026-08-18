using UnityEngine;
using System;

public class EnemyHealth : MonoBehaviour
{
    private EnemyStats stats;
    private Animator animator;

    public int currentHp;

    public event Action OnEnemyDeath;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        stats = GetComponent<EnemyStats>();
        animator = GetComponentInChildren<Animator>();

        currentHp = stats.maxHP;
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        currentHp -= damage;

        //if (animator != null)
        //{
        //    animator.SetTrigger("Hit");
        //}

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

        OnEnemyDeath?.Invoke();

        if (animator != null)
        {
            animator.SetTrigger("Death");
        }

        Destroy(gameObject, 1f);
    }
}
