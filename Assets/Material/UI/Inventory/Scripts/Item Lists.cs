using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Health,
    Mana,
    Equipment,
    Quest,
    Default
}
[CreateAssetMenu(fileName = "ListItem", menuName = "Item")]
public class ItemLists : ScriptableObject
{
    public string itemCode;
    public string itemName;
    public Sprite itemIcon;
    public ItemType itemType;
    public string itemDescription;
    public int itemLevel;
    public int itemValue;
    public int itemPrice;
    public int itemAmount;
}
