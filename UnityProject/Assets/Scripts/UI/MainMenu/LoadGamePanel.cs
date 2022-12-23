using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LoadGamePanel : MonoBehaviour
{
    [SerializeField]
    private string sceneName;
    [SerializeField]
    private GameObject loadButtonObject;
    [SerializeField]
    private GameObject deleteButtonObject;
    [SerializeField]
    private GameObject textObject;

    private Button loadButton;
    private Button deleteButton;
    private TextMeshProUGUI text;

    private LoadGameModal parentModal;

    private void Awake()
    {
        loadButton = loadButtonObject.GetComponent<Button>();
        deleteButton = deleteButtonObject.GetComponent<Button>();
        text = textObject.GetComponent<TextMeshProUGUI>();

        parentModal = gameObject.GetComponentInParent<LoadGameModal>();

        loadButton.onClick.AddListener(LoadGame);
        deleteButton.onClick.AddListener(DeleteGame);
    }

    public void SetName(string name)
    {
        if (name != null && !name.Equals(""))
        {
            text.text = name;
        }
    }

    private void LoadGame()
    {
        LoadedGame.SetLoadedGame(text.text);
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    private void DeleteGame()
    {
        SavedGamesManager.RemoveGame(text.text);

        parentModal.ProcessDeletedGame();
        Destroy(gameObject);
    }
}
