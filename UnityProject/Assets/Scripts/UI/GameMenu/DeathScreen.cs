using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeathScreen : MonoBehaviour
{
    private static readonly string INTERACTION_LOCK_KEY = "Death_screen_key";

    [SerializeField]
    private GameObject respawnButton;
    [SerializeField]
    private GameObject exitButton;
    [SerializeField]
    private string menuSceneName;

    private GameLoader gameLoader;

    private Button respawnBtn;
    private Button exitBtn;

    private void Awake()
    {
        gameLoader = FindObjectOfType<GameLoader>();

        respawnBtn = respawnButton.GetComponent<Button>();
        exitBtn = exitButton.GetComponent<Button>();

        respawnBtn.onClick.AddListener(Respawn);
        exitBtn.onClick.AddListener(ExitToMainMenu);

        PlayerSceneInteraction.DisableInteraction(INTERACTION_LOCK_KEY);
        PlayerAttackMode.DisableAttack(INTERACTION_LOCK_KEY);
    }

    private void Respawn()
    {
        PlayerSceneInteraction.EnableInteraction(INTERACTION_LOCK_KEY);
        PlayerAttackMode.EnableAttack(INTERACTION_LOCK_KEY);
        Destroy(gameObject);
    }

    private void ExitToMainMenu()
    {
        PlayerSceneInteraction.EnableInteraction(INTERACTION_LOCK_KEY);
        PlayerAttackMode.EnableAttack(INTERACTION_LOCK_KEY);

        gameLoader.SaveScene();
        SceneManager.LoadScene(menuSceneName, LoadSceneMode.Single);
    }
}
