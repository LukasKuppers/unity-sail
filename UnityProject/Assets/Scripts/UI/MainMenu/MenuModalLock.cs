using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MenuModalLock
{
    private static bool freeToOpen = true;

    public static bool IsFreeToOpenModal()
    {
        return freeToOpen;
    }

    public static void OpenModal()
    {
        freeToOpen = false;
    }

    public static void CloseModal()
    {
        freeToOpen = true;
    }
}
