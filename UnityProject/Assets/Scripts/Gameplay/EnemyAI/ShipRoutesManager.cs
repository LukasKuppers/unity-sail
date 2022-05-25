using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipRoutesManager : MonoBehaviour
{
    [SerializeField]
    private GameObject dayNightCycleManager;
    [SerializeField]
    private GameObject enemyShipSpawner;
    [SerializeField]
    private GameObject islandMapsManager;

    private DayNightCycle timeManager;
    private EnemyShipSpawner shipSpawner;
    private IslandMapsManager islandManager;

    private Dictionary<int, AIShipRoute> currentRoutes;
    private Dictionary<int, GameObject> travellingShips;
    private int routeIndexCounter = 0;

    private void Start()
    {
        timeManager = dayNightCycleManager.GetComponent<DayNightCycle>();
        shipSpawner = enemyShipSpawner.GetComponent<EnemyShipSpawner>();
        islandManager = islandMapsManager.GetComponent<IslandMapsManager>();

        currentRoutes = new Dictionary<int, AIShipRoute>();
        travellingShips = new Dictionary<int, GameObject>();
    }

    public List<AIShipRoute> GetShipRoutes()
    {
        List<AIShipRoute> routes = new List<AIShipRoute>();
        foreach (KeyValuePair<int, AIShipRoute> entry in currentRoutes)
        {
            AIShipRoute route = entry.Value;
            if (travellingShips.ContainsKey(entry.Key))
                route.shipPosition = travellingShips[entry.Key].transform.position;

            routes.Add(route);
        }
        return routes;
    }

    public void SetShipRoutes(List<AIShipRoute> routes)
    {
        foreach (AIShipRoute route in routes)
        {
            if (TimeIsPassed(route.startMonth, route.startDay, route.startTime))
            {
                currentRoutes.Add(routeIndexCounter, route);
                SpawnShip(routeIndexCounter, route.destIsland, route.shipPosition);

                routeIndexCounter++;
            }
            else
            {
                AddFutureRoute(route);
            }
        }
    }

    public AIShipRoute CreateRandomRoute()
    {
        Islands start = GetRandomIsland();
        Islands dest = GetRandomIsland();
        while (dest == start)
            dest = GetRandomIsland();

        int startDay = timeManager.GetDay() + Random.Range(1, 4);
        float startTime = Random.Range(0f, 0.99f);

        return CreateRoute(start, dest, startDay, startTime);
    }

    public AIShipRoute CreateRoute(Islands start, Islands dest, int startDay, float startTime)
    {
        int startMonth = timeManager.GetMonth();
        if (startDay < timeManager.GetDay())
            startMonth += 1;
        else if (startDay == timeManager.GetDay() && startTime < timeManager.GetTimePercent())
            return null;

        AIShipRoute newRoute = new AIShipRoute()
        {
            startIsland = start,
            destIsland = dest,
            startMonth = startMonth,
            startDay = startDay,
            startTime = startTime,
            shipPosition = Vector3.zero
        };

        AddFutureRoute(newRoute);
        return newRoute;
    }

    public void CompleteRoute(int id)
    {
        if (!currentRoutes.ContainsKey(id))
        {
            Debug.LogError($"ShipRoutesManager:CompleteRoute: no route with id {id} exists");
            return;
        }

        currentRoutes.Remove(id);
        if (travellingShips.ContainsKey(id))
        {
            Destroy(travellingShips[id]);
            travellingShips.Remove(id);
        }
    }

    private void AddFutureRoute(AIShipRoute route)
    {
        currentRoutes.Add(routeIndexCounter, route);

        timeManager.AddDayTimeListener(route.startDay, route.startTime,
            () => StartRoute(routeIndexCounter));

        routeIndexCounter++;
    }

    private void StartRoute(int routeID)
    {
        AIShipRoute route = currentRoutes[routeID];

        IslandData startIsland = islandManager.GetIslandInfo(route.startIsland);
        Vector3 startPos = startIsland.islandObject.transform.position + new Vector3(100f, 0, 0);
        SpawnShip(routeID, route.destIsland, startPos);
    }

    private void SpawnShip(int routeID, Islands destIsland, Vector3 pos)
    {
        GameObject ship = shipSpawner.SpawnRandomShip(pos);
        ship.GetComponent<EnemyIslandVisitManager>().AddVisitListener(
            destIsland,
            () => CompleteRoute(routeID));

        travellingShips.Add(routeID, ship);
    }

    private bool TimeIsPassed(int month, int day, float time)
    {
        int currentMonth = timeManager.GetMonth();
        int currentDay = timeManager.GetDay();
        float currentTime = timeManager.GetTimePercent();

        if (currentMonth > month)
            return true;
        if (currentMonth == month)
        {
            if (currentDay > day)
                return true;
            if (currentDay == day)
            {
                if (currentTime > time)
                    return true;
            }
        }
        return false;
    }

    private Islands GetRandomIsland()
    {
        Islands island = Islands.NONE;
        System.Array vals = System.Enum.GetValues(typeof(Islands));

        while (island != Islands.NONE && island != Islands.EUREKA_TRADING_POST)
        {
            island = (Islands)vals.GetValue(Random.Range(0, vals.Length));
        }
        return island;
    }
}

public class AIShipRoute
{
    public Islands startIsland;

    public Islands destIsland;

    public int startMonth;

    public int startDay;

    public float startTime;

    public Vector3 shipPosition;
}

