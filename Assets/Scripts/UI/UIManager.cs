using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject characterUIPanel;

    [Header("Player References")]
    public PlayerStats playerStats;
    public PlayerInventory playerInventory;

    [Header("UI References")]
    public Image hpFillImage;
    public Image expFillImage;

    [Header("Inventory Slots")]
    public GameObject slot1Icon;
    public TMP_Text slot1AmountText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            characterUIPanel.SetActive(!characterUIPanel.activeSelf);
        }

        if (playerStats != null)
        {
            hpFillImage.fillAmount = (float)playerStats.currentHP / playerStats.maxHP;
            expFillImage.fillAmount = (float)playerStats.currentExp / playerStats.requiredExp;
        }

        if (playerInventory != null && characterUIPanel.activeSelf)
        {
            int amount = playerInventory.mobDrop;

            if (amount > 0)
            {
                slot1Icon.SetActive(true);
                slot1AmountText.gameObject.SetActive(true);
                slot1AmountText.text = amount.ToString();
            }
            else
            {
                slot1Icon.SetActive(false);
                slot1AmountText.gameObject.SetActive(false);
            }
        }
    }
}
