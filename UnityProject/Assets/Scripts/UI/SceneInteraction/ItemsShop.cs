using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsShop : MonoBehaviour
{
    [SerializeField]
    private GameObject inventoryObject;
    [SerializeField]
    private GameObject uIParent;
    [SerializeField]
    private GameObject safeZoneObject;
    [SerializeField]
    private GameObject itemShopModal;
    [SerializeField]
    private int foodPrice = 1;
    [SerializeField]
    private int woodPrice = 1;
    [SerializeField]
    private int cannonballPrice = 1;

    private SafeZone safeZone;
    private bool mouseIsFocused = false;

    private void Start()
    {
        safeZone = safeZoneObject.GetComponent<SafeZone>();
        PlayerInputManager inputManager = InputReference.GetInputManager();
        inputManager.AddInputListener(InputEvent.MOUSE_LEFT, OpenModal);
    }

    private void OpenModal()
    {
        if (safeZone.ShipInZone() && mouseIsFocused && PlayerSceneInteraction.InteractionEnabled())
        {
            GameObject modal = Instantiate(itemShopModal, uIParent.transform);
            BuyItemsModal modalData = modal.GetComponent<BuyItemsModal>();

            modalData.InitParameters(inventoryObject,
                foodPrice, woodPrice, cannonballPrice);
        }
    }

    private void OnMouseEnter()
    {
        mouseIsFocused = true;
    }

    private void OnMouseExit()
    {
        mouseIsFocused = false;
    }
}
