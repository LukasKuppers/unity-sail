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
    [SerializeField]
    private GameObject mapTreasureManager;

    private IslandTreasureSpawnManager spawnManager;
    private MapTreasureManager mapSpawnManager;

    private void Start()
    {
        spawnManager = islandTreasureSpawnManager.GetComponent<IslandTreasureSpawnManager>();
        mapSpawnManager = mapTreasureManager.GetComponent<MapTreasureManager>();
    }

    public string GetJsonKey()
    {
        return JSON_KEY;
    }

    public string GetJsonString()
    {
        Queue<Islands> recentVisits = spawnManager.GetRecentVisits();
        HashSet<Islands> mapLocations = mapSpawnManager.GetIslandsWithTreasure();

        string[] visitsAsArr = IslandQueueToStringArr(recentVisits);
        string[] locationsArr = IslandSetToStringArr(mapLocations);

        PersistentIslandTreasureData data = new PersistentIslandTreasureData()
        {
            recentVisitQueue = visitsAsArr, 
            mapTreasureSet = locationsArr
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

        if (data == null || data.recentVisitQueue == null || data.mapTreasureSet == null)
            return;

        Queue<Islands> islandsQueue = StringArrToIslandQueue(data.recentVisitQueue);
        Islands[] islandsSet = StringArrToIslandArr(data.mapTreasureSet);

        spawnManager.SetRecentVisits(islandsQueue);
        mapSpawnManager.SetIslandsWithTreasure(islandsSet);
    }

    private string[] IslandQueueToStringArr(Queue<Islands> queue)
    {
        Islands[] queueAsArr = queue.ToArray();

        string[] stringArr = new string[queueAsArr.Length];
        for (int i = 0; i < stringArr.Length; i++)
        {
            stringArr[i] = queueAsArr[i].ToString();
        }
        return stringArr;
    }

    private string[] IslandSetToStringArr(HashSet<Islands> set)
    {
        List<string> stringLst = new List<string>();
        foreach (Islands island in set)
        {
            stringLst.Add(island.ToString());
        }
        return stringLst.ToArray();
    }

    private Queue<Islands> StringArrToIslandQueue(string[] arr)
    {
        Islands[] islandsArr = StringArrToIslandArr(arr);
        return new Queue<Islands>(islandsArr);
    }

    private Islands[] StringArrToIslandArr(string[] arr)
    {
        Islands[] islandsArr = new Islands[arr.Length];
        for (int i = 0; i < islandsArr.Length; i++)
        {
            Enum.TryParse(arr[i], out Islands island);
            islandsArr[i] = island;
        }
        return islandsArr;
    }
}

[Serializable]
public class PersistentIslandTreasureData
{
    public string[] recentVisitQueue;

    public string[] mapTreasureSet;
}
