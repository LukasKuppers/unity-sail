using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using SimpleJSON;

public class DiscoveredLocationsLoadModule : MonoBehaviour, ILoadModule
{
    private static readonly string JSON_KEY = "Discovered_locations_data";

    [SerializeField]
    private GameObject islandMapsManager;
    [SerializeField]
    private GameObject navDataManager;

    private IslandMapsManager islandsManager;
    private NavigationDataManager navManager;

    private void Start()
    {
        islandsManager = islandMapsManager.GetComponent<IslandMapsManager>();
        navManager = navDataManager.GetComponent<NavigationDataManager>();
    }

    public string GetJsonKey()
    {
        return JSON_KEY;
    }

    public string GetJsonString()
    {
        IslandData[] discoveredIslands = islandsManager.GetDiscoveredIslands();
        string[] discoveredAsArr = new string[discoveredIslands.Length];
        for (int i = 0; i < discoveredAsArr.Length; i++)
        {
            discoveredAsArr[i] = discoveredIslands[i].island.ToString();
        }

        Islands[] navigatedIslands = navManager.GetNavigatedIslands();
        string[] navigatedAsArr = new string[navigatedIslands.Length];
        for (int i = 0; i < navigatedAsArr.Length; i++)
        {
            navigatedAsArr[i] = navigatedIslands[i].ToString();
        }

        PersistentDiscoveredLocData data = new PersistentDiscoveredLocData()
        {
            discoveredIslands = discoveredAsArr,
            navigatedIslands = navigatedAsArr
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
        PersistentDiscoveredLocData data = JsonUtility.FromJson<PersistentDiscoveredLocData>(objectData);

        if (data == null)
            return;

        // set discovered islands
        Islands[] discoveredIslands = StringArrToIslandsArr(data.discoveredIslands);
        Islands[] navigatedIslands = StringArrToIslandsArr(data.navigatedIslands);

        islandsManager.SetDiscoveredIslands(discoveredIslands);
        navManager.SetNavigatedIslands(navigatedIslands);
    }

    private Islands[] StringArrToIslandsArr(string[] arr)
    {
        Islands[] islandsArr = new Islands[arr.Length];
        for (int i = 0; i < arr.Length; i++)
        {
            Enum.TryParse(arr[i], out Islands island);
            islandsArr[i] = island;
        }
        return islandsArr;
    }
}

[Serializable]
public class PersistentDiscoveredLocData
{
    public string[] discoveredIslands;

    public string[] navigatedIslands;
}
