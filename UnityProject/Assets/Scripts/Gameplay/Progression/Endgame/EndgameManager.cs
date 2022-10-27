using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndgameManager : MonoBehaviour
{
    private static readonly string INSTRUCTION_TEXT_SOURCE_FILENAME = "EndgameInstructions";
    private static readonly int GATE_DELAY_TIME = 5;

    [SerializeField]
    private GameObject textSequenceDisplayObject;
    [SerializeField]
    private GameObject playerIslandVisitManager;
    [SerializeField]
    private GameObject arenaGateObject;

    private TextSequenceDisplay textDisplay;
    private IslandVisitManager visitManager;
    private EndgameAreaGate arenaGate;

    private void Start()
    {
        textDisplay = textSequenceDisplayObject.GetComponent<TextSequenceDisplay>();
        textDisplay.SetTextSourceFile(INSTRUCTION_TEXT_SOURCE_FILENAME);

        visitManager = playerIslandVisitManager.GetComponent<IslandVisitManager>();
        visitManager.AddSpecificVisitListener(Islands.TEMPLE_OF_THE_SEA_BEAST, InitiateEndgameSequence);

        arenaGate = arenaGateObject.GetComponent<EndgameAreaGate>();
    }

    private void InitiateEndgameSequence()
    {
        textDisplay.StartSequenceDisplay();

        StartCoroutine(CloseGateOnDelay(GATE_DELAY_TIME));
    }

    private IEnumerator CloseGateOnDelay(float delayTimeSec)
    {
        yield return new WaitForSeconds(delayTimeSec);
        arenaGate.CloseGate();
    }
}
