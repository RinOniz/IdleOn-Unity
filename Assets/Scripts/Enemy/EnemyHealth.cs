using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHp = 10;

    private int currentHp;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        currentHp = maxHp;
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
        Destroy(gameObject);
    }
}
