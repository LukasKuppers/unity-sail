using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndgameManager : MonoBehaviour
{
    private static readonly string INSTRUCTION_TEXT_SOURCE_FILENAME = "EndgameInstructions";
    private static readonly int GATE_DELAY_TIME = 10;
    private static readonly int MAX_SEQUENCE_INDEX = 2;
    private static readonly int END_NOTE_INDEX = 14;

    [SerializeField]
    private GameObject enemySpawnerObject;
    [SerializeField]
    private GameObject seabeastManagerObject;
    [SerializeField]
    private GameObject pickupGeneratorObject;
    [SerializeField]
    private GameObject textSequenceDisplayObject;
    [SerializeField]
    private GameObject playerIslandVisitManager;
    [SerializeField]
    private GameObject playerShipPrefabManager;
    [SerializeField]
    private GameObject storyNoteManagerObject;
    [SerializeField]
    private GameObject arenaGateObject;
    [SerializeField]
    private GameObject[] arenaBeaconObjects;
    [SerializeField]
    private GameObject[] waveSpawnerObjects;

    private EnemyShipSpawner enemySpawner;
    private SeaBeastManager seaBeastManager;
    private PickupGenerator pickupGenerator;
    private TextSequenceDisplay textDisplay;
    private IslandVisitManager visitManager;
    private ShipPrefabManager playerPrefabManager;
    private StoryNotesManager storyNotesManager;
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
        enemySpawner = enemySpawnerObject.GetComponent<EnemyShipSpawner>();
        seaBeastManager = seabeastManagerObject.GetComponent<SeaBeastManager>();
        pickupGenerator = pickupGeneratorObject.GetComponent<PickupGenerator>();
        storyNotesManager = storyNoteManagerObject.GetComponent<StoryNotesManager>();

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
            spawners[i] = waveSpawnerObjects[i].GetComponent<EnemyWaveSpawner>();
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
        seaBeastManager.DisableActiveAgression();
    }

    // to be used by load module - assumes player in endgame
    public void SetSequenceIndex(int sequenceIndex)
    {
        playerInArena = true;
        textDisplay.StartSequenceDisplay();
        arenaGate.SetStateNoAnim(false);

        seaBeastManager.EnableActiveAgression();

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
            seaBeastManager.EnableActiveAgression();
        }
    }

    public void ResetEndgameSequence()
    {
        playerInArena = false;
        arenaGate.OpenGate();
        textDisplay.EndSequenceDisplay();

        seaBeastManager.DisableActiveAgression();
        
        foreach (EnemyWaveSpawner spawner in spawners)
        {
            spawner.RemoveAllShips();
        }

        foreach (TempleBeaconManager beacon in arenaBeacons)
        {
            beacon.RespawnBeacon();
        }
    }

    private void OnBeaconDestroyed(GameObject _)
    {
        IncrementSequenceIndex();

        if (currentSequenceIndex > 2)
            Debug.LogError("EndgameManager:OnBeaconDestroyed: sequence index invalid");
    }

    private void IncrementSequenceIndex()
    {
        currentSequenceIndex++;
        textDisplay.IncrementSequence();

        StartSpawnerConfig(currentSequenceIndex);

        if (currentSequenceIndex >= MAX_SEQUENCE_INDEX)
            InitializeFinalBoss();
    }

    private void InitializeFinalBoss()
    {
        Vector3 spawnPoint = waveSpawnerObjects[0].transform.position;
        GameObject bossShip = enemySpawner.SpawnShip(EnemyType.BOSS_NAVY, spawnPoint, AIShipMode.Agressive);

        bossShip.GetComponent<IDestructable>().AddDestructionListener((destObj) =>
        {
            if (playerInArena)
            {
                playerBeatEndgame = true;

                textDisplay.EndSequenceDisplay();
                arenaGate.OpenGate();
                StopSpawners();
                storyNotesManager.DiscoverNote(END_NOTE_INDEX);

                Vector3 pos = destObj.transform.position;
                pickupGenerator.SpawnPickup(Item.TREASURE, 10, (pos + Vector3.right * 5));
                pickupGenerator.SpawnPickup(Item.FOOD, 10, (pos + Vector3.forward * 5));
                pickupGenerator.SpawnPickup(Item.WOOD, 200, (pos + Vector3.left * 5));
                pickupGenerator.SpawnPickup(Item.CANNONBALL, 200, (pos + Vector3.back * 5));
            }
        });
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
