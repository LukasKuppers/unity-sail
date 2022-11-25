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
        PICKUP_TREASURE
    }

    [SerializeField]
    private GameObject shipPrefabManagerObject;
    [SerializeField]
    private string menuSceneName;
    [SerializeField]
    private string instructionsTextResName;

    private ShipPrefabManager shipPrefabManager;
    private TextSequenceDisplay textDisplay;

    private Stage currentStage;

    private void Start()
    {
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
        }
    }

    public void CompleteTutorial()
    {
        SavedTutorialState.SetTutorialCompleted();
        SceneManager.LoadScene(menuSceneName, LoadSceneMode.Single);
    }
}
