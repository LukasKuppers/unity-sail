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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && PlayerSceneInteraction.InteractionEnabled())
        {
            GameObject modal = Instantiate(DropItemsModalPrefab, transform.parent);
            DropItemsModal modalController = modal.GetComponent<DropItemsModal>();

            modalController.InitParameters(pickupSpawner, inventoryObject, shipPrefabManager);
        }
    }
}
