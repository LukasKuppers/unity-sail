using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cartographer : MonoBehaviour, IClickableObject
{
    [SerializeField]
    private GameObject uIParent;
    [SerializeField]
    private GameObject cartographerModalPrefab;
    [SerializeField]
    private GameObject navDataManager;
    [SerializeField]
    private GameObject islandMapsManager;
    [SerializeField]
    private GameObject inventoryObject;

    public void Interact(string _interactionLockKey)
    {
        GameObject modal = Instantiate(cartographerModalPrefab, uIParent.transform);
        CartographerModal data = modal.GetComponent<CartographerModal>();

        data.InitParameters(navDataManager, inventoryObject, islandMapsManager);
    }
}
