using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack Stats")]
    public float attackRange = 2.0f;
    public float attackSpeed = 1.0f;

    public EnemyHealth currentTarget;

    private PlayerStats playerStats;
    private PlayerMovement playerMovement;
    private Animator animator;

    private float attackTimer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        playerStats = GetComponent<PlayerStats>();
        playerMovement = GetComponent<PlayerMovement>();
        animator = GetComponentInChildren<Animator>();

        attackTimer = 100f;
    }

    // Update is called once per frame
    private void Update()
    {
        currentTarget = FindNearestEnemy();

        attackTimer += Time.deltaTime;

        if (playerMovement != null && playerMovement.isAuto)
        {
            AutoAttack();
        }
        else
        {
            ManualAttack();
        }
    }

    private void Attack()
    {
        EnemyStats enemyStats = currentTarget.GetComponent<EnemyStats>();

        int damage = playerStats.attack - enemyStats.defense;

        damage = Mathf.Max(1, damage);

        currentTarget.TakeDamage(damage);

        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }

        Debug.Log($"Attack {currentTarget.name} for {damage} damage");
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

            float cooldownRequired = 1.0f / attackSpeed;

            if (distance <= attackRange && attackTimer >= cooldownRequired)
            {
                Attack();

                attackTimer = 0f;
            }
        }
    }

    private void ManualAttack()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            if (currentTarget != null)
            {
                float distance = Vector2.Distance(transform.position, currentTarget.transform.position);

                float cooldownRequired = 1.0f / attackSpeed;

                if (distance <= attackRange && attackTimer >= cooldownRequired)
                {
                    Attack();

                    attackTimer = 0f;
                }
            }
        }
    }
}
