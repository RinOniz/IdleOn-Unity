using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventorySlot
{
    public ItemData itemData;
    public int amount;
}

public class PlayerInventory : MonoBehaviour
{
    [Header("Inventory Settings")]
    public int maxSlots = 16;

    public List<InventorySlot> slots = new List<InventorySlot>();

    private void Start()
    {
        for (int i = 0; i < maxSlots; i++)
        {
            slots.Add(new InventorySlot());
        }
    }

    private void Update()
    {

    }

    public bool AddItem(ItemData itemToAdd, int amountToAdd)
    {   
        foreach (InventorySlot slot in slots)
        {
            if (slot.itemData == itemToAdd && slot.amount < itemToAdd.maxStack)
            {
                slot.amount += amountToAdd;

                return true;
            }
        }

        foreach (InventorySlot slot in slots)
        {
            if (slot.itemData == null)
            {
                slot.itemData = itemToAdd;
                slot.amount = amountToAdd;

                return true;
            }
        }

        Debug.Log("Full Inventory");

        return false;
    }
}
