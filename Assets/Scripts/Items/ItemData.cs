using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "IdleOn/Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite itemIcon;
    public int maxStack = 99;
    public bool isEquippable;
}
