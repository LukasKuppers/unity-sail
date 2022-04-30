using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureShop : MonoBehaviour
{
    [SerializeField]
    private GameObject inventoryObject;
    [SerializeField]
    private GameObject safeZoneObject;
    [SerializeField]
    private GameObject uIParent;
    [SerializeField]
    private GameObject modalPrefab;

    private SafeZone safeZone;
    private bool mouseIsFocused = false;

    private void Start()
    {
        safeZone = safeZoneObject.GetComponent<SafeZone>();
    }

    private void Update()
    {
        if (safeZone.ShipInZone())
        {
            if (Input.GetMouseButtonDown(0) && mouseIsFocused)
            {
                OpenModal();
            }
        }
    }

    private void OpenModal()
    {
        GameObject modal = Instantiate(modalPrefab, uIParent.transform);
        SellTreasureModal modalData = modal.GetComponent<SellTreasureModal>();

        modalData.SetInventory(inventoryObject);
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
