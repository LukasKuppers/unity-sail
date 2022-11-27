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
        SAIL_TO_PORT, 
        SELL_TREASURE, 
        SELL_NAV_DATA, 
        MAP_INFO, 
        BUY_CANNONBALLS, 
        SAIL_TO_DUMMY_TARGET, 
        SHOOT_TARGET, 
        DESTROY_TARGET
    }

    [SerializeField]
    private GameObject playerInventoryObject;
    [SerializeField]
    private GameObject shipPrefabManagerObject;
    [SerializeField]
    private GameObject navDataManagerObject;
    [SerializeField]
    private GameObject treasureIslandObject;
    [SerializeField]
    private GameObject dummyTarget;
    [SerializeField]
    private GameObject portIslandObject;
    [SerializeField]
    private string menuSceneName;
    [SerializeField]
    private string instructionsTextResName;

    private PlayerInventory inventory;
    private ShipPrefabManager shipPrefabManager;
    private NavigationDataManager navDataManager;
    private TextSequenceDisplay textDisplay;

    private Stage currentStage;

    private void Start()
    {
        inventory = playerInventoryObject.GetComponent<PlayerInventory>();
        shipPrefabManager = shipPrefabManagerObject.GetComponent<ShipPrefabManager>();
        navDataManager = navDataManagerObject.GetComponent<NavigationDataManager>();

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
            case Stage.SAIL_TO_PORT:
                ship = shipPrefabManager.GetCurrentShip();
                if (Vector3.Distance(ship.transform.position, portIslandObject.transform.position) < 70)
                {
                    textDisplay.IncrementSequence();
                    currentStage = Stage.SELL_TREASURE;
                }
                break;
            case Stage.SELL_TREASURE:
                if (inventory.GetTreasureAmount() == 0)
                {
                    textDisplay.IncrementSequence();
                    currentStage = Stage.SELL_NAV_DATA;
                }
                break;
            case Stage.SELL_NAV_DATA:
                if (navDataManager.GetNumNavigatedIslands() == 0)
                {
                    textDisplay.IncrementSequence();
                    currentStage = Stage.MAP_INFO;
                }
                break;
            case Stage.MAP_INFO:
                if (Input.GetKeyDown(KeyCode.Tab))
                {
                    textDisplay.IncrementSequence();
                    currentStage = Stage.BUY_CANNONBALLS;
                }
                break;
            case Stage.BUY_CANNONBALLS:
                if (inventory.GetCannonballAmount() == 100)
                {
                    textDisplay.IncrementSequence();
                    currentStage = Stage.SAIL_TO_DUMMY_TARGET;
                }
                break;
            case Stage.SAIL_TO_DUMMY_TARGET:
                ship = shipPrefabManager.GetCurrentShip();
                if (Vector3.Distance(ship.transform.position, dummyTarget.transform.position) <= 70)
                {
                    textDisplay.IncrementSequence();
                    currentStage = Stage.SHOOT_TARGET;
                }
                break;
            case Stage.SHOOT_TARGET:
                IDamageable targetHealth = dummyTarget.GetComponent<IDamageable>();
                if (targetHealth.GetHealth() < targetHealth.GetMaxHealth())
                {
                    textDisplay.IncrementSequence();
                    currentStage = Stage.DESTROY_TARGET;
                }
                break;
            case Stage.DESTROY_TARGET:
                if (dummyTarget == null)
                {
                    textDisplay.IncrementSequence();
                }
                break;
        }

        if (currentStage == Stage.SHOOT_TARGET || currentStage == Stage.DESTROY_TARGET)
        {
            if (inventory.GetCannonballAmount() == 0)
                inventory.IncrementCannonball(10);
        }
    }

    public void CompleteTutorial()
    {
        SavedTutorialState.SetTutorialCompleted();
        SceneManager.LoadScene(menuSceneName, LoadSceneMode.Single);
    }
}
