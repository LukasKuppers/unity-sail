using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour, IDestructable
{
    [SerializeField]
    private GameObject displayPrefab;
    [SerializeField]
    private GameObject pickupDisplayPrefab;
    [SerializeField]
    private GameObject uiParent;
    [SerializeField]
    private GameObject shipPrefabManager;
    [SerializeField]
    private GameObject inventoryObject;
    [SerializeField]
    private float pickupRadius = 1f;
    [SerializeField]
    private float displayRadius = 2f;
    [SerializeField]
    private Item item = Item.FOOD;
    [SerializeField]
    private int itemAmount = 1;

    private ShipPrefabManager shipManager;
    private PlayerInventory inventory;

    private GameObject playerShip;
    private GameObject displayObject;
    private PickupDisplay display;

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, pickupRadius);
        Gizmos.DrawWireSphere(transform.position, displayRadius);
    }

    private void Start()
    {
        shipManager = shipPrefabManager.GetComponent<ShipPrefabManager>();
        inventory = inventoryObject.GetComponent<PlayerInventory>();

        shipManager.AddSpawnListener(UpdatePlayerShip);
    }

    private void Awake()
    {
        if (uiParent != null)
        {
            SetupDisplay();
        }
    }

    private void SetupDisplay()
    {
        displayObject = Instantiate(displayPrefab, uiParent.transform);
        display = displayObject.GetComponent<PickupDisplay>();

        display.SetInitParameters(item, itemAmount, gameObject);
        displayObject.SetActive(false);
    }

    private void Update()
    {
        if (playerShip != null)
        {
            float distToShip = Vector3.Distance(transform.position, playerShip.transform.position);
            if (distToShip <= displayRadius)
            {
                displayObject.SetActive(true);

                if (distToShip <= pickupRadius)
                {
                    SendItemToPlayer();
                }
            }
            else
            {
                displayObject.SetActive(false);
            }
        }
    }

    private void SendItemToPlayer()
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
        display.UpdateItemAmount(itemAmount);

        if (pickupAmount > 0)
        {
            GameObject pickupDisplay = Instantiate(pickupDisplayPrefab, 
                display.transform.position, Quaternion.identity, uiParent.transform);

            InventoryIncreaseDisplay disp = pickupDisplay.GetComponent<InventoryIncreaseDisplay>();
            disp.Init(item, pickupAmount);
        }

        if (itemAmount <= 0)
        {
            Destroy();
        }
    }

    private void UpdatePlayerShip()
    {
        playerShip = shipManager.GetCurrentShip();
        display.SetInitParameters(item, itemAmount, gameObject);
    }

    public void AddDestructionListener(UnityEngine.Events.UnityAction<GameObject> call)
    {
        throw new System.NotImplementedException("ItemPickup:AddDestructionListener: item pickup has no destruction event");
    }

    public void Destroy()
    {
        Destroy(displayObject);
        Destroy(gameObject);
    }

    public void SetParameters(GameObject newShipPrefabManager, GameObject newInventoryObject,
                              GameObject hudParent, Item itemType, int amount)
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
        uiParent = hudParent;
        playerShip = shipManager.GetCurrentShip();
        item = itemType;
        itemAmount = amount;

        SetupDisplay();
    }
}
