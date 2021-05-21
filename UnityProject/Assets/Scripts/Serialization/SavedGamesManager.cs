using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SavedGamesManager
{
    private static readonly string SAVED_GAMES_KEY = "ProjSail_savedGames_names";

    public static string[] GetSavedGames()
    {
        SavedGamesNames saves = LoadSavesData();
        if (saves == null)
        {
            return new string[0];
        }

        return saves.saveNames;
    }

    public static bool CreateGame(string name)
    {
        if (name == null || name.Equals(""))
        {
            Debug.LogError("SavedGamesManager: Cannot create a new game with null or empty name");
            return false;
        }

        SavedGamesNames saves = LoadSavesData();
        if (saves == null)
        {
            saves = new SavedGamesNames()
            { 
                saveNames = new string[] { name }
            };
        }
        else
        {
            foreach (string existingName in saves.saveNames)
            {
                if (name.Equals(existingName))
                {
                    return false;
                }
            }
            string[] newSaves = new string[saves.saveNames.Length + 1];
            for (int i = 0; i < saves.saveNames.Length; i++)
            {
                newSaves[i] = saves.saveNames[i];
            }
            newSaves[newSaves.Length - 1] = name;
            saves.saveNames = newSaves;
        }

        string json = JsonUtility.ToJson(saves);
        PlayerPrefs.SetString(SAVED_GAMES_KEY, json);
        return true;
    }

    public static void RemoveGame(string name)
    {
        if (name == null || name.Equals(""))
        {
            Debug.LogError("SavedGamesManager: Cannot remove a game with null or empty name");
            return;
        }

        SavedGamesNames saves = LoadSavesData();
        string[] newSaves = new string[saves.saveNames.Length - 1];
        int i = 0;
        foreach (string save in saves.saveNames)
        {
            if (!save.Equals(name))
            {
                newSaves[i] = save;
                i++;
            }
        }

        saves.saveNames = newSaves;
        string json = JsonUtility.ToJson(saves);
        PlayerPrefs.SetString(SAVED_GAMES_KEY, json);

        // delete data for saved game
        PlayerPrefs.DeleteKey(name);
    }

    private static SavedGamesNames LoadSavesData()
    {
        string savedGamesJson = PlayerPrefs.GetString(SAVED_GAMES_KEY, "");

        SavedGamesNames savedNames;
        if (savedGamesJson.Equals(""))
        {
            return null;
        }

        savedNames = JsonUtility.FromJson<SavedGamesNames>(savedGamesJson);
        return savedNames;
    }
}
