using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartArenaButton : MonoBehaviour
{
    [SerializeField]
    private string arenaSceneName;

    private Button startButton;

    private void Start()
    {
        startButton = gameObject.GetComponent<Button>();

        startButton.onClick.AddListener(StartArena);
    }

    private void StartArena()
    {
        SceneManager.LoadScene(arenaSceneName, LoadSceneMode.Single);
    }
}
