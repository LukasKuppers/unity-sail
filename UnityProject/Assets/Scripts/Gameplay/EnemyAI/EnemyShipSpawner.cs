using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShipSpawner : MonoBehaviour
{
    private static readonly float COROUTINE_TICK_TIME = 5.0f;

    [SerializeField]
    private GameObject shipPrefabManager;
    [SerializeField]
    private GameObject shipSafetyManager;
    [SerializeField]
    private GameObject waveManager;
    [SerializeField]
    private GameObject windManager;
    [SerializeField]
    private GameObject[] shipPrefabs;
    [SerializeField]
    private int numShips = 1;
    [SerializeField]
    private float spawnRadius = 500f;
    [SerializeField]
    private float agressionRadius = 250f;

    private ShipPrefabManager shipManager;
    private GameObject playerShip;
    private GameObject[] spawnedShips;

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
        Gizmos.DrawWireSphere(transform.position, agressionRadius);
    }

    private void Start()
    {
        if (agressionRadius > spawnRadius)
        {
            Debug.LogWarning("EnemyShipSpawner: Agression radius should be less than spawn radius");
        }

        shipManager = shipPrefabManager.GetComponent<ShipPrefabManager>();
        shipManager.AddSpawnListener(UpdateShip);

        spawnedShips = new GameObject[numShips];
    }

    private void Awake()
    {
        StartCoroutine(ControlAIBehaviour());
    }

    private void UpdateShip()
    {
        playerShip = shipManager.GetCurrentShip();
    }

    private IEnumerator ControlAIBehaviour()
    {
        while (true)
        {
            if (playerShip != null)
            {
                float dist = Vector3.Distance(transform.position, playerShip.transform.position);

                if (dist <= spawnRadius)
                {
                    SpawnShips();
                    if (dist <= agressionRadius)
                    {
                        SetShipModes(AIShipMode.Agressive);
                    }
                    else
                    {
                        SetShipModes(AIShipMode.Passive);
                    }
                }
                else
                {
                    KillShips();
                }
            }
            yield return new WaitForSeconds(COROUTINE_TICK_TIME);
        }
    }

    private void SpawnShips()
    {
        if (spawnedShips[0] == null)
        {
            for (int i = 0; i < numShips; i++)
            {
                int shipIndex = Random.Range(0, shipPrefabs.Length);
                Quaternion randRot = Quaternion.Euler(0, Random.Range(0f, 360f), 0);
                Vector3 randPosOffset = new Vector3(Random.Range(-25f, 25f), 10, Random.Range(-25f, 25f));

                GameObject newShip = Instantiate(shipPrefabs[shipIndex],
                    transform.position + randPosOffset,
                    randRot,
                    transform);

                PhysicsAdvancedBuoyancy buoyancy = newShip.GetComponent<PhysicsAdvancedBuoyancy>();
                PhysicsShipController controller = newShip.GetComponent<PhysicsShipController>();
                ShipAutomation shipAutomation = newShip.GetComponent<ShipAutomation>();
                AIShipController aiController = newShip.GetComponent<AIShipController>();
                WindFlag flag = newShip.GetComponentInChildren<WindFlag>();
                AICanonController[] cannons = newShip.GetComponentsInChildren<AICanonController>();

                buoyancy.SetWaveManager(waveManager);
                controller.SetWindManager(windManager);
                shipAutomation.SetWindManager(windManager);
                aiController.SetShipPrefabManager(shipPrefabManager);
                aiController.SetShipSafetyManager(shipSafetyManager);
                flag.SetWindGenerator(windManager);
                foreach (AICanonController cannon in cannons)
                {
                    cannon.SetShipPrefabManager(shipPrefabManager);
                }

                aiController.SetMode(AIShipMode.Anchored);

                spawnedShips[i] = newShip;
            }
        }
    }

    private void KillShips()
    {
        if (spawnedShips[0] != null)
        {
            for (int i = 0; i < numShips; i++)
            {
                Destroy(spawnedShips[i]);
            }
        }
    }

    private void SetShipModes(AIShipMode mode)
    {
        foreach (GameObject ship in spawnedShips)
        {
            AIShipController controller = ship.GetComponent<AIShipController>();
            controller.SetMode(mode);
        }
    }
}
