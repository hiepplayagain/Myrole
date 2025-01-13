using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdentifyItem : MonoBehaviour
{
    public ItemLists item;

    public void Identify(ItemLists item)
    {
        this.item = item;
    }
    public void Use()
    {
        switch (item.itemType)
        { 

            case ItemType.Health:
                if (FindAnyObjectByType<ManageCharacters>().currentHealth < FindAnyObjectByType<ManageCharacters>().maxHealth)
                {
                    FindAnyObjectByType<ManageCharacters>().currentHealth += item.itemValue;
                    
                }
                break;
            case ItemType.Mana:
                if (FindAnyObjectByType<ManageCharacters>().currentMana < FindAnyObjectByType<ManageCharacters>().maxMana)
                {
                    FindAnyObjectByType<ManageCharacters>().currentMana += item.itemValue;
                }
                break;

        }    

    }
}
