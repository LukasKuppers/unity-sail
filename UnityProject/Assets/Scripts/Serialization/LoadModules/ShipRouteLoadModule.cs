using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using SimpleJSON;

public class ShipRouteLoadModule : MonoBehaviour, ILoadModule
{
    private static readonly string JSON_KEY = "AI_Ship_routes_data";

    [SerializeField]
    private GameObject shipRoutesManager;

    private ShipRoutesManager routesManager;

    private void Start()
    {
        routesManager = shipRoutesManager.GetComponent<ShipRoutesManager>();
    }

    public string GetJsonKey()
    {
        return JSON_KEY;
    }

    public string GetJsonString()
    {
        List<AIShipRoute> routes = routesManager.GetShipRoutes();
        PersistentRouteData[] formattedRoutes = new PersistentRouteData[routes.Count];

        int index = 0;
        foreach (AIShipRoute route in routes)
        {
            PersistentRouteData routeData = new PersistentRouteData()
            {
                startIsland = route.startIsland.ToString(),
                destIsland = route.destIsland.ToString(),
                startMonth = route.startMonth,
                startDay = route.startDay,
                startTime = route.startTime,
                shipType = route.shipType.ToString(), 
                position = route.shipPosition
            };
            formattedRoutes[index] = routeData;
            index++;
        }

        PersistentRoutesData data = new PersistentRoutesData() { routes = formattedRoutes };

        string jsonString = JsonUtility.ToJson(data);
        return jsonString;
    }

    public void Load(string saveJson)
    {
        JSONNode json = JSON.Parse(saveJson);
        if (json == null)
        {
            return;
        }
        string objectData = json[JSON_KEY].Value;
        PersistentRoutesData data = JsonUtility.FromJson<PersistentRoutesData>(objectData);

        if (data == null || data.routes == null)
        {
            return;
        }

        List<AIShipRoute> routes = new List<AIShipRoute>();
        foreach (PersistentRouteData route in data.routes)
        {
            Enum.TryParse(route.startIsland, out Islands start);
            Enum.TryParse(route.destIsland, out Islands dest);
            Enum.TryParse(route.shipType, out EnemyType type);

            AIShipRoute loadedRoute = new AIShipRoute()
            {
                startIsland = start,
                destIsland = dest,
                startMonth = route.startMonth,
                startDay = route.startDay,
                startTime = route.startTime,
                shipType = type, 
                shipPosition = route.position
            };
            routes.Add(loadedRoute);
        }

        routesManager.SetShipRoutes(routes);
    }
}

[Serializable]
public class PersistentRoutesData
{
    public PersistentRouteData[] routes;
}

[Serializable]
public class PersistentRouteData
{
    public string startIsland;

    public string destIsland;

    public int startMonth;

    public int startDay;

    public float startTime;

    public string shipType;

    public Vector3 position;
}
