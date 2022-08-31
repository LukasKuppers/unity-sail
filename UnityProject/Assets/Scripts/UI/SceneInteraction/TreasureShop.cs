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
        PlayerInputManager inputManager = InputReference.GetInputManager();
        inputManager.AddInputListener(InputEvent.MOUSE_LEFT, OpenModal);
    }

    private void OpenModal()
    {
        if (safeZone.ShipInZone() && mouseIsFocused && PlayerSceneInteraction.InteractionEnabled())
        {
            GameObject modal = Instantiate(modalPrefab, uIParent.transform);
            SellTreasureModal modalData = modal.GetComponent<SellTreasureModal>();

            modalData.SetInventory(inventoryObject);

            AudioManager.GetInstance().Play(SoundMap.TURN_PAGE);
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
