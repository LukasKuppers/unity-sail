using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipSafetyManager : MonoBehaviour
{
    private bool shipIsSafe = true;
    private bool locked = false;
    private string lockKey = "";

    public bool ShipIsSafe()
    {
        return shipIsSafe;
    }

    public void SetShipSafety(bool isSafe)
    {
        if (!locked)
        {
            shipIsSafe = isSafe;
        }
    }

    public void LockShipSafety(bool isSafe, string key)
    {
        if (!locked)
        {
            shipIsSafe = isSafe;
            locked = true;
            lockKey = key;
        }
    }

    public void UnlockShipSafety(string key)
    {
        if (key == lockKey)
        {
            locked = false;
        }
    }
}
