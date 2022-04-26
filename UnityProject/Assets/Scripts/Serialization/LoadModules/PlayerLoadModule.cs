using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using SimpleJSON;

public class PlayerLoadModule : MonoBehaviour, ILoadModule
{
    private static readonly string JSON_KEY = "PlayerLoadModule_data";

    [SerializeField]
    private GameObject shipPrefabManagerObject;

    private ShipPrefabManager shipPrefabManager;

    private void Start()
    {
        shipPrefabManager = shipPrefabManagerObject.GetComponent<ShipPrefabManager>();
    }

    public string GetJsonKey()
    {
        return JSON_KEY;
    }

    public void Load(string saveJson)
    {
        JSONNode json = JSON.Parse(saveJson);
        if (json == null || saveJson == null || saveJson == "")
        {
            InitNewGame();
            return;
        }
        string objectData = json[JSON_KEY].Value;
        PlayerData data = JsonUtility.FromJson<PlayerData>(objectData);

        if (data == null)
        {
            InitNewGame();
            return;
        }

        int shipIndex = data.shipIndex;
        GameObject loadedShip = shipPrefabManager.SpawnShip(shipIndex);
        loadedShip.transform.position = data.position;
        loadedShip.transform.rotation = Quaternion.Euler(data.rotation.x, data.rotation.y, data.rotation.z);

        IDamageable healthManager = loadedShip.GetComponent<IDamageable>();
        healthManager.SetHealth(data.health);
    }

    public string GetJsonString()
    {
        GameObject currentShip = shipPrefabManager.GetCurrentShip();
        IDamageable healthManager = currentShip.GetComponent<IDamageable>();

        PlayerData savedData = new PlayerData
        {
            shipIndex = shipPrefabManager.GetShipIndex(), 
            position = currentShip.transform.position,
            rotation = currentShip.transform.rotation.eulerAngles,
            health = healthManager.GetHealth()
        };

        string jsonString = JsonUtility.ToJson(savedData);
        return jsonString;
    }

    private void InitNewGame()
    {
        GameObject loadedShip = shipPrefabManager.SpawnShip(0);
        IDamageable healthManager = loadedShip.GetComponent<IDamageable>();
        healthManager.ResetHealth();
    }
}

[Serializable]
public class PlayerData
{
    public int shipIndex;

    public Vector3 position;

    public Vector3 rotation;

    public float health;
}

