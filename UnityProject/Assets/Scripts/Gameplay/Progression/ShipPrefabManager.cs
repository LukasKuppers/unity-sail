using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

    private UnityEvent spawnedNewShipEvent;
    private UnityEvent respawnedShipEvent;

    private int currentShipIndex;
    private GameObject currentShip;

    public void AddSpawnListener(UnityAction call)
    {
        if (spawnedNewShipEvent == null)
        {
            spawnedNewShipEvent = new UnityEvent();
        }
        spawnedNewShipEvent.AddListener(call);
    }

    public void AddRespawnListener(UnityAction call)
    {
        if (respawnedShipEvent == null)
            respawnedShipEvent = new UnityEvent();
        respawnedShipEvent.AddListener(call);
    }

    public int GetShipIndex()
    {
        return currentShipIndex;
    }

    public GameObject GetCurrentShip()
    {
        return currentShip;
    }

    public GameObject RespawnShip()
    {
        if (respawnedShipEvent != null)
            respawnedShipEvent.Invoke();

        return SpawnShip(currentShipIndex);
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
        currentShip.transform.rotation = respawnPoint.transform.rotation;

        // setup ship dependencies
        PhysicsAdvancedBuoyancy buoyancy = currentShip.GetComponent<PhysicsAdvancedBuoyancy>();
        PhysicsShipController shipController = currentShip.GetComponent<PhysicsShipController>();
        PlayerController playerController = currentShip.GetComponent<PlayerController>();
        PlayerCanonController cannonController = currentShip.GetComponent<PlayerCanonController>();
        PlayerDestroyer destroyer = currentShip.GetComponent<PlayerDestroyer>();
        WindFlag flag = currentShip.GetComponentInChildren<WindFlag>();

        buoyancy.SetWaveManager(waveManager);
        shipController.SetWindManager(windManager);
        playerController.SetWindManager(windManager);
        cannonController.SetInventory(inventoryManager);
        destroyer.SetRespawnPoint(respawnPoint);
        destroyer.SetPlayerInventory(inventoryManager);
        destroyer.SetPrefabManager(gameObject);
        flag.SetWindGenerator(windManager);

        // setup external dependencies to ship
        Follower waveFollower = waveObject.GetComponent<Follower>();
        Follower camFollower = cameraRoot.GetComponent<Follower>();
        ShipAttributesWindow debug = debugWindow.GetComponent<ShipAttributesWindow>();

        waveFollower.SetTarget(currentShip);
        camFollower.SetTarget(currentShip);
        debug.SetShipObject(currentShip);

        if (spawnedNewShipEvent != null)
        {
            spawnedNewShipEvent.Invoke();
        }
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
