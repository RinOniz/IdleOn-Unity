using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public Image fillImage;
    private EnemyHealth enemyHealth; 
    private EnemyStats enemyStats;

    private void Start()
    {
        enemyHealth = GetComponentInParent<EnemyHealth>();
        enemyStats = GetComponentInParent<EnemyStats>();
    }

    private void Update()
    {
        if (enemyHealth != null)
        {
            fillImage.fillAmount = (float)enemyHealth.currentHp / enemyStats.maxHP;
        }
    }
}
