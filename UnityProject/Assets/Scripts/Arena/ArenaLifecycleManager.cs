using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaLifecycleManager : MonoBehaviour
{
    [SerializeField]
    private GameObject shipPrefabManagerObject;
    [SerializeField]
    private GameObject shipSafetyManagerObject;
    [SerializeField]
    private GameObject playerInventoryObject;

    [SerializeField]
    private int initFoodAmount;
    [SerializeField]
    private int initWoodAmount;
    [SerializeField]
    private int initCannonballAmount;

    private ShipPrefabManager shipPrefabManager;
    private ShipSafetyManager safetyManager;
    private PlayerInventory playerInventory;

    private void Start()
    {
        shipPrefabManager = shipPrefabManagerObject.GetComponent<ShipPrefabManager>();
        safetyManager = shipSafetyManagerObject.GetComponent<ShipSafetyManager>();
        playerInventory = playerInventoryObject.GetComponent<PlayerInventory>();

        StartCoroutine(InitOnDelay());
    }

    private IEnumerator InitOnDelay()
    {
        yield return null;

        shipPrefabManager.SpawnShip(0);
        safetyManager.SetShipSafety(false);

        playerInventory.SetFoodAmount(initFoodAmount);
        playerInventory.SetWoodAmount(initWoodAmount);
        playerInventory.SetCannonballAmount(initCannonballAmount);
    }
}
