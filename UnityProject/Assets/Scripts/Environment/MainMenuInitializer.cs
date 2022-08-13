using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuInitializer : MonoBehaviour
{
    public static readonly string INTERACTION_LOCK_KEY = "menu_initializer_lock_key";

    void Start()
    {
        PlayerSceneInteraction.DisableInteraction(INTERACTION_LOCK_KEY);
    }
}
