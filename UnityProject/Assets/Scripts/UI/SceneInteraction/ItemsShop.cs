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
    private GameObject itemShopModal;
    [SerializeField]
    private int foodPrice = 1;
    [SerializeField]
    private int woodPrice = 1;
    [SerializeField]
    private int cannonballPrice = 1;

    private bool mouseIsFocused = false;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && mouseIsFocused && PlayerSceneInteraction.InteractionEnabled())
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
