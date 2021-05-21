using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoader : MonoBehaviour
{
    private void Start()
    {
        string loadName = LoadedGame.GetLoadedGame();
        if (loadName != null)
        {
            Debug.Log("Current game: " + loadName);
        }
    }
}
