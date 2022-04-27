using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryAmountText : MonoBehaviour
{
    [SerializeField]
    private GameObject inventoryObject;
    [SerializeField]
    Item item;

    private PlayerInventory inventory;
    private TextMeshProUGUI amountText;

    private void Start()
    {
        inventory = inventoryObject.GetComponent<PlayerInventory>();
        amountText = gameObject.GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        int amount = 0;
        switch (item)
        {
            case Item.FOOD:
                amount = inventory.GetFoodAmount();
                break;
            case Item.WOOD:
                amount = inventory.GetWoodAmount();
                break;
            case Item.CANNONBALL:
                amount = inventory.GetCannonballAmount();
                break;
            case Item.TREASURE:
                amount = inventory.GetTreasureAmount();
                break;
            case Item.COIN:
                amount = inventory.GetCoinAmount();
                break;
        }
        amountText.SetText(amount.ToString());
    }
}

public enum Item
{
    FOOD, 
    WOOD, 
    CANNONBALL, 
    TREASURE, 
    COIN
}
