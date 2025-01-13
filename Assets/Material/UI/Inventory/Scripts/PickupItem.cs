using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PickupItem : MonoBehaviour
{
    public TMP_Text amountItems;

    public ItemLists item;
    public bool canPickUp = false;

    public Transform itemArrow;
    public GameObject itemArrowPrefab;

    public Sprite[] pressedButton;

    private void Update()
    {
        if (canPickUp && Input.GetKeyDown(KeyCode.F))
        {
            itemArrow.GetChild(0).GetComponent<SpriteRenderer>().sprite = pressedButton[1];
            AddItem();
        }
        

    }

    void AddItem()
    {
        InventorySystem.Instance.AddItem(item);
        
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            canPickUp = true;

            Vector3 offset = new(0, 0.2f, 0);

            GameObject itemArrowGuide = Instantiate(itemArrowPrefab, itemArrow);

            itemArrowGuide.transform.position = transform.position + offset;

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            canPickUp = false;
            Destroy(itemArrow.GetChild(0).gameObject);
        }
    }
}
