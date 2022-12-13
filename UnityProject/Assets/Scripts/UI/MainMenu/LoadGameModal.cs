using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadGameModal : MonoBehaviour
{
    [SerializeField]
    private GameObject gameButtonPrefab;
    [SerializeField]
    private GameObject exitButton;
    [SerializeField]
    private GameObject emptyStateTextObject;

    private Button button;

    private void Awake()
    {
        button = exitButton.GetComponent<Button>();
        button.onClick.AddListener(CloseModal);

        PopulateGameList();
    }

    private void PopulateGameList()
    {
        string[] gameNames = SavedGamesManager.GetSavedGames();

        if (gameNames.Length <= 0)
        {
            emptyStateTextObject.SetActive(true);
            return;
        }
        else
            emptyStateTextObject.SetActive(false);

        foreach (string name in gameNames)
        {
            GameObject panel = Instantiate(gameButtonPrefab, gameObject.transform);
            panel.GetComponent<LoadGamePanel>().SetName(name);
        }
    }

    public void ProcessDeletedGame()
    {
        int numSaves = SavedGamesManager.GetSavedGames().Length;
        if (numSaves <= 0)
            emptyStateTextObject.SetActive(true);
    }

    private void CloseModal()
    {
        Destroy(gameObject);
    }
}
