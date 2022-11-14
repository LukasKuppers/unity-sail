using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaWaveManager : MonoBehaviour
{
    private static readonly float WAVE_START_DELAY_TIME = 10.0f;
    private static readonly int NUM_SHIP_TYPES = 4;
    private static readonly EnemyType[] enemyTypes = new EnemyType[4]
    {
        EnemyType.TINY_SHIP_NAVY, EnemyType.SLOOP_NAVY, EnemyType.BRIG_NAVY, EnemyType.NAO_NAVY
    };

    [SerializeField]
    private GameObject[] enemyShipSpawners;

    private Dictionary<EnemyType, EnemyWaveSpawner> waveSpawners;
    private Dictionary<EnemyType, int> destroyedShipsTally;

    private int currentWave = 1;

    private void Awake()
    {
        if (enemyShipSpawners.Length != NUM_SHIP_TYPES)
        {
            Debug.LogError("ArenaWaveManager:Start:4 ship spawners must be initialized");
            return;
        }

        ResetDestroyedShipsTally();
        InitSpawners();
        StartCoroutine(SpawnWaveOnDelay(currentWave));
    }

    private void InitSpawners()
    {
        waveSpawners = new Dictionary<EnemyType, EnemyWaveSpawner>();
        for (int i = 0; i < NUM_SHIP_TYPES; i++)
        {
            EnemyWaveSpawner spawner = enemyShipSpawners[i].GetComponent<EnemyWaveSpawner>();
            waveSpawners.Add(enemyTypes[i], spawner);
        }
    }


    private void StartNextWave()
    {
        currentWave++;
        ResetDestroyedShipsTally();

        StartCoroutine(SpawnWaveOnDelay(currentWave));
    }

    private void SpawnWave(int waveNum)
    {
        foreach (EnemyType shipType in enemyTypes)
        {
            int numShips = GetNumShipsForWave(waveNum, shipType);
            waveSpawners[shipType].SpawnSingleWave(numShips, shipType, (newShip) =>
            {
                newShip.GetComponent<IDestructable>().AddDestructionListener((_) =>
                {
                    destroyedShipsTally[shipType]++;
                    if (WaveDestroyed(waveNum, destroyedShipsTally))
                        StartNextWave();
                });
            });
        }
    }

    private int GetNumShipsForWave(int wave, EnemyType shipType)
    {
        return 1;
    }

    private bool WaveDestroyed(int currentWave, Dictionary<EnemyType, int> destroyedShipsTally)
    {
        foreach (EnemyType shipType in enemyTypes)
        {
            int numShipsForWave = GetNumShipsForWave(currentWave, shipType);
            if (destroyedShipsTally[shipType] < numShipsForWave)
                return false;
        }
        return true;
    }

    private void ResetDestroyedShipsTally()
    {
        if (destroyedShipsTally == null)
            destroyedShipsTally = new Dictionary<EnemyType, int>();

        foreach (EnemyType shipType in enemyTypes)
        {
            if (destroyedShipsTally.ContainsKey(shipType))
                destroyedShipsTally[shipType] = 0;
            else
                destroyedShipsTally.Add(shipType, 0);
        }
    }

    private IEnumerator SpawnWaveOnDelay(int wave)
    {
        yield return new WaitForSeconds(WAVE_START_DELAY_TIME);

        SpawnWave(wave);
    }
}
