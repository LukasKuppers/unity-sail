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
    [SerializeField]
    private string arenaManagerTag = "ArenaManager";

    private TextMeshProUGUI scoreText;
    private TextMeshProUGUI saveNameTextField;
    private Button quitButton;

    int score;

    private void Start()
    {
        scoreText = scoreTextObject.GetComponent<TextMeshProUGUI>();
        saveNameTextField = saveNameTextFieldObject.GetComponent<TextMeshProUGUI>();
        quitButton = quitButtonObject.GetComponent<Button>();

        quitButton.onClick.AddListener(QuitToMainMenu);
        GetScore();

        PlayerSceneInteraction.DisableInteraction(INTERACTION_LOCK_KEY);
        PlayerAttackMode.DisableAttack(INTERACTION_LOCK_KEY);
    }

    private void GetScore()
    {
        ArenaWaveManager waveManager = GameObject.FindGameObjectWithTag(arenaManagerTag)
            .GetComponent<ArenaWaveManager>();

        score = waveManager.GetCurrentWave();
        scoreText.text = $"Waves Conquered: {score}";
    }

    private void QuitToMainMenu()
    {
        string saveName = saveNameTextField.text;
        if (saveName != null && saveName != "")
        {
            ArenaLeaderboardEntry newEntry = new ArenaLeaderboardEntry()
            {
                score = score,
                name = saveName
            };

            SavedArenaLeaderboard.SaveArenaScore(newEntry);
        }

        PlayerSceneInteraction.EnableInteraction(INTERACTION_LOCK_KEY);
        PlayerAttackMode.EnableAttack(INTERACTION_LOCK_KEY);

        SceneManager.LoadScene(menuSceneName, LoadSceneMode.Single);
    }
}
