using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyShipSpawnableType
{
    public EnemyType shipType;

    public GameObject shipPrefab;
}

public class EnemyShipSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject npcActivtyManager;
    [SerializeField]
    private GameObject shipPrefabManager;
    [SerializeField]
    private GameObject shipSafetyManager;
    [SerializeField]
    private GameObject pickupSpawner;
    [SerializeField]
    private GameObject waveManager;
    [SerializeField]
    private GameObject windManager;
    [SerializeField]
    private EnemyShipSpawnableType[] ships;

    private DistributedTaskManager taskManager;
    private NPCActivityManager npcManager;

    private Dictionary<EnemyType, GameObject> shipPrefabMap;

    private void Start()
    {
        taskManager = DistributedTaskManager.GetInstance();
        npcManager = npcActivtyManager.GetComponent<NPCActivityManager>();

        shipPrefabMap = new Dictionary<EnemyType, GameObject>();
        foreach (EnemyShipSpawnableType shipType in ships)
        {
            shipPrefabMap.Add(shipType.shipType, shipType.shipPrefab);
        }
    }

    public GameObject SpawnShip(EnemyType type, AIShipMode mode, Vector3 position, 
                                Quaternion rotation, bool isRouteShip)
    {
        GameObject newShip = Instantiate(shipPrefabMap[type],
            position, rotation, transform);

        PhysicsAdvancedBuoyancy buoyancy = newShip.GetComponent<PhysicsAdvancedBuoyancy>();
        PhysicsShipController controller = newShip.GetComponent<PhysicsShipController>();
        ShipAutomation shipAutomation = newShip.GetComponent<ShipAutomation>();
        AIShipController aiController = newShip.GetComponent<AIShipController>();
        WindFlag flag = newShip.GetComponentInChildren<WindFlag>();
        AICanonController[] cannons = newShip.GetComponentsInChildren<AICanonController>();
        EnemyShipDestroyer shipDestroyer = newShip.GetComponent<EnemyShipDestroyer>();

        buoyancy.SetWaveManager(waveManager);
        controller.SetWindManager(windManager);
        shipAutomation.SetWindManager(windManager);
        aiController.SetShipPrefabManager(shipPrefabManager);
        aiController.SetShipSafetyManager(shipSafetyManager);
        flag.SetWindGenerator(windManager);
        shipDestroyer.SetPickupGenerator(pickupSpawner);

        foreach (AICanonController cannon in cannons)
        {
            cannon.SetShipPrefabManager(shipPrefabManager);
        }

        aiController.SetMode(mode);
        taskManager.AddTask(newShip);
        npcManager.RegisterNewShip(newShip, isRouteShip);

        return newShip;
    }

    public GameObject SpawnShip(EnemyType type, AIShipMode mode, Vector3 position, Quaternion rotation)
    {
        return SpawnShip(type, mode, position, rotation, false);
    }

    public GameObject SpawnShip(EnemyType type, Vector3 position, AIShipMode mode)
    {
        return SpawnShip(type, mode, position, Quaternion.identity);
    }

    public GameObject SpawnRandomShip(Vector3 position, AIShipMode mode)
    {
        EnemyType randType = GetRandType();
        return SpawnShip(randType, position, mode);
    }

    public GameObject SpawnRandomShip(Vector3 position, AIShipMode mode, out EnemyType type)
    {
        type = GetRandType();
        return SpawnShip(type, position, mode);
    }

    public GameObject SpawnRandomTravellingShip(Vector3 position, AIShipMode mode)
    {
        EnemyType randType = GetRandType();
        return SpawnShip(randType, mode, position, Quaternion.identity, true);
    }

    public GameObject SpawnRandomTravellingShip(Vector3 position, AIShipMode mode, out EnemyType type)
    {
        type = GetRandType();
        return SpawnShip(type, mode, position, Quaternion.identity, true);
    }

    private EnemyType GetRandType()
    {
        EnemyType randType;
        do
        {
            int randIndex = Random.Range(0, ships.Length);
            randType = ships[randIndex].shipType;
        }
        while (randType == EnemyType.BOSS_NAVY);

        return randType;
    }
}
