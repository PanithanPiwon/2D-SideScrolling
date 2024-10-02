using UnityEngine;

public enum ItemType
{
    Resources,
    Tools,
    CraftedObjects,
    Seeds
}

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item", order = 1)]
public class Item : ScriptableObject
{
    public string itemID;
    public string itemName;      
    public Sprite icon;        
    public bool isStackable;      
    public bool equippable;
    public bool useable;
    public int maxStackSize;       
    public ItemType itemType;     
}
