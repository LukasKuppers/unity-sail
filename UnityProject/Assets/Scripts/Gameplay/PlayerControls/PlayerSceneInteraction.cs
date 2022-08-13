using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class PlayerSceneInteraction
{
    private static bool interactionEnabled = true;

    private static List<string> lockKeys = new List<string>();
    private static UnityEvent<bool> changeEvent;

    public static bool InteractionEnabled()
    {
        return interactionEnabled;
    }

    public static void DisableInteraction(string key)
    {
        if (!lockKeys.Contains(key))
        {
            if (changeEvent != null && interactionEnabled)
                changeEvent.Invoke(false);

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
            if (changeEvent != null && !interactionEnabled)
                changeEvent.Invoke(true);

            interactionEnabled = true;
        }
    }

    public static void AddInteractionChangeListener(UnityAction<bool> call)
    {
        if (changeEvent == null)
            changeEvent = new UnityEvent<bool>();

        changeEvent.AddListener(call);
    }
}
