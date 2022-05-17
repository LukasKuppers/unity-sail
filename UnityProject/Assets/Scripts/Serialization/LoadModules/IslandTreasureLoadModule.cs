using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using SimpleJSON;

public class IslandTreasureLoadModule : MonoBehaviour, ILoadModule
{
    private static readonly string JSON_KEY = "Island_treasure_spawn_queue";

    [SerializeField]
    private GameObject islandTreasureSpawnManager;

    private IslandTreasureSpawnManager spawnManager;

    private void Start()
    {
        spawnManager = islandTreasureSpawnManager.GetComponent<IslandTreasureSpawnManager>();
    }

    public string GetJsonKey()
    {
        return JSON_KEY;
    }

    public string GetJsonString()
    {
        Queue<Islands> recentVisits = spawnManager.GetRecentVisits();
        Islands[] queueAsArr = recentVisits.ToArray();

        string[] stringArr = new string[queueAsArr.Length];
        for (int i = 0; i < stringArr.Length; i++)
        {
            stringArr[i] = queueAsArr[i].ToString();
        }

        PersistentIslandTreasureData data = new PersistentIslandTreasureData()
        {
            recentVisitQueue = stringArr
        };

        string jsonString = JsonUtility.ToJson(data);
        return jsonString;
    }

    public void Load(string saveJson)
    {
        JSONNode json = JSON.Parse(saveJson);
        if (json == null)
            return;

        string objectData = json[JSON_KEY].Value;
        PersistentIslandTreasureData data = JsonUtility.FromJson<PersistentIslandTreasureData>(objectData);

        if (data == null || data.recentVisitQueue == null)
            return;

        // convert string arr to queue of Islands
        string[] stringArr = data.recentVisitQueue;
        Islands[] islandsArr = new Islands[stringArr.Length];
        for (int i = 0; i < islandsArr.Length; i++)
        {
            Enum.TryParse(stringArr[i], out Islands island);
            islandsArr[i] = island;
        }

        spawnManager.SetRecentVisits(new Queue<Islands>(islandsArr));
    }
}

[Serializable]
public class PersistentIslandTreasureData
{
    public string[] recentVisitQueue;
}
