using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameMenuModal : MonoBehaviour
{
    private static readonly string INTERACTION_LOCK_KEY = "Game_menu_key";

    [SerializeField]
    private GameObject exitButton;
    [SerializeField]
    private GameObject quitButton;
    [SerializeField]
    private GameObject mainContent;
    [SerializeField]
    private string menuSceneName;
    [SerializeField]
    private GameObject[] pages;
    [SerializeField]
    private GameObject[] pageButtons;

    private Button quitBtn;
    private Button exitBtn;

    private GameLoader gameLoader;

    private void Awake()
    {
        quitBtn = quitButton.GetComponent<Button>();
        exitBtn = exitButton.GetComponent<Button>();

        quitBtn.onClick.AddListener(SaveAndQuit);
        exitBtn.onClick.AddListener(CloseMenu);

        ClosePages();
        InitPageButtons();

        PlayerSceneInteraction.DisableInteraction(INTERACTION_LOCK_KEY);
    }

    public void InitParameters(GameObject gameLoader)
    {
        this.gameLoader = gameLoader.GetComponent<GameLoader>();
    }

    private void CloseMenu()
    {
        PlayerSceneInteraction.EnableInteraction(INTERACTION_LOCK_KEY);
        Destroy(gameObject);
    }

    private void InitPageButtons()
    {
        for (int i = 0; i < pages.Length; i++)
        {
            Button pageBtn = pageButtons[i].GetComponent<Button>();
            int index = int.Parse(i.ToString());
            pageBtn.onClick.AddListener(() => OpenPage(index));
        }
    }

    private void OpenPage(int pageIndex)
    {
        mainContent.SetActive(false);
        pages[pageIndex].SetActive(true);
    }

    public void ClosePages()
    {
        foreach (GameObject page in pages)
            page.SetActive(false);

        mainContent.SetActive(true);
    }

    private void SaveAndQuit()
    {
        PlayerSceneInteraction.EnableInteraction(INTERACTION_LOCK_KEY);
        gameLoader.SaveScene();
        SceneManager.LoadScene(menuSceneName, LoadSceneMode.Single);
    }
}
