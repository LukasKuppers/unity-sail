using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartArenaButton : MonoBehaviour
{
    [SerializeField]
    private string arenaSceneName;
    [SerializeField]
    private GameObject launchTutorialModalPrefab;
    [SerializeField]
    private GameObject HUDParent;

    private Button startButton;

    private void Start()
    {
        startButton = gameObject.GetComponent<Button>();

        startButton.onClick.AddListener(StartArena);
    }

    private void StartArena()
    {
        if (!SavedTutorialState.TutorialIsCompleted())
        {
            Instantiate(launchTutorialModalPrefab, HUDParent.transform);
            return;
        }

        LoadedGame.SetLoadedGame(arenaSceneName);
        SceneManager.LoadScene(arenaSceneName, LoadSceneMode.Single);
    }
}
