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
    private GameObject waveObject;
    [SerializeField]
    private GameObject respawnPoint;
    [SerializeField]
    private GameObject inventoryManager;
    [SerializeField]
    private GameObject cameraRoot;
    [SerializeField]
    private GameObject debugWindow;

    private int currentShipIndex;
    private GameObject currentShip;

    public int GetShipIndex()
    {
        return currentShipIndex;
    }

    public GameObject GetCurrentShip()
    {
        return currentShip;
    }

    public GameObject SpawnShip(int shipTypeIndex)
    {
        currentShipIndex = shipTypeIndex;

        if (currentShip != null)
        {
            DestroyCurrentShip();
        }

        currentShip = Instantiate(shipTypePrefabs[shipTypeIndex]);
        currentShip.transform.position = respawnPoint.transform.position;

        // setup ship dependencies
        PhysicsAdvancedBuoyancy buoyancy = currentShip.GetComponent<PhysicsAdvancedBuoyancy>();
        PhysicsShipController shipController = currentShip.GetComponent<PhysicsShipController>();
        PlayerController playerController = currentShip.GetComponent<PlayerController>();
        PlayerDestroyer destroyer = currentShip.GetComponent<PlayerDestroyer>();
        WindFlag flag = currentShip.GetComponentInChildren<WindFlag>();

        buoyancy.SetWaveManager(waveManager);
        shipController.SetWindManager(windManager);
        playerController.SetWindManager(windManager);
        destroyer.SetRespawnPoint(respawnPoint);
        destroyer.SetPlayerInventory(inventoryManager);
        flag.SetWindGenerator(windManager);

        // setup external dependencies to ship
        Follower waveFollower = waveObject.GetComponent<Follower>();
        Follower camFollower = cameraRoot.GetComponent<Follower>();
        ShipAttributesWindow debug = debugWindow.GetComponent<ShipAttributesWindow>();

        waveFollower.SetTarget(currentShip);
        camFollower.SetTarget(currentShip);
        debug.SetShipObject(currentShip);

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
