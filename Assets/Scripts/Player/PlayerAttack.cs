using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack Stats")]
    public float attackRange = 2.0f;
    public float attackSpeed = 1.0f;

    public EnemyHealth currentTarget;
    public GameObject damagePopupPrefab;

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
        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }
    }

    public void DealDamageHit()
    {
        if (currentTarget == null)
        {
            return;
        }

        EnemyStats enemyStats = currentTarget.GetComponent<EnemyStats>();

        int damage = playerStats.attack - enemyStats.defense;

        damage = Mathf.Max(1, damage);

        currentTarget.TakeDamage(damage);

        Debug.Log($"Attack {currentTarget.name} for {damage} damage");

        if (damagePopupPrefab != null)
        {
            Vector3 randomOffset = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(0.5f, 1f), 0);
            GameObject popup = Instantiate(damagePopupPrefab, currentTarget.transform.position + randomOffset, Quaternion.identity);
            
            popup.GetComponent<DamagePopup>().Setup(damage);
        }
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
