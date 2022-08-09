using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandTreasurePickup : MonoBehaviour, IClickableObject
{
    private static readonly string HUD_TAG = "Canvas";

    [SerializeField]
    private GameObject pickupDisplayPrefab;
    [SerializeField]
    private GameObject inventoryObject;
    [SerializeField]
    private int treasureAmount = 1;

    private PlayerInventory inventory;

    private void Start()
    {
        if (inventoryObject != null)
        {
            inventory = inventoryObject.GetComponent<PlayerInventory>();
        }
    }

    public void Interact(string interactionLockKey)
    {
        if (PlayerSceneInteraction.InteractionEnabled())
        {
            int actualInc = inventory.IncrementTreasure(treasureAmount);
            if (actualInc > 0)
            {
                SpawnPickupDisplay(actualInc);

                PlayerAttackMode.EnableAttack(interactionLockKey);
                Destroy(gameObject);
            }
        }
    }

    private void SpawnPickupDisplay(int amount)
    {
        Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
        GameObject dispObj = Instantiate(pickupDisplayPrefab, pos, Quaternion.identity,
            GameObject.FindGameObjectWithTag(HUD_TAG).transform);

        InventoryIncreaseDisplay disp = dispObj.GetComponent<InventoryIncreaseDisplay>();
        disp.Init(Item.TREASURE, amount);
    }

    public void SetInventory(GameObject inventoryManager)
    {
        inventoryObject = inventoryManager;
        inventory = inventoryObject.GetComponent<PlayerInventory>();
    }
}
