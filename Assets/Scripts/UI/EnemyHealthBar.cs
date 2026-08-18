using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    [SerializeField] private GameObject healthBarVisual;

    private EnemyHealth enemyHealth; 
    private EnemyStats enemyStats;

    private void Start()
    {
        enemyHealth = GetComponentInParent<EnemyHealth>();
        enemyStats = GetComponentInParent<EnemyStats>();

        // Enemy mới spawn có full HP → ẩn thanh máu
        healthBarVisual.SetActive(false);
    }

    private void Update()
    {
        if (enemyHealth == null || enemyStats == null)
            return;

        float hpPercent = (float)enemyHealth.currentHp / enemyStats.maxHP;

        fillImage.fillAmount = hpPercent;

        // Mất máu → hiện
        if (enemyHealth.currentHp < enemyStats.maxHP)
        {
            if (!healthBarVisual.activeSelf)
                healthBarVisual.SetActive(true);
        }
        // Full HP → ẩn
        else
        {
            if (healthBarVisual.activeSelf)
                healthBarVisual.SetActive(false);
        }
    }
}
