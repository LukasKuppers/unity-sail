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

    private Button button;

    private void Start()
    {
        button = gameObject.GetComponent<Button>();

        button.onClick.AddListener(CreateModal);
    }

    private void CreateModal()
    {
        GameObject modal = Instantiate(DropItemsModalPrefab, transform.parent);
        DropItemsModal modalController = modal.GetComponent<DropItemsModal>();

        modalController.InitParameters(pickupSpawner, inventoryObject, shipPrefabManager);
    }
}
