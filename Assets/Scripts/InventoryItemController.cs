using UnityEngine;

/// <summary>
/// This is a UI inventory item 
/// </summary>
public class InventoryItemController : MonoBehaviour
{
    /// <summary>
    /// The Item this UI inventory item represents
    /// </summary>
    Item item;

    /// <summary>
    /// Remove this Item from the inventory
    /// </summary>
    public void RemoveItem()
    {
        if(InventoryManager.Instance.Remove(item) == 0)
            Destroy(gameObject);
    }

    /// <summary>
    /// Set the item that this UI Inventory Item represents
    /// </summary>
    /// <param name="newItem">The item this UI Item is to represent</param>
    public void AddItem(Item newItem)
    {
        item = newItem;
    }

    /// <summary>
    /// Use this Item
    /// </summary>
    public void UseItem()
    {
        // Decide what to do based on the type of item we're using
        switch (item.itemType)
        {
            case Item.ItemType.RingOfHealth:
                Player.Instance.IncreaseHealth(item.value);
                break;
            case Item.ItemType.BookOfXP:
                Player.Instance.IncreaseExp(item.value);
                break;
        }

        // Remove the item or decrease it's count depending on how many we have
        RemoveItem();
    }
}