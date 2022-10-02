using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class IslandTreasureSpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject islandVisitManager;
    [SerializeField]
    private int islandQueueSize = 5;
    [SerializeField]
    private GameObject[] islandsTreasureManagers;
    [SerializeField]
    private Islands[] islandsMapping;

    private IslandVisitManager visitManager;
    private Queue<Islands> recentVisits;
    private Dictionary<Islands, IslandTreasureSpawner> islandSpawnerMap;

    private void Start()
    {
        visitManager = islandVisitManager.GetComponent<IslandVisitManager>();
        visitManager.AddGeneralVisitListener(ProcessNewVisit);

        recentVisits = new Queue<Islands>();
        for (int i = 0; i < islandQueueSize; i++)
        {
            recentVisits.Enqueue(Islands.NONE);
        }

        islandSpawnerMap = new Dictionary<Islands, IslandTreasureSpawner>();
        for (int i = 0; i < islandsMapping.Length; i++)
        {
            IslandTreasureSpawner spawner = islandsTreasureManagers[i].GetComponent<IslandTreasureSpawner>();
            islandSpawnerMap.Add(islandsMapping[i], spawner);

        }
    }

    private void ProcessNewVisit(Islands island)
    {
        bool islandIsValid = islandsMapping.Contains(island) && island != Islands.EUREKA_TRADING_POST;
        if (!recentVisits.Contains(island) && islandIsValid) 
        {
            recentVisits.Dequeue();
            recentVisits.Enqueue(island);

            islandSpawnerMap[island].SpawnTreasure();
        }
    }

    public Queue<Islands> GetRecentVisits()
    {
        return recentVisits;
    }

    public void SetRecentVisits(Queue<Islands> queue)
    {
        if (queue != null)
            recentVisits = queue;
    }
}
