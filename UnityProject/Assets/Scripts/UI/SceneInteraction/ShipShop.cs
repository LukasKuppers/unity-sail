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
    private GameObject modalPrefab;

    [SerializeField]
    private string[] shipNames;
    [SerializeField]
    private int[] shipPrices;

    private bool mouseIsFocused = false;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && mouseIsFocused && PlayerSceneInteraction.InteractionEnabled())
        {
            OpenModal();
        }
    }

    private void OpenModal()
    {
        GameObject modal = Instantiate(modalPrefab, uIParent.transform);

        UpgradeShipModal modalData = modal.GetComponent<UpgradeShipModal>();
        modalData.InitParameters(inventoryObject, shipPrefabManager, shipNames, shipPrices);
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
