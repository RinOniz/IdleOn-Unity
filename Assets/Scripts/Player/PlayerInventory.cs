using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public int mobDrop = 0;

    public void AddMobDrop(int amount)
    {
        mobDrop += amount;

        Debug.Log($"Mob drop: {mobDrop}");
    }
}
