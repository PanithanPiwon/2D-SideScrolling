using UnityEngine;

public class PickupItem : MonoBehaviour
{
    [SerializeField] private Item item;           

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Inventory.Instance.AddItem(item);  
            Destroy(gameObject); 
        }
    }
}
