using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InputReference
{
    private static PlayerInputManager inputManager;

    public static PlayerInputManager GetInputManager()
    {
        if (inputManager != null)
        {
            return inputManager;
        }

        inputManager = Object.FindObjectOfType<PlayerInputManager>();
        return inputManager;
    }
}
