using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandTreasurePickup : MonoBehaviour
{
    private static readonly string ATTACK_LOCK_KEY = "Island_treasure_pickup_key";

    [SerializeField]
    private GameObject inventoryObject;

    private PlayerInventory inventory;
    private bool mouseIsFocused = false;

    private void Start()
    {
        if (inventoryObject != null)
        {
            inventory = inventoryObject.GetComponent<PlayerInventory>();
        }
        PlayerInputManager inputManager = InputReference.GetInputManager();
        inputManager.AddInputListener(InputEvent.MOUSE_LEFT, PickupTreasure);
    }

    public void SetInventory(GameObject inventoryManager)
    {
        inventoryObject = inventoryManager;
        inventory = inventoryObject.GetComponent<PlayerInventory>();
    }

    private void PickupTreasure()
    {
        if (mouseIsFocused && PlayerSceneInteraction.InteractionEnabled())
        {
            int acutalInc = inventory.IncrementTreasure(1);
            if (acutalInc == 1)
            {
                PlayerAttackMode.EnableAttack(ATTACK_LOCK_KEY);
                Destroy(gameObject);
            }
        }
    }

    private void OnMouseEnter()
    {
        mouseIsFocused = true;
        PlayerAttackMode.DisableAttack(ATTACK_LOCK_KEY);
    }

    private void OnMouseExit()
    {
        mouseIsFocused = false;
        PlayerAttackMode.EnableAttack(ATTACK_LOCK_KEY);
    }
}
