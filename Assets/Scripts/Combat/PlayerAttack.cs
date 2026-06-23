using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float attackRange = 2.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
        }
    }

    private void Attack()
    {
        EnemyHealth enemy = FindFirstObjectByType<EnemyHealth>();

        if (enemy == null)
        {
            return;
        }

        float distance = Vector2.Distance(transform.position, enemy.transform.position);

        if (distance <= attackRange)
        {
            enemy.TakeDamage(5);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
