using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SavedTutorialState
{
    private static readonly string PLAYER_PREFS_KEY = "saved_tutorial_state_key";

    public static bool TutorialIsCompleted()
    {
        string serializedTutorialState = PlayerPrefs.GetString(PLAYER_PREFS_KEY);

        if (serializedTutorialState == null || serializedTutorialState == "")
            return false;

        return bool.Parse(serializedTutorialState);
    }

    public static void SetTutorialCompleted()
    {
        bool tutorialAlreadyCompleted = TutorialIsCompleted();

        if (!tutorialAlreadyCompleted)
        {
            string serializedTutorialState = true.ToString();
            PlayerPrefs.SetString(PLAYER_PREFS_KEY, serializedTutorialState);
        }
    }
}
