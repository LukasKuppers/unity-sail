using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerAttackMode
{
    private static bool attackEnabled = true;

    private static List<string> lockKeys = new List<string>();

    public static bool AttackEnabled()
    {
        return attackEnabled;
    }

    public static void DisableAttack(string key)
    {
        if (!lockKeys.Contains(key))
        {
            attackEnabled = false;
            lockKeys.Add(key);
        }
    }

    public static void EnableAttack(string key)
    {
        if (lockKeys.Contains(key))
        {
            lockKeys.Remove(key);
        }
        if (lockKeys.Count == 0)
        {
            attackEnabled = true;
        }
    }
}
