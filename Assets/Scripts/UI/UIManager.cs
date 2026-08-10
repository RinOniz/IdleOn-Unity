using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Player References")]
    public PlayerStats playerStats;

    [Header("UI References")]
    public Image hpFillImage;
    public Image expFillImage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        if (playerStats != null)
        {
            hpFillImage.fillAmount = (float)playerStats.currentHP / playerStats.maxHP;
            expFillImage.fillAmount = (float)playerStats.currentExp / playerStats.requiredExp;
        }
    }
}
