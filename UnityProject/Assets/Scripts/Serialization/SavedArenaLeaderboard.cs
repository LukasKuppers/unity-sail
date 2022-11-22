using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ArenaLeaderboardEntry
{
    public int score;
    public string name;
}

[System.Serializable]
public class SerializedArenaLeaderboard
{
    public ArenaLeaderboardEntry[] leaderboard;
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

        SerializedArenaLeaderboard serializedLeaderboard = JsonUtility
            .FromJson<SerializedArenaLeaderboard>(savedJson);
       
        if (serializedLeaderboard == null || serializedLeaderboard.leaderboard == null ||
            serializedLeaderboard.leaderboard.Length <= 0)
            return new List<ArenaLeaderboardEntry>();

        return new List<ArenaLeaderboardEntry>(serializedLeaderboard.leaderboard);
    }

    private static void SerializeAndSaveLeaderboard(List<ArenaLeaderboardEntry> leaderboard)
    {
        SerializedArenaLeaderboard saveObj = new SerializedArenaLeaderboard()
        {
            leaderboard = leaderboard.ToArray()
        };
        string jsonString = JsonUtility.ToJson(saveObj);

        PlayerPrefs.SetString(PLAYER_PREFS_KEY, jsonString);
    }
}
