using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using SimpleJSON;

public class GameLoader : MonoBehaviour
{
    [SerializeField]
    private string menuSceneName;
    [SerializeField]
    private GameObject[] loadModules;

    private ILoadModule[] modules;

    private void Start()
    {
        modules = new ILoadModule[loadModules.Length];
        for (int i = 0; i < loadModules.Length; i++)
        {
            modules[i] = loadModules[i].GetComponent<ILoadModule>();
        }

        LoadScene();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            SaveScene();
        }
    }

    private void LoadScene()
    {
        string loadName = LoadedGame.GetLoadedGame();
        if (loadName != null)
        {
            string loadJson = GetLoadJson(loadName);
            foreach (ILoadModule module in modules)
            {
                module.Load(loadJson);
            }
        }
        else
        {
            SceneManager.LoadScene(menuSceneName, LoadSceneMode.Single);
        }
    }

    public void SaveScene()
    {
        string loadName = LoadedGame.GetLoadedGame();
        if (loadName != null)
        {
            JSONObject savedJson = new JSONObject();
            foreach (ILoadModule module in modules)
            {
                string objectJson = module.GetJsonString();
                savedJson.Add(module.GetJsonKey(), objectJson);
            }

            PlayerPrefs.SetString(loadName, savedJson.ToString());
        }
    }

    private string GetLoadJson(string loadName)
    {
        string json = PlayerPrefs.GetString(loadName, "");
        return json;
    }
}
