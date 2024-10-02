using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance { get; private set; } 
    public List<Item> items = new List<Item>();
    public InventoryUI inventoryUI;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void AddItem(Item newItem)
    {
        items.Add(newItem);
        inventoryUI.UpdateUI();
    }

    public void RemoveItem(Item itemToRemove)
    {
        if (items.Contains(itemToRemove))
        {
            items.Remove(itemToRemove);
        }
    }
}
