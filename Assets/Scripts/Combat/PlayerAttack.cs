using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float attackRange = 2.0f;
    [SerializeField] private float attackCooldown = 1.0f;

    private EnemyHealth currentTarget;

    private float attackTimer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        currentTarget = FindNearestEnemy();

        AutoAttack();
    }

    private void Attack()
    {
        currentTarget.TakeDamage(5);

        Debug.Log("Attacked " + currentTarget.name);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    private EnemyHealth FindNearestEnemy()
    {
        EnemyHealth[] enemies = FindObjectsByType<EnemyHealth>(FindObjectsSortMode.None);

        EnemyHealth nearestEnemy = null;

        float nearestDistance = Mathf.Infinity;

        foreach (EnemyHealth enemy in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);

            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestEnemy = enemy;
            }
        }

        return nearestEnemy;
    }

    private void AutoAttack()
    {
        attackTimer += Time.deltaTime;

        if (currentTarget != null)
        {
            float distance = Vector2.Distance(transform.position, currentTarget.transform.position);

            if (distance <= attackRange && attackTimer >= attackCooldown)
            {
                Attack();

                attackTimer = 0f;
            }
        }

        //// Manual Attack 
        //if (Input.GetKeyDown(KeyCode.LeftAlt))
        //{
        //    if (currentTarget != null)
        //    {
        //        float distance = Vector2.Distance(transform.position, currentTarget.transform.position);

        //        if (distance <= attackRange && attackTimer >= attackCooldown)
        //        {
        //            Attack();

        //            attackTimer = 0f;
        //        }
        //    }
        //}
    }
}
