using System.Collections.Generic;
using UnityEngine;
using System;

public static class SavedOptionsManager
{
    private static readonly string PLAYER_PREFS_KEY = "Saved_options_key";

    public static void SaveOptions(Dictionary<string, string> options)
    {
        if (options == null)
        {
            Debug.LogWarning("SavedOptionsManager:SaveOptions: given options are null");
            return;
        }

        PersistenOptionsData newOptions = new PersistenOptionsData
        {
            keys = GetKeyArray(options),
            values = GetValueArray(options)
        };

        string jsonString = JsonUtility.ToJson(newOptions);
        PlayerPrefs.SetString(PLAYER_PREFS_KEY, jsonString);
    }

    public static Dictionary<string, string> LoadOptions()
    {
        string savedJson = PlayerPrefs.GetString(PLAYER_PREFS_KEY);
        if (savedJson == null || savedJson == "")
        {
            return null;
        }

        PersistenOptionsData savedOptions = JsonUtility.FromJson<PersistenOptionsData>(savedJson);
        Dictionary<string, string> options = ConvertArraysToDict(savedOptions.keys, savedOptions.values);
        return options;
    }

    private static Dictionary<string, string> ConvertArraysToDict(string[] keys, string[] values)
    {
        Dictionary<string, string> dict = new Dictionary<string, string>();

        if (keys == null || values == null)
        {
            return dict;
        }

        for (int i = 0; i < keys.Length; i++)
        {
            dict.Add(keys[i], values[i]);
        }

        return dict;
    }

    private static string[] GetKeyArray(Dictionary<string, string> dict)
    {
        List<string> keys = new List<string>();
        foreach (string key in dict.Keys)
            keys.Add(key);

        return keys.ToArray();
    }

    private static string[] GetValueArray(Dictionary<string, string> dict)
    {
        List<string> values = new List<string>();
        foreach (string value in dict.Values)
            values.Add(value);

        return values.ToArray();
    }
}

[Serializable]
public class PersistenOptionsData
{
    public string[] keys;

    public string[] values;
}
