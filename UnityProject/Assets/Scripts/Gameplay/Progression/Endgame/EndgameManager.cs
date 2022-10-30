using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndgameManager : MonoBehaviour
{
    private static readonly string INSTRUCTION_TEXT_SOURCE_FILENAME = "EndgameInstructions";
    private static readonly int GATE_DELAY_TIME = 10;

    [SerializeField]
    private GameObject textSequenceDisplayObject;
    [SerializeField]
    private GameObject playerIslandVisitManager;
    [SerializeField]
    private GameObject playerShipPrefabManager;
    [SerializeField]
    private GameObject arenaGateObject;
    [SerializeField]
    private GameObject[] arenaBeaconObjects;
    [SerializeField]
    private GameObject[] waveSpanerObjects;

    private TextSequenceDisplay textDisplay;
    private IslandVisitManager visitManager;
    private ShipPrefabManager playerPrefabManager;
    private EndgameAreaGate arenaGate;
    private TempleBeaconManager[] arenaBeacons;
    private EnemyWaveSpawner[] spawners;

    private bool playerInArena = false;
    private bool playerBeatEndgame = false;
    private int currentSequenceIndex = 0;

    class WaveSpawnerConfig
    {
        public int numShips;
        public EnemyType shipType;
    }

    private Dictionary<int, Dictionary<int, WaveSpawnerConfig>> spawnerConfig = new Dictionary<int, Dictionary<int, WaveSpawnerConfig>>
    {
        { 0, new Dictionary<int, WaveSpawnerConfig>
        {
            { 0, new WaveSpawnerConfig(){ numShips=4, shipType=EnemyType.TINY_SHIP_NAVY } },
            { 1, new WaveSpawnerConfig(){ numShips=3, shipType=EnemyType.SLOOP_NAVY } }
        } },
        { 1, new Dictionary<int, WaveSpawnerConfig>
        {
            { 0, new WaveSpawnerConfig(){ numShips=3, shipType=EnemyType.SLOOP_NAVY} },
            { 1, new WaveSpawnerConfig(){ numShips=2, shipType=EnemyType.BRIG_NAVY} }
        } },
        { 2, new Dictionary<int, WaveSpawnerConfig>
        {
            { 0, new WaveSpawnerConfig(){ numShips=1, shipType=EnemyType.BRIG_NAVY} },
            { 1, new WaveSpawnerConfig(){ numShips=1, shipType=EnemyType.BRIG_NAVY} }
        } }
    };

    private void Start()
    {
        textDisplay = textSequenceDisplayObject.GetComponent<TextSequenceDisplay>();
        textDisplay.SetTextSourceFile(INSTRUCTION_TEXT_SOURCE_FILENAME);

        visitManager = playerIslandVisitManager.GetComponent<IslandVisitManager>();
        visitManager.AddSpecificVisitListener(Islands.TEMPLE_OF_THE_SEA_BEAST, InitiateEndgameSequence);

        playerPrefabManager = playerShipPrefabManager.GetComponent<ShipPrefabManager>();
        playerPrefabManager.AddRespawnListener(() => {
            if (playerInArena)
                ResetEndgameSequence();
        });

        arenaGate = arenaGateObject.GetComponent<EndgameAreaGate>();
        arenaBeacons = new TempleBeaconManager[arenaBeaconObjects.Length];
        for (int i = 0; i < arenaBeacons.Length; i++)
        {
            arenaBeacons[i] = arenaBeaconObjects[i].GetComponent<TempleBeaconManager>();
            arenaBeacons[i].GetCurrentBeaconObject().GetComponent<IDestructable>()
                .AddDestructionListener(OnBeaconDestroyed);
        }
        spawners = new EnemyWaveSpawner[2];
        for (int i = 0; i < 2; i++)
        {
            spawners[i] = waveSpanerObjects[i].GetComponent<EnemyWaveSpawner>();
        }
    }

    public bool GetPlayerInEndgame()
    {
        return playerInArena;
    }

    public bool PlayerBeatEndgame()
    {
        return playerBeatEndgame;
    }

    public int GetCurrentSequenceIndex()
    {
        return currentSequenceIndex;
    }

    // to be used by load module
    public void SetPlayerBeatEndgame()
    {
        playerBeatEndgame = true;

        foreach (TempleBeaconManager beacon in arenaBeacons)
        {
            beacon.SetBeaconDestroyed();
        }
        arenaGate.OpenGate();
        StopSpawners();
    }

    // to be used by load module - assumes player in endgame
    public void SetSequenceIndex(int sequenceIndex)
    {
        playerInArena = true;
        textDisplay.StartSequenceDisplay();
        arenaGate.SetStateNoAnim(false);

        if (sequenceIndex >= 1)
            arenaBeacons[0].SetBeaconDestroyed();
        if (sequenceIndex >= 2)
            arenaBeacons[1].SetBeaconDestroyed();
    }

    private void InitiateEndgameSequence()
    {
        if (!playerInArena)
        {
            playerInArena = true;
            textDisplay.StartSequenceDisplay();

            StartCoroutine(CloseGateOnDelay(GATE_DELAY_TIME));

            StartSpawnerConfig(0);
        }
    }

    public void ResetEndgameSequence()
    {
        playerInArena = false;
        arenaGate.OpenGate();
        textDisplay.EndSequenceDisplay();

        foreach (TempleBeaconManager beacon in arenaBeacons)
        {
            beacon.RespawnBeacon();
        }
        StopSpawners();
    }

    private void OnBeaconDestroyed(GameObject _)
    {
        IncrementSequenceIndex();

        if (currentSequenceIndex == 2)
        {
            // initiate final endgame stage
        }
        else if (currentSequenceIndex > 2)
            Debug.LogError("EndgameManager:OnBeaconDestroyed: sequence index invalid");
    }

    private void IncrementSequenceIndex()
    {
        currentSequenceIndex++;
        textDisplay.IncrementSequence();

        StopSpawners();
        StartSpawnerConfig(currentSequenceIndex);
    }

    private void StartSpawnerConfig(int sequenceIndex)
    {
        for (int i = 0; i < 2; i++)
        {
            WaveSpawnerConfig config = spawnerConfig[sequenceIndex][i];
            spawners[i].StopSpawningShips();
            spawners[i].MaintainConstantWave(config.numShips, config.shipType);
        }
    }

    private void StopSpawners()
    {
        foreach(EnemyWaveSpawner spawner in spawners)
        {
            spawner.StopSpawningShips();
        }
    }

    private IEnumerator CloseGateOnDelay(float delayTimeSec)
    {
        yield return new WaitForSeconds(delayTimeSec);
        arenaGate.CloseGate();
    }
}
