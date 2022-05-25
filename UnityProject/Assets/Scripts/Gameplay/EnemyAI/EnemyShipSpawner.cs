using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShipSpawner : MonoBehaviour
{
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
    private GameObject[] shipPrefabs;

    public GameObject SpawnShip(int shipIndex, AIShipMode mode, Vector3 position, Quaternion rotation)
    {
        GameObject newShip = Instantiate(shipPrefabs[shipIndex],
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

        return newShip;
    }

    public GameObject SpawnShip(int shipIndex, AIShipMode mode, Vector3 position)
    {
        return SpawnShip(shipIndex, mode, position, Quaternion.identity);
    }

    public GameObject SpawnRandomShip(Vector3 position, AIShipMode mode)
    {
        int randIndex = Random.Range(0, shipPrefabs.Length);
        return SpawnShip(randIndex, mode, position);
    }
}
