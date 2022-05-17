using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTreasurePickup : MonoBehaviour, IClickableObject
{
    [SerializeField]
    private GameObject inventoryObject;
    [SerializeField]
    private GameObject mapTreasureManager;
    [SerializeField]
    private Islands island;
    [SerializeField]
    private int treasureAmount = 1;


    private PlayerInventory inventory;
    private MapTreasureManager treasureManger;

    private void Start()
    {
        if (inventoryObject != null)
            inventory = inventoryObject.GetComponent<PlayerInventory>();

        if (mapTreasureManager != null)
            treasureManger = mapTreasureManager.GetComponent<MapTreasureManager>();
    }

    public void Interact(string interactionLockKey)
    {
        if (PlayerSceneInteraction.InteractionEnabled())
        {
            int acutalInc = inventory.IncrementTreasure(treasureAmount);
            if (acutalInc > 0)
            {
                treasureManger.RegisterRemovedTreasure(island);

                PlayerAttackMode.EnableAttack(interactionLockKey);
                Destroy(gameObject);
            }
        }
    }

    public void InitParameters(GameObject inventoryManager, GameObject mapTreasureManager, Islands island)
    {
        inventoryObject = inventoryManager;
        inventory = inventoryObject.GetComponent<PlayerInventory>();

        this.mapTreasureManager = mapTreasureManager;
        treasureManger = mapTreasureManager.GetComponent<MapTreasureManager>();

        this.island = island;
    }
}
