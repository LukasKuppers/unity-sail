using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenDropItemsButton : MonoBehaviour
{
    [SerializeField]
    private GameObject pickupSpawner;
    [SerializeField]
    private GameObject inventoryObject;
    [SerializeField]
    private GameObject shipPrefabManager;
    [SerializeField]
    private GameObject DropItemsModalPrefab;

    private void Start()
    {
        PlayerInputManager inputManager = InputReference.GetInputManager();
        inputManager.AddInputListener(InputEvent.DROP_ITEMS, OpenModal);
    }

    private void OpenModal()
    {
        if (PlayerSceneInteraction.InteractionEnabled())
        {
            GameObject modal = Instantiate(DropItemsModalPrefab, transform.parent);
            DropItemsModal modalController = modal.GetComponent<DropItemsModal>();

            modalController.InitParameters(pickupSpawner, inventoryObject, shipPrefabManager);

            AudioManager.GetInstance().Play(SoundMap.TURN_PAGE);
        }
    }
}
