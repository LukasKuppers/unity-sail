using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandTreasurePickup : MonoBehaviour, IClickableObject
{
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
            int acutalInc = inventory.IncrementTreasure(treasureAmount);
            if (acutalInc > 0)
            {
                PlayerAttackMode.EnableAttack(interactionLockKey);
                Destroy(gameObject);
            }
        }
    }

    public void SetInventory(GameObject inventoryManager)
    {
        inventoryObject = inventoryManager;
        inventory = inventoryObject.GetComponent<PlayerInventory>();
    }
}
