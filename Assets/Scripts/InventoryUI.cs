using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private Transform itemsParent;
    [SerializeField] private ItemDisplay inventorySlotPrefab;

    [SerializeField] private Button sortResourcesButton;
    [SerializeField] private Button sortToolsButton;
    [SerializeField] private Button sortCraftedObjectsButton;
    [SerializeField] private Button sortSeedsButton;
    [SerializeField] private Button showAllButton;

    private List<Item> originalItemList;
    private List<ItemDisplay> slotList = new List<ItemDisplay>();
    void Start()
    {
        sortResourcesButton.onClick.AddListener(() => SortItemsByType(ItemType.Resources));
        sortToolsButton.onClick.AddListener(() => SortItemsByType(ItemType.Tools));
        sortCraftedObjectsButton.onClick.AddListener(() => SortItemsByType(ItemType.CraftedObjects));
        sortSeedsButton.onClick.AddListener(() => SortItemsByType(ItemType.Seeds));
        showAllButton.onClick.AddListener(ShowAllItems);
    }

    public void UpdateUI()
    {
        var newItem = Inventory.Instance.items[Inventory.Instance.items.Count - 1];

        if (slotList.Count == 0)
        {
            var newSlot = Instantiate(inventorySlotPrefab, itemsParent);
            SetItemInSlot(newSlot, newItem);
            slotList.Add(newSlot);
        }
        else
        {
            var itemFound = false;

            for (int i = 0; i < slotList.Count; i++)
            {
                var currentSlot = slotList[i].GetComponent<ItemDisplay>();

                if (newItem.itemName == currentSlot.itemName && newItem.isStackable && currentSlot.stackCount < newItem.maxStackSize)
                {
                    currentSlot.stackCount++;
                    currentSlot.stackText.text = currentSlot.stackCount.ToString();
                    itemFound = true;
                    break;
                }
            }

            if (!itemFound)
            {
                var newSlot = Instantiate(inventorySlotPrefab, itemsParent);
                SetItemInSlot(newSlot, newItem);
                slotList.Add(newSlot);
            }
        }
    }

    private void SetItemInSlot(ItemDisplay slot, Item item)
    {
        slot.itemRef = item;
        slot.SetData();
    }

    public void ShowAllItems()
    {
        for (int i = 0; i < slotList.Count; i++)
        {
            Destroy(slotList[i]);
        }

        slotList.Clear();
        ClearSlot();

        if (Inventory.Instance.items.Count == 0)
        {
            return;
        }

        for (int i = 0; i < Inventory.Instance.items.Count; i++)
        {
            var item = Inventory.Instance.items[i];

            bool itemFoundInSlot = false;

            for (int j = 0; j < slotList.Count; j++)
            {
                var existingSlot = slotList[j];
                var itemDisplay = existingSlot.GetComponentInChildren<ItemDisplay>();

                if (itemDisplay != null && itemDisplay.itemName == item.itemName && itemDisplay.stackCount < item.maxStackSize)
                {
                    itemDisplay.stackCount++;
                    itemDisplay.stackText.text = itemDisplay.stackCount.ToString();
                    itemFoundInSlot = true;
                    break;
                }
            }

            if (!itemFoundInSlot)
            {
                var newSlot = Instantiate(inventorySlotPrefab, itemsParent);
                SetItemInSlot(newSlot, item);
                slotList.Add(newSlot);
            }
        }
    }

    private void ClearSlot()
    {
        if (itemsParent.transform.childCount == 0)
            return;

        for (int i = 0; i < itemsParent.transform.childCount; i++)
        {
            Destroy(itemsParent.transform.GetChild(i).gameObject);
        }
    }

    private void SortItemsByType(ItemType itemType)
    {
        for (int i = 0; i < slotList.Count; i++)
        {
            Destroy(slotList[i]);
        }

        slotList.Clear();
        ClearSlot();

        if (Inventory.Instance.items.Count == 0)
        {
            return;
        }

        for (int i = 0; i < Inventory.Instance.items.Count; i++)
        {
            var item = Inventory.Instance.items[i];

            if (item.itemType != itemType)
                continue;

            bool itemFoundInSlot = false;

            for (int j = 0; j < slotList.Count; j++)
            {
                var existingSlot = slotList[j];
                var itemDisplay = existingSlot.GetComponentInChildren<ItemDisplay>();

                if (itemDisplay != null && itemDisplay.itemName == item.itemName && itemDisplay.stackCount < item.maxStackSize)
                {
                    itemDisplay.stackCount++;
                    itemDisplay.stackText.text = itemDisplay.stackCount.ToString();
                    itemFoundInSlot = true;
                    break;
                }
            }

            if (!itemFoundInSlot)
            {
                var newSlot = Instantiate(inventorySlotPrefab, itemsParent);
                SetItemInSlot(newSlot, item);
                slotList.Add(newSlot);
            }
        }
    }
    public string GenerateUniqueID()
    {
        return Guid.NewGuid().ToString();
    }
}
