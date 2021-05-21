using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NewGameModal : MonoBehaviour
{
    [SerializeField]
    private string sceneName;

    [SerializeField]
    private GameObject exitButtonObject;
    [SerializeField]
    private GameObject createButtonObject;
    [SerializeField]
    private GameObject textInput;

    private Button exitButton;
    private Button createButton;
    private TextMeshProUGUI text;

    private void Start()
    {
        exitButton = exitButtonObject.GetComponent<Button>();
        createButton = createButtonObject.GetComponent<Button>();
        text = textInput.GetComponent<TextMeshProUGUI>();

        exitButton.onClick.AddListener(CloseModal);
        createButton.onClick.AddListener(CreateGame);
    }

    private void CloseModal()
    {
        Destroy(gameObject);
    }

    private void CreateGame()
    {
        if (text.text != null || !text.text.Equals(""))
        {
            bool success = SavedGamesManager.CreateGame(text.text);
            if (success)
            {
                SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
            }
            else
            {
                text.text = "";
            }
        }
    }
}
