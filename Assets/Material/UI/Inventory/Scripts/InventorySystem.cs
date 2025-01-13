using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{
    public List<ItemLists> itemLists = new();

    public static InventorySystem Instance { get; private set; }

    public Transform itemHolder;
    public GameObject itemPrefab;

    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void AddItem(ItemLists item)
    {
        item.itemAmount++;
        if (!itemLists.Contains(item))
        {
            itemLists.Add(item);
            DisplayInventory(item);
        }
        else
        {
            UpdateItemAmount(item);
        }
    }

    public void RemoveItem(ItemLists item)
    {
        if (itemLists.Contains(item))
        {
            itemLists.Remove(item);
        }
    }

    public void DisplayInventory(ItemLists item)
    {
        GameObject itemCreated = Instantiate(itemPrefab, itemHolder);

        var itemName = itemCreated.transform.Find("Name").GetComponent<TextMeshProUGUI>();
        var itemIcon = itemCreated.transform.Find("Icon").GetComponent<Image>();
        var itemAmounts = itemCreated.transform.Find("Amount").GetComponent<TextMeshProUGUI>();

        itemName.text = item.itemName;
        itemIcon.sprite = item.itemIcon;
        itemAmounts.text = item.itemAmount.ToString();

        itemCreated.GetComponent<IdentifyItem>().Identify(item);
    }
    public void UpdateItemAmount(ItemLists item)
    {
        foreach (Transform child in itemHolder)
        {
            var identifyItem = child.GetComponent<IdentifyItem>();
            if (identifyItem != null && identifyItem.item == item)
            {
                var itemAmounts = child.Find("Amount").GetComponent<TextMeshProUGUI>();
                itemAmounts.text = item.itemAmount.ToString();
                break;
            }
        }
    }

}
