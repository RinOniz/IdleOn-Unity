using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

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

    [Header("Inventory UI")]
    public Transform inventoryGrid;

    [Header("Auto Mode")]
    public bool isAutoModeOn = false;

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
            for (int i = 0; i < playerInventory.slots.Count; i++)
            {
                InventorySlot slotData = playerInventory.slots[i];

                if (i < inventoryGrid.childCount)
                {
                    Transform uiSlot = inventoryGrid.GetChild(i);
                    Transform iconTransform = uiSlot.Find("Item_Icon");
                    Transform txtTransform = uiSlot.Find("TXT_Amount");

                    if (iconTransform != null && txtTransform != null)
                    {
                        GameObject iconObj = iconTransform.gameObject;
                        TMP_Text amountText = txtTransform.GetComponent<TMP_Text>();

                        if (slotData.itemData != null && slotData.amount > 0)
                        {
                            iconObj.SetActive(true);
                            iconObj.GetComponent<Image>().sprite = slotData.itemData.itemIcon;
                            amountText.gameObject.SetActive(true);
                            amountText.text = slotData.amount.ToString();
                        }
                        else
                        {
                            iconObj.SetActive(false);
                            amountText.gameObject.SetActive(false);
                        }
                    }
                }
            }
            
        }
    }

    public void ToggleInventoryButton()
    {
        if (characterUIPanel != null)
        {
            characterUIPanel.SetActive(!characterUIPanel.activeSelf);
        }
    }

    public void ToggleAutoModeButton()
    {
        isAutoModeOn = !isAutoModeOn;

        if (playerStats != null)
        {
            PlayerMovement playerMovement = playerStats.GetComponent<PlayerMovement>();

            if (playerMovement != null)
            {
                playerMovement.isAuto = isAutoModeOn;
            }
        }

        Debug.Log($"Auto Mode is now {(isAutoModeOn ? "ON" : "OFF")}");
    }
}
