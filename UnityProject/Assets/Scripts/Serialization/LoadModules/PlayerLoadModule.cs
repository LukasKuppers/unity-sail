using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using SimpleJSON;

public class PlayerLoadModule : MonoBehaviour, ILoadModule
{
    private static readonly string JSON_KEY = "PlayerLoadModule_data";

    private IDamageable healthManager;

    private void Start()
    {
        healthManager = gameObject.GetComponent<IDamageable>();
    }

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

        if (data == null)
        {
            return;
        }

        transform.position = data.position;
        transform.rotation = Quaternion.Euler(data.rotation.x, data.rotation.y, data.rotation.z);

        healthManager.SetHealth(data.health);
    }

    public string GetJsonString()
    {
        PlayerData savedData = new PlayerData
        {
            position = transform.position,
            rotation = transform.rotation.eulerAngles,
            health = healthManager.GetHealth()
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

    public float health;
}

