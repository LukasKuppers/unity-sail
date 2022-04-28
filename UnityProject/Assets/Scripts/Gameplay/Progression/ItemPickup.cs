using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [SerializeField]
    private GameObject shipPrefabManager;
    [SerializeField]
    private GameObject inventoryObject;
    [SerializeField]
    private float pickupRadius = 1f;

    private ShipPrefabManager shipManager;
    private PlayerInventory inventory;

    private GameObject playerShip;
    private Item item = Item.FOOD;
    private int itemAmount = 1;

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, pickupRadius);
    }

    private void Start()
    {
        shipManager = shipPrefabManager.GetComponent<ShipPrefabManager>();
        inventory = inventoryObject.GetComponent<PlayerInventory>();

        shipManager.AddSpawnListener(UpdatePlayerShip);
    }

    private void Update()
    {
        if (playerShip != null && 
            Vector3.Distance(transform.position, playerShip.transform.position) <= pickupRadius)
        {
            int pickupAmount = 0;
            switch (item)
            {
                case Item.FOOD:
                    pickupAmount = inventory.IncrementFood(itemAmount);
                    break;
                case Item.WOOD:
                    pickupAmount = inventory.IncrementWood(itemAmount);
                    break;
                case Item.CANNONBALL:
                    pickupAmount = inventory.IncrementCannonball(itemAmount);
                    break;
                case Item.TREASURE:
                    pickupAmount = inventory.IncrementTreasure(itemAmount);
                    break;
            }
            itemAmount -= pickupAmount;
            if (itemAmount <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    private void UpdatePlayerShip()
    {
        playerShip = shipManager.GetCurrentShip();
    }

    public void SetParameters(GameObject newShipPrefabManager, GameObject newInventoryObject,
                              Item itemType, int amount)
    {
        if (itemType == Item.COIN)
        {
            Debug.LogWarning("ItemPickup:SetParameters: itemType cannot be COIN - default FOOD is used");
            itemType = Item.FOOD;
        }
        shipPrefabManager = newShipPrefabManager;
        inventoryObject = newInventoryObject;

        shipManager = shipPrefabManager.GetComponent<ShipPrefabManager>();
        inventory = inventoryObject.GetComponent<PlayerInventory>();
        playerShip = shipManager.GetCurrentShip();
        item = itemType;
        itemAmount = amount;
    }
}
