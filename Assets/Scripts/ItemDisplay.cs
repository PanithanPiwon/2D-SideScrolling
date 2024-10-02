using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private bool isWearing;
    [SerializeField] private string itemID;
    [SerializeField] private Image icon;
    [SerializeField] private ItemType type;

    public string itemName;
    public int stackCount;
    public Text stackText;
    public Item itemRef;
    public GameObject wearingPanel;

    [SerializeField] private GameObject windownPopup;
    [SerializeField] private Button equipButton;
    [SerializeField] private Button removeEquipButton;
    [SerializeField] private Button useButton;
    [SerializeField] private Button removeButton;

    private void Start()
    {
        equipButton.onClick.AddListener(EquipItem);
        removeEquipButton.onClick.AddListener(RemoveEquipitem);
        //useButton.onClick.AddListener();
        removeButton.onClick.AddListener(RemoveItem);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isWearing)
        {
            windownPopup.SetActive(true);
        }
        else
        {
            wearingPanel.SetActive(true);
            removeEquipButton.gameObject.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isWearing)
        {
            windownPopup.SetActive(false);
        }
        else
        {
            windownPopup.SetActive(false);
            wearingPanel.SetActive(false);
            removeEquipButton.gameObject.SetActive(false);
        }
    }

    private void EquipItem()
    {
        isWearing = true;
        windownPopup.SetActive(false);
        wearingPanel.SetActive(true);
        PlayerController.Instance.SetSpriteEquipment(icon.sprite, gameObject.GetComponent<ItemDisplay>());
    }

    public void RemoveEquipitem()
    {
        isWearing = false;
        windownPopup.SetActive(true);
        wearingPanel.SetActive(false);
        PlayerController.Instance.RemoveEquipItem();
    }

    private void RemoveItem()
    {
        if (stackCount > 1)
        {
            stackCount--;
            stackText.text = stackCount.ToString();
        }
        else
        {
            Inventory.Instance.RemoveItem(new Item { itemName = itemName, itemID = itemID });
            Destroy(gameObject);
        }
    }
    public void SetData()
    {
        itemID = itemRef.itemID;
        icon.sprite = itemRef.icon;
        type = itemRef.itemType;
        itemName = itemRef.itemName;
        stackCount = 1;
        stackText.text = stackCount.ToString();
        stackText.gameObject.SetActive(itemRef.isStackable);

        equipButton.gameObject.SetActive(itemRef.equippable);
        useButton.gameObject.SetActive(itemRef.useable);
    }
}
