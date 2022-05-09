using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipShop : MonoBehaviour
{
    [SerializeField]
    private GameObject inventoryObject;
    [SerializeField]
    private GameObject shipPrefabManager;
    [SerializeField]
    private GameObject uIParent;
    [SerializeField]
    private GameObject safeZoneObject;
    [SerializeField]
    private GameObject modalPrefab;

    [SerializeField]
    private string[] shipNames;
    [SerializeField]
    private int[] shipPrices;

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
            GameObject modal = Instantiate(modalPrefab, uIParent.transform);

            UpgradeShipModal modalData = modal.GetComponent<UpgradeShipModal>();
            modalData.InitParameters(inventoryObject, shipPrefabManager, shipNames, shipPrices);
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
