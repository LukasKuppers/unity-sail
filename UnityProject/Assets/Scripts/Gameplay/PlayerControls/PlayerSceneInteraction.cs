using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerSceneInteraction
{
    private static bool interactionEnabled = true;

    private static List<string> lockKeys = new List<string>();

    public static bool InteractionEnabled()
    {
        return interactionEnabled;
    }

    public static void DisableInteraction(string key)
    {
        if (!lockKeys.Contains(key))
        {
            interactionEnabled = false;
            lockKeys.Add(key);
        }
    }

    public static void EnableInteraction(string key)
    {
        if (lockKeys.Contains(key))
        {
            lockKeys.Remove(key);
        }
        if (lockKeys.Count == 0)
        {
            interactionEnabled = true;
        }
    }
}
