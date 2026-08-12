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

    [Header("Drop Settings")]
    public GameObject lootPrefab;

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

    public void SwapSlots(int index1, int index2)
    {
        InventorySlot slot1 = slots[index1]; // Ô đang cầm trên tay (chuột)
        InventorySlot slot2 = slots[index2]; // Ô bị thả vào

        // KIỂM TRA: Nếu 2 ô đang chứa CÙNG MỘT LOẠI đồ vật
        if (slot1.itemData != null && slot2.itemData != null && slot1.itemData == slot2.itemData)
        {
            int maxStack = slot1.itemData.maxStack;
            int totalAmount = slot1.amount + slot2.amount;

            // Trường hợp 1: Cộng lại vẫn chưa vượt qua mức tối đa (ví dụ 1 + 1 = 2)
            if (totalAmount <= maxStack)
            {
                slot2.amount = totalAmount; // Ô bị thả vào nhận toàn bộ số lượng

                // Dọn sạch ô cũ trên tay
                slot1.itemData = null;
                slot1.amount = 0;
            }
            // Trường hợp 2: Cộng lại bị tràn max stack (ví dụ 80 + 30 = 110, max là 99)
            else
            {
                slot2.amount = maxStack; // Ô bị thả vào chứa đầy 99
                slot1.amount = totalAmount - maxStack; // Ô trên tay giữ lại phần thừa là 11
            }
        }
        else
        {
            // NẾU LÀ 2 ĐỒ VẬT KHÁC NHAU HOẶC THẢ VÀO Ô TRỐNG -> ĐỔI CHỖ BÌNH THƯỜNG
            InventorySlot temp = slots[index1];
            slots[index1] = slots[index2];
            slots[index2] = temp;
        }
    }

    public void DropItem(int index)
    {
        InventorySlot slot = slots[index];

        if (slot.itemData != null)
        {
            float dropDirX = Random.value > 0.5f ? 1f : -1f;
            float dropDirY = Random.Range(0f, 0.5f);

            Vector3 dropPosition = transform.position + new Vector3(dropDirX, dropDirY, 0);

            GameObject dropped = Instantiate(lootPrefab, dropPosition, Quaternion.identity);

            LootItem lootCode = dropped.GetComponent<LootItem>();
            lootCode.itemData = slot.itemData;
            lootCode.amount = slot.amount;

            slot.itemData = null;
            slot.amount = 0;
        }
    }
}
