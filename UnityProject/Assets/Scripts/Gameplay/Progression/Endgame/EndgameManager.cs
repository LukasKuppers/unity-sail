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

    private TextSequenceDisplay textDisplay;
    private IslandVisitManager visitManager;
    private ShipPrefabManager playerPrefabManager;
    private EndgameAreaGate arenaGate;
    private TempleBeaconManager[] arenaBeacons;

    private bool playerInArena = false;

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
        }
    }

    private void InitiateEndgameSequence()
    {
        playerInArena = true;
        textDisplay.StartSequenceDisplay();

        StartCoroutine(CloseGateOnDelay(GATE_DELAY_TIME));
    }

    private void ResetEndgameSequence()
    {
        playerInArena = false;
        arenaGate.OpenGate();
        foreach (TempleBeaconManager beacon in arenaBeacons)
        {
            beacon.RespawnBeacon();
        }
    }

    private IEnumerator CloseGateOnDelay(float delayTimeSec)
    {
        yield return new WaitForSeconds(delayTimeSec);
        arenaGate.CloseGate();
    }
}
