using UnityEngine;

public class LootItem : MonoBehaviour
{
    public string itemName = "Spore Cap";
    public int amount = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerInventory inventory = collision.GetComponent<PlayerInventory>();

        if (inventory != null)
        {
            inventory.AddMobDrop(amount);

            Destroy(gameObject);
        }
    }
}
