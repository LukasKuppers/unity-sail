using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndgameManager : MonoBehaviour
{
    private static readonly string INSTRUCTION_TEXT_SOURCE_FILENAME = "EndgameInstructions";

    [SerializeField]
    private GameObject textSequenceDisplayObject;
    [SerializeField]
    private GameObject playerIslandVisitManager;

    private TextSequenceDisplay textDisplay;
    private IslandVisitManager visitManager;

    private void Start()
    {
        textDisplay = textSequenceDisplayObject.GetComponent<TextSequenceDisplay>();
        textDisplay.SetTextSourceFile(INSTRUCTION_TEXT_SOURCE_FILENAME);

        visitManager = playerIslandVisitManager.GetComponent<IslandVisitManager>();
        visitManager.AddSpecificVisitListener(Islands.TEMPLE_OF_THE_SEA_BEAST, InitiateEndgameSequence);
    }

    private void InitiateEndgameSequence()
    {
        textDisplay.StartSequenceDisplay();
    }
}
