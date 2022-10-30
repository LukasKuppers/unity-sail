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

    private enum SpawnState { ACTIVE_SPAWN_MODE, PASSIVE };

    private SpawnState internalState = SpawnState.PASSIVE;
    private int numSpawnedShips = 0;

    private void Start()
    {
        shipSpawner = enemyShipSpawnerObject.GetComponent<EnemyShipSpawner>();
    }

    // maintain a wave of numShips enemies - when an enemy is destroyed, a new one spawns
    public void MaintainConstantWave(int numShips, EnemyType shipType)
    {
        if (internalState == SpawnState.ACTIVE_SPAWN_MODE)
        {
            Debug.LogWarning("EnemyWaveSpawner:MaintainConstantWave: spawner already active");
            return;
        }

        if (numShips <= 0)
            return;

        internalState = SpawnState.ACTIVE_SPAWN_MODE;
        StartCoroutine(SpawnShipsOnDelay(numShips, shipType, (newShip) =>
        {
            newShip.GetComponent<IDestructable>().AddDestructionListener((_) =>
            {
                numSpawnedShips--;
                if (internalState == SpawnState.ACTIVE_SPAWN_MODE && numSpawnedShips < numShips)
                    SpawnShip(shipType);
            });
        }));
    }

    public void StopSpawningShips()
    {
        internalState = SpawnState.PASSIVE;
    }

    private IEnumerator SpawnShipsOnDelay(int numShips, EnemyType shipType, UnityAction<GameObject> spawnCallback)
    {
        for (int i = 0; i < numShips; i++)
        {
            if (internalState == SpawnState.PASSIVE)
                yield break;    // exit if no longner in spawn mode

            GameObject newShip = SpawnShip(shipType);
            spawnCallback.Invoke(newShip);

            yield return new WaitForSeconds(spawnDelay);
        }
    }

    private GameObject SpawnShip(EnemyType shipType)
    {
        GameObject newShip = shipSpawner.SpawnShip(shipType, transform.position, AIShipMode.Agressive);
        numSpawnedShips++;
        return newShip;
    }
}
