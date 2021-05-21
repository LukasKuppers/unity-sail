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

    private string gameName = "";
    private Button button;
    private TextMeshProUGUI text;

    private void Start()
    {
        button = gameObject.GetComponent<Button>();
        text = textObject.GetComponent<TextMeshProUGUI>();

        button.onClick.AddListener(LoadGame);
    }

    public void SetName(string name)
    {
        if (name != null && !name.Equals(""))
        {
            gameName = name;
            text.text = name;
        }
    }

    private void LoadGame()
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
}
