using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoader : MonoBehaviour
{
    [SerializeField]
    private string menuSceneName;
    [SerializeField]
    private GameObject[] loadModules;

    private void Start()
    {
        LoadScene();
    }

    private void LoadScene()
    {
        string loadName = LoadedGame.GetLoadedGame();
        if (loadName != null)
        {
            string loadJson = GetLoadJson(loadName);
            foreach (GameObject loadObj in loadModules)
            {
                ILoadModule module = loadObj.GetComponent<ILoadModule>();
                module.Load(loadJson);
            }
        }
        else
        {
            SceneManager.LoadScene(menuSceneName, LoadSceneMode.Single);
        }
    }

    private string GetLoadJson(string loadName)
    {
        string json = PlayerPrefs.GetString(loadName, "");
        return json;
    }
}
