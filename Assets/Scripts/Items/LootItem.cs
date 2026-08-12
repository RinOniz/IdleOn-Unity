using UnityEngine;

public class LootItem : MonoBehaviour
{
    public ItemData itemData;
    public int amount = 1;

    private PlayerInventory playerInventory;

    private void Start()
    {
        if (itemData != null)
        {
            GetComponent<SpriteRenderer>().sprite = itemData.itemIcon;
        }

        playerInventory = FindFirstObjectByType<PlayerInventory>();
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(0))
        {
            if (playerInventory != null)
            {
                bool pickedUp = playerInventory.AddItem(itemData, amount);

                if (pickedUp)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
