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
        
        foreach (string name in gameNames)
        {
            GameObject panel = Instantiate(gameButtonPrefab, gameObject.transform.GetChild(0).transform);
            panel.GetComponent<LoadGamePanel>().SetName(name);
        }
    }

    private void CloseModal()
    {
        Destroy(gameObject);
    }
}
