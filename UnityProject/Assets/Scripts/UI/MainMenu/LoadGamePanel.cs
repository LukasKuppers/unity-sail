using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LoadGamePanel : MonoBehaviour
{
    [SerializeField]
    private string sceneName;
    [SerializeField]
    private GameObject textObject;

    private Button button;
    private TextMeshProUGUI text;

    private void Awake()
    {
        button = gameObject.GetComponent<Button>();
        text = textObject.GetComponent<TextMeshProUGUI>();

        button.onClick.AddListener(LoadGame);
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
}
