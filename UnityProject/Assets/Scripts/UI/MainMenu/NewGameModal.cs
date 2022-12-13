using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NewGameModal : MonoBehaviour
{
    [SerializeField]
    private string sceneName;

    [SerializeField]
    private GameObject launchTutorialModalPrefab;
    [SerializeField]
    private GameObject exitButtonObject;
    [SerializeField]
    private GameObject createButtonObject;
    [SerializeField]
    private GameObject textInput;

    private Button exitButton;
    private Button createButton;
    private TMP_InputField text;

    private void Start()
    {
        exitButton = exitButtonObject.GetComponent<Button>();
        createButton = createButtonObject.GetComponent<Button>();
        text = textInput.GetComponent<TMP_InputField>();

        exitButton.onClick.AddListener(CloseModal);
        createButton.onClick.AddListener(CreateGame);
    }

    private void CloseModal()
    {
        Destroy(gameObject);
    }

    private void CreateGame()
    {
        string input = text.text.Trim();

        if (!string.IsNullOrEmpty(input))
        {
            if (!SavedTutorialState.TutorialIsCompleted())
            {
                Instantiate(launchTutorialModalPrefab, gameObject.transform.parent);
                return;
            }

            bool success = SavedGamesManager.CreateGame(input);
            if (success)
            {
                LoadedGame.SetLoadedGame(input);
                SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
            }
            else
            {
                text.text = "";
                AudioManager.GetInstance().Play(SoundMap.ERROR);
            }
        }
        else
            AudioManager.GetInstance().Play(SoundMap.ERROR);
    }
}
