using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyWaveSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyShipSpawnerObject;
    [SerializeField]
    private float spawnDelay = 5f;

    private EnemyShipSpawner shipSpawner;

    private System.Guid currentConfigId = System.Guid.Empty;
    private HashSet<GameObject> spawnedShips;

    private void Start()
    {
        shipSpawner = enemyShipSpawnerObject.GetComponent<EnemyShipSpawner>();
        spawnedShips = new HashSet<GameObject>();
    }

    public void RemoveAllShips()
    {
        StopSpawningShips();

        foreach (GameObject ship in spawnedShips)
        {
            Destroy(ship);
        }
        spawnedShips.Clear();
    }

    // maintain a wave of numShips enemies - when an enemy is destroyed, a new one spawns
    public void MaintainConstantWave(int numShips, EnemyType shipType)
    {
        if (!currentConfigId.Equals(System.Guid.Empty))
        {
            Debug.LogWarning("EnemyWaveSpawner:MaintainConstantWave: spawner already active");
            return;
        }

        if (numShips <= 0)
            return;

        currentConfigId = System.Guid.NewGuid();
        StartCoroutine(SpawnShipsOnDelay(numShips, shipType, (newShip) =>
        {
            System.Guid originalConfigId = currentConfigId;
            newShip.GetComponent<IDestructable>().AddDestructionListener((_) =>
            {
                if (currentConfigId.Equals(originalConfigId) && spawnedShips.Count < numShips)
                    SpawnShip(shipType);
            });
        }));
    }

    public void StopSpawningShips()
    {
        currentConfigId = System.Guid.Empty;
    }

    private IEnumerator SpawnShipsOnDelay(int numShips, EnemyType shipType, UnityAction<GameObject> spawnCallback)
    {
        System.Guid originalConfigId = currentConfigId;
        for (int i = 0; i < numShips; i++)
        {
            if (!originalConfigId.Equals(currentConfigId))
                yield break;    // exit if no longner in spawn mode

            GameObject newShip = SpawnShip(shipType);
            spawnCallback.Invoke(newShip);

            yield return new WaitForSeconds(spawnDelay);
        }
    }

    private GameObject SpawnShip(EnemyType shipType)
    {
        GameObject newShip = shipSpawner.SpawnShip(shipType, transform.position, AIShipMode.Agressive);
        spawnedShips.Add(newShip);

        newShip.GetComponent<IDestructable>().AddDestructionListener((_) =>
        {
            if (spawnedShips.Contains(newShip))
                spawnedShips.Remove(newShip);
        });

        return newShip;
    }
}
