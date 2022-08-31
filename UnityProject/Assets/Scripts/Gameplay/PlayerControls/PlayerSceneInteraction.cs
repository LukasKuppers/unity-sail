using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class PlayerSceneInteraction
{
    private static List<string> lockKeys = new List<string>();
    private static UnityEvent<bool> changeEvent;

    public static bool InteractionEnabled()
    {
        return lockKeys.Count == 0;
    }

    public static void DisableInteraction(string key)
    {
        if (!lockKeys.Contains(key))
        {
            if (changeEvent != null && InteractionEnabled())
                changeEvent.Invoke(false);

            lockKeys.Add(key);
        }
    }

    public static void EnableInteraction(string key)
    {
        StaticCoroutineRunner.GetInstance().CallNextFrame(() =>
        {
            if (lockKeys.Contains(key))
            {
                lockKeys.Remove(key);

                if (changeEvent != null && InteractionEnabled())
                    changeEvent.Invoke(true);
            }
        });
    }

    public static void AddInteractionChangeListener(UnityAction<bool> call)
    {
        if (changeEvent == null)
            changeEvent = new UnityEvent<bool>();

        changeEvent.AddListener(call);
    }
}
