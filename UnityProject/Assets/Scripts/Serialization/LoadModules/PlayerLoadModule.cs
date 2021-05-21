using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using SimpleJSON;

public class PlayerLoadModule : MonoBehaviour, ILoadModule
{
    private static readonly string JSON_KEY = "PlayerLoadModule_data";

    public string GetJsonKey()
    {
        return JSON_KEY;
    }

    public void Load(string saveJson)
    {
        JSONNode json = JSON.Parse(saveJson);
        if (json == null)
        {
            return;
        }
        string objectData = json[JSON_KEY].Value;
        PlayerData data = JsonUtility.FromJson<PlayerData>(objectData);

        transform.position = data.position;
        transform.rotation = Quaternion.Euler(data.rotation.x, data.rotation.y, data.rotation.z);
    }

    public string GetJsonString()
    {
        PlayerData savedData = new PlayerData
        {
            position = transform.position,
            rotation = transform.rotation.eulerAngles
        };

        string jsonString = JsonUtility.ToJson(savedData);
        return jsonString;
    }
}

[Serializable]
public class PlayerData
{
    public Vector3 position;

    public Vector3 rotation;
}

