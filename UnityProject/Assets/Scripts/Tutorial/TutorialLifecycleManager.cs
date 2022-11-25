using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialLifecycleManager : MonoBehaviour
{
    private enum Stage
    {
        MOVE_CAMERA,
        STEER_SHIP,
        FACE_DIRECTION,
        SAIL_TO_TREASURE,
        PICKUP_TREASURE, 
        SAIL_TO_PORT
    }

    [SerializeField]
    private GameObject playerInventoryObject;
    [SerializeField]
    private GameObject shipPrefabManagerObject;
    [SerializeField]
    private GameObject treasureIslandObject;
    [SerializeField]
    private string menuSceneName;
    [SerializeField]
    private string instructionsTextResName;

    private PlayerInventory inventory;
    private ShipPrefabManager shipPrefabManager;
    private TextSequenceDisplay textDisplay;

    private Stage currentStage;

    private void Start()
    {
        inventory = playerInventoryObject.GetComponent<PlayerInventory>();
        shipPrefabManager = shipPrefabManagerObject.GetComponent<ShipPrefabManager>();
        textDisplay = gameObject.GetComponent<TextSequenceDisplay>();
        if (textDisplay == null)
            Debug.LogError("TutorialLifecycleManager:start: Must assign text sequence display to manager object");

        currentStage = Stage.MOVE_CAMERA;
        shipPrefabManager.SpawnShip(0);
        textDisplay.SetTextSourceFile(instructionsTextResName);
        textDisplay.StartSequenceDisplay();
    }

    private void Update()
    {
        switch (currentStage)
        {
            case Stage.MOVE_CAMERA:
                if (Input.GetMouseButtonDown(1))
                {
                    textDisplay.IncrementSequence();
                    currentStage = Stage.STEER_SHIP;
                }
                break;
            case Stage.STEER_SHIP:
                if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
                {
                    textDisplay.IncrementSequence();
                    currentStage = Stage.FACE_DIRECTION;
                }
                break;
            case Stage.FACE_DIRECTION:
                GameObject ship = shipPrefabManager.GetCurrentShip();
                float rot = ship.transform.eulerAngles.y;
                if (rot >= 260f && rot <= 280f)
                {
                    textDisplay.IncrementSequence();
                    currentStage = Stage.SAIL_TO_TREASURE;
                }
                break;
            case Stage.SAIL_TO_TREASURE:
                ship = shipPrefabManager.GetCurrentShip();
                if (Vector3.Distance(ship.transform.position, treasureIslandObject.transform.position) < 50f)
                {
                    textDisplay.IncrementSequence();
                    currentStage = Stage.PICKUP_TREASURE;
                }
                break;
            case Stage.PICKUP_TREASURE:
                if (inventory.GetTreasureAmount() == 1)
                {
                    textDisplay.IncrementSequence();
                    currentStage = Stage.SAIL_TO_PORT;
                }
                break;
        }
    }

    public void CompleteTutorial()
    {
        SavedTutorialState.SetTutorialCompleted();
        SceneManager.LoadScene(menuSceneName, LoadSceneMode.Single);
    }
}
