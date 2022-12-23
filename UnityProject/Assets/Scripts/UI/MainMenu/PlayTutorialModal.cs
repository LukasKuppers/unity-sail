using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayTutorialModal : MonoBehaviour
{
    [SerializeField]
    private GameObject exitButtonObject;
    [SerializeField]
    private GameObject launchTutorialButtonObject;
    [SerializeField]
    private string tutorialSceneName;

    private Button exitButton;
    private Button launchTutorialButton;

    private void Start()
    {
        exitButton = exitButtonObject.GetComponent<Button>();
        launchTutorialButton = launchTutorialButtonObject.GetComponent<Button>();

        exitButton.onClick.AddListener(CloseModal);
        launchTutorialButton.onClick.AddListener(LaunchTutorial);

        MenuModalLock.OpenModal();
    }

    private void CloseModal()
    {
        MenuModalLock.CloseModal();
        Destroy(gameObject);
    }

    private void LaunchTutorial()
    {
        MenuModalLock.CloseModal();

        LoadedGame.SetLoadedGame(tutorialSceneName);
        SceneManager.LoadScene(tutorialSceneName, LoadSceneMode.Single);
    }
}
