using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ArenaDeathScreen : MonoBehaviour
{
    private static readonly string INTERACTION_LOCK_KEY = "arena_death_screen_lock_key";

    [SerializeField]
    private GameObject scoreTextObject;
    [SerializeField]
    private GameObject saveNameTextFieldObject;
    [SerializeField]
    private GameObject quitButtonObject;
    [SerializeField]
    private string menuSceneName;

    private TextMeshProUGUI scoreText;
    private InputField saveNameTextField;
    private Button quitButton;

    private int score;

    private void Start()
    {
        scoreText = scoreTextObject.GetComponent<TextMeshProUGUI>();
        saveNameTextField = saveNameTextFieldObject.GetComponent<InputField>();
        quitButton = quitButtonObject.GetComponent<Button>();

        quitButton.onClick.AddListener(QuitToMainMenu);

        PlayerSceneInteraction.DisableInteraction(INTERACTION_LOCK_KEY);
        PlayerAttackMode.DisableAttack(INTERACTION_LOCK_KEY);
    }

    private void SetScore(int score)
    {
        string scoreMsg = $"Waves Conquered: {score}";
        scoreText.text = scoreMsg;

        this.score = score;
    }

    private void QuitToMainMenu()
    {
        SceneManager.LoadScene(menuSceneName, LoadSceneMode.Single);
    }
}
