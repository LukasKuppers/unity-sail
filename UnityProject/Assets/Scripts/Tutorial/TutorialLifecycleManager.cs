using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialLifecycleManager : MonoBehaviour
{
    [SerializeField]
    private GameObject shipPrefabManagerObject;
    [SerializeField]
    private string menuSceneName;
    [SerializeField]
    private string instructionsTextResName;

    private ShipPrefabManager shipPrefabManager;
    private TextSequenceDisplay textDisplay;

    private void Start()
    {
        shipPrefabManager = shipPrefabManagerObject.GetComponent<ShipPrefabManager>();
        textDisplay = gameObject.GetComponent<TextSequenceDisplay>();
        if (textDisplay == null)
            Debug.LogError("TutorialLifecycleManager:start: Must assign text sequence display to manager object");

        shipPrefabManager.SpawnShip(0);
        textDisplay.SetTextSourceFile(instructionsTextResName);
        textDisplay.StartSequenceDisplay();
    }

    public void CompleteTutorial()
    {
        SavedTutorialState.SetTutorialCompleted();
        SceneManager.LoadScene(menuSceneName, LoadSceneMode.Single);
    }
}
