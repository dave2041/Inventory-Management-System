using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    /// <summary>
    /// A list of all items in the game
    /// </summary>
    [SerializeField] List<Item> ItemStore = new List<Item>();

    /// <summary>
    /// Items belonging to the player. Using item name as dictionary key
    /// </summary>
    [SerializeField] Dictionary<string, Item> Items = new Dictionary<string, Item>();

    /// <summary>
    /// This is the Inventory UI transform element under which all items will be instantiated
    /// </summary>
    [SerializeField] Transform ItemContent;

    /// <summary>
    /// A blank inventory item prefab
    /// </summary>
    [SerializeField] GameObject InventoryItemPrefab;

    /// <summary>
    /// Inventory UI gameobject used to toggle opening and closing the inventory UI
    /// </summary>
    [SerializeField] GameObject Inventory;

    /// <summary>
    /// A list of the items currently being displayed in the Inventory UI
    /// </summary>
    [SerializeField] InventoryItemController[] InventoryItems;

    private void Awake()
    {
        Instance = this;

        // Reset the quantity here otherwise it will persist as we change the scriptable items during play
        foreach (var item in ItemStore)
        {
            item.quantity = 0;
        }
    }

    /// <summary>
    /// Add an item to the Inventory
    /// </summary>
    /// <param name="item">Item to add to the inventory</param>
    public void Add(Item item)
    {
        // If the item exists in the inventory then increase it's quantity, otherwise add it
        if (Items.ContainsKey(item.name))
        {
            Items[item.name].quantity++;
            print($"Item exists {item.name} Item quantity is {Items[item.name].quantity}");
        }
        else
        {
            Items.Add(item.name, item);
            Items[item.name].quantity = 1;
        }

        // Refresh the inventory contents
        StartCoroutine(ListItems());
    }

    /// <summary>
    /// Remove an item from the inventory. Will decrease the item quantity by 1 if it exists
    /// </summary>
    /// <param name="item">Item to remove from the inventory</param>
    /// <returns>Returns the current count of the item removed</returns>
    public int Remove(Item item)
    {
        // Is the item in the inventory?
        if (Items.ContainsKey(item.name))
        {
            // If the quantity of the item is more than one then we can decrease the quantity of the item by 1
            if (Items[item.name].quantity > 1)
            {
                print($"Item {item.name} Quantity = {Items[item.name].quantity}");
                StartCoroutine(ListItems());
                return Items[item.name].quantity -= 1;
            }
            else
            {
                print($"Deleting {item.name} from inventory");
                Items.Remove(item.name);
                StartCoroutine(ListItems());
            }
        }
        return 0;
    }

    /// <summary>
    /// List the items in the Inventory's UI
    /// </summary>
    /// <returns></returns>
    public IEnumerator ListItems()
    {
        CleanInventory();

        // Wait for a frame so that the destroy calls from "CleanInventory()" have a chance to execute
        yield return 0;

        // Iterate through the items in the Items dictionary
        foreach (KeyValuePair<string, Item> item in Items)
        {
            // instantiate the item in the UI and setup the text and image objects
            GameObject obj = Instantiate(InventoryItemPrefab, ItemContent);
            var itemName = obj.transform.Find("ItemName").GetComponent<TextMeshProUGUI>();
            var itemIcon = obj.transform.Find("ItemIcon").GetComponent<Image>();
            var ItemCount = itemIcon.transform.Find("ItemCount").GetComponent<TextMeshProUGUI>();
            itemName.text = item.Value.itemName;
            itemIcon.sprite = item.Value.icon;
            ItemCount.text = item.Value.quantity > 1 ? $"x{ item.Value.quantity}" : "";
        }

        // Add the InventoryItems instantiated above to the InventoryItems array
        SetInventoryItems();
    }


    /// <summary>
    /// Open or close the Inventory on screen depending on it's current state
    /// </summary>
    public void ToggleInventory()
    {
        if (!Inventory.activeInHierarchy)
        {
            StartCoroutine(ListItems());
            Inventory.SetActive(true);
        }
        else
        {
            Inventory.SetActive(false);
        }
    }


    /// <summary>
    /// Destorys the current UI contents of the Inventory
    /// </summary>
    public void CleanInventory()
    {
        foreach (Transform item in ItemContent)
        {
            Destroy(item.gameObject);
        }
    }

    /// <summary>
    /// Adds each Inventory Item to the InvetoryItems array
    /// </summary>
    public void SetInventoryItems()
    {
        InventoryItems = ItemContent.GetComponentsInChildren<InventoryItemController>();
        int i = 0;
        foreach (KeyValuePair<string, Item> kvp in Items)
        {
            InventoryItems[i++].AddItem(kvp.Value);
        }
    }
}