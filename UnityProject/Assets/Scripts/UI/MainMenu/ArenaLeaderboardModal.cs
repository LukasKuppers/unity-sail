using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArenaLeaderboardModal : MonoBehaviour
{
    private static readonly string EMPTY_LEADERBOARD_MSG = "No arena games have been saved.";

    [SerializeField]
    private GameObject leaderboardEntryPrefab;
    [SerializeField]
    private GameObject exitButtonObject;
    [SerializeField]
    private GameObject scrollViewContentParent;

    private Button exitButton;

    private void Start()
    {
        exitButton = exitButtonObject.GetComponent<Button>();
        exitButton.onClick.AddListener(CloseModal);

        PopulateLeaderboardEntries();

        MenuModalLock.OpenModal();
    }

    private void PopulateLeaderboardEntries()
    {
        List<ArenaLeaderboardEntry> leaderboard = SavedArenaLeaderboard.GetArenaLeaderboard();

        if (leaderboard == null || leaderboard.Count <= 0)
        {
            GameObject msgContainer = Instantiate(leaderboardEntryPrefab, scrollViewContentParent.transform);
            msgContainer.GetComponent<LeaderboardEntry>().InitParameters(EMPTY_LEADERBOARD_MSG);
            return;
        }

        foreach (ArenaLeaderboardEntry entry in leaderboard)
        {
            int score = entry.score;
            string name = entry.name;

            GameObject entryObj = Instantiate(leaderboardEntryPrefab, scrollViewContentParent.transform);
            entryObj.GetComponent<LeaderboardEntry>().InitParameters(score, name);
        }
    }

    private void CloseModal()
    {
        MenuModalLock.CloseModal();
        Destroy(gameObject);
    }
}
