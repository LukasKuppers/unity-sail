using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ArenaWaveManager : MonoBehaviour
{
    private static readonly float WAVE_START_DELAY_TIME = 10.0f;
    private static readonly int NUM_SHIP_TYPES = 4;
    private static readonly EnemyType[] enemyTypes = new EnemyType[4]
    {
        EnemyType.TINY_SHIP_NAVY, EnemyType.SLOOP_NAVY, EnemyType.BRIG_NAVY, EnemyType.NAO_NAVY
    };
    private static readonly Dictionary<EnemyType, int> shipWeights = new Dictionary<EnemyType, int>()
    {
        { enemyTypes[0], 1 }, { enemyTypes[1], 2 },
        { enemyTypes[2], 3 }, { enemyTypes[3], 6 }
    };

    [SerializeField]
    private GameObject playerInventoryObject;
    [SerializeField]
    private GameObject[] enemyShipSpawners;
    [SerializeField]
    private GameObject WaveTextObject;
    [SerializeField]
    private int CoinRewardPerKill = 100;

    private PlayerInventory playerInventory;
    private TextMeshProUGUI waveText;
    private Dictionary<EnemyType, EnemyWaveSpawner> waveSpawners;
    private Dictionary<EnemyType, int> destroyedShipsTally;
    private Dictionary<EnemyType, int> shipCounts;

    private int currentWave = 1;
    private int calculatedWave = 0;

    private void Awake()
    {
        if (enemyShipSpawners.Length != NUM_SHIP_TYPES)
        {
            Debug.LogError("ArenaWaveManager:Start:4 ship spawners must be initialized");
            return;
        }

        playerInventory = playerInventoryObject.GetComponent<PlayerInventory>();
        waveText = WaveTextObject.GetComponent<TextMeshProUGUI>();
        SetWaveText();

        shipCounts = new Dictionary<EnemyType, int>();
        foreach (EnemyType shipType in enemyTypes)
        {
            shipCounts.Add(shipType, 0);
        }

        ResetDestroyedShipsTally();
        InitSpawners();
        StartCoroutine(SpawnWaveOnDelay(currentWave));
    }

    public int GetCurrentWave()
    {
        return currentWave;
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
        SetWaveText();

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

                    RewardPlayerWithCoins();
                });
                newShip.GetComponent<EnemyShipDestroyer>().DropGenerousResources();
            });
        }
    }

    private int GetNumShipsForWave(int wave, EnemyType shipType)
    {
        // allow caching of enemy counts
        if (calculatedWave == wave)
            return shipCounts[shipType];

        calculatedWave = wave;
        foreach (EnemyType sType in enemyTypes)
        {
            shipCounts[sType] = 0;
        }

        while (wave > 0)
        {
            for (int i = NUM_SHIP_TYPES - 1; i >= 0; i--)
            {
                int shipWeight = shipWeights[enemyTypes[i]];
                if (shipWeight <= wave)
                {
                    shipCounts[enemyTypes[i]]++;
                    wave -= shipWeight;
                    continue;
                }
            }
        }
        return shipCounts[shipType];
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

    private void RewardPlayerWithCoins()
    {
        playerInventory.IncrementCoin(CoinRewardPerKill);

        AudioManager.GetInstance().Play(SoundMap.COINS);
    }

    private void SetWaveText()
    {
        string msg = $"Wave {currentWave}";
        waveText.text = msg;
    }

    private IEnumerator SpawnWaveOnDelay(int wave)
    {
        yield return new WaitForSeconds(WAVE_START_DELAY_TIME);

        SpawnWave(wave);
    }
}
