using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipPrefabManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] shipTypePrefabs;
    [SerializeField]
    private GameObject waveManager;
    [SerializeField]
    private GameObject windManager;
    [SerializeField]
    private GameObject respawnPoint;
    [SerializeField]
    private GameObject inventoryManager;

    private GameObject currentShip;

    public GameObject SpawnShip(int shipTypeIndex)
    {
        if (currentShip != null)
        {
            DestroyCurrentShip();
        }

        currentShip = Instantiate(shipTypePrefabs[shipTypeIndex]);

        // setup ship dependencies
        PhysicsAdvancedBuoyancy buoyancy = currentShip.GetComponent<PhysicsAdvancedBuoyancy>();
        PhysicsShipController shipController = currentShip.GetComponent<PhysicsShipController>();
        PlayerController playerController = currentShip.GetComponent<PlayerController>();
        PlayerDestroyer destroyer = currentShip.GetComponent<PlayerDestroyer>();

        buoyancy.SetWaveManager(waveManager);
        shipController.SetWindManager(windManager);
        playerController.SetWindManager(windManager);
        destroyer.SetRespawnPoint(respawnPoint);
        destroyer.SetPlayerInventory(inventoryManager);

        // setup external dependencies to ship
        Follower waveFollower = waveManager.GetComponent<Follower>();
        waveFollower.SetTarget(currentShip);

        return currentShip;
    }

    public void DestroyCurrentShip()
    {
        if (currentShip != null)
        {
            Destroy(currentShip);
            currentShip = null;
            return;
        }

        Debug.LogWarning("ShipPrefabManager:DestroyCurrentShip: no player ship currently exists");
    }
}
