using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ArenaLeaderboardEntry
{
    public int score;
    public string name;
}

public static class SavedArenaLeaderboard
{
    private static readonly string PLAYER_PREFS_KEY = "saved_arena_leaderboard_key";
    private static readonly int LEADERBOARD_SIZE = 10;

    public static void SaveArenaScore(ArenaLeaderboardEntry newEntry)
    {
        List<ArenaLeaderboardEntry> currentLeaderboard = GetArenaLeaderboard();

        int insertIndex = -1;
        for (int i = 0; i < currentLeaderboard.Count; i++)
        {
            ArenaLeaderboardEntry currentEntry = currentLeaderboard[i];
            if (newEntry.score >= currentEntry.score)
            {
                insertIndex = i;
                break;
            }
        }

        if (insertIndex == -1)
            currentLeaderboard.Add(newEntry);
        else
            currentLeaderboard.Insert(insertIndex, newEntry);

        if (currentLeaderboard.Count > LEADERBOARD_SIZE)
            currentLeaderboard.RemoveAt(LEADERBOARD_SIZE);

        SerializeAndSaveLeaderboard(currentLeaderboard);
    }

    public static List<ArenaLeaderboardEntry> GetArenaLeaderboard()
    {
        string savedJson = PlayerPrefs.GetString(PLAYER_PREFS_KEY);
        if (savedJson == null || savedJson == "")
            return new List<ArenaLeaderboardEntry>();

        List<ArenaLeaderboardEntry> savedLeaderbaord = JsonUtility
            .FromJson<List<ArenaLeaderboardEntry>>(savedJson);

        if (savedLeaderbaord == null)
            return new List<ArenaLeaderboardEntry>();

        return savedLeaderbaord;
    }

    private static void SerializeAndSaveLeaderboard(List<ArenaLeaderboardEntry> leaderboard)
    {
        string jsonString = JsonUtility.ToJson(leaderboard);
        PlayerPrefs.SetString(PLAYER_PREFS_KEY, jsonString);
    }
}
