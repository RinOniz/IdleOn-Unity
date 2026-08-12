using UnityEngine;

public class LootItem : MonoBehaviour
{
    public ItemData itemData;
    public int amount = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerInventory inventory = collision.GetComponent<PlayerInventory>();

        if (inventory != null)
        {
            bool pickedUp = inventory.AddItem(itemData, amount);

            if (pickedUp)
            {
                Destroy(gameObject);
            }
        }
    }
}
