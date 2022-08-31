using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class PlayerAttackMode
{
    private static List<string> lockKeys = new List<string>();

    public static bool AttackEnabled()
    {
        return lockKeys.Count == 0;
    }

    public static void DisableAttack(string key)
    {
        if (!lockKeys.Contains(key))
            lockKeys.Add(key);
    }

    public static void EnableAttack(string key)
    {
        StaticCoroutineRunner.GetInstance().CallNextFrame(() =>
        {
            if (lockKeys.Contains(key))
                lockKeys.Remove(key);
        });
    }
}
