using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LoadedGame
{
    private static string loadedGameName;

    public static void SetLoadedGame(string name)
    {
        if (name != null && !name.Equals(""))
        {
            loadedGameName = name;
        }
    }

    public static string GetLoadedGame()
    {
        return loadedGameName;
    }
}
