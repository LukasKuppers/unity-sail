using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDestroyer : MonoBehaviour, IDestructable
{
    [SerializeField]
    private GameObject respawnPoint;
    [SerializeField]
    private GameObject playerInventory;
    [SerializeField]
    private GameObject shipPrefabManager;

    [SerializeField]
    private int defaultFoodAmount = 0;
    [SerializeField]
    private int defaultWoodAmount = 0;
    [SerializeField]
    private int defaultCannonballAmount = 0;

    private PlayerInventory inventory;
    private ShipPrefabManager prefabManager;

    private void Start()
    {
        inventory = playerInventory.GetComponent<PlayerInventory>();
        prefabManager = shipPrefabManager.GetComponent<ShipPrefabManager>();
    }

    public void SetRespawnPoint(GameObject newRespawnPoint)
    {
        respawnPoint = newRespawnPoint;
    }

    public void SetPlayerInventory(GameObject newInventory)
    {
        playerInventory = newInventory;
        inventory = playerInventory.GetComponent<PlayerInventory>();
    }

    public void SetPrefabManager(GameObject newShipPrefabManager)
    {
        shipPrefabManager = newShipPrefabManager;
        prefabManager = shipPrefabManager.GetComponent<ShipPrefabManager>();
    }

    public void Destroy()
    {
        prefabManager.SpawnShip(prefabManager.GetShipIndex());

        inventory.SetFoodAmount(defaultFoodAmount);
        inventory.SetWoodAmount(defaultWoodAmount);
        inventory.SetCannonballAmount(defaultCannonballAmount);
        inventory.SetTreasureAmount(0);
    }
}
