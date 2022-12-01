using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTreasureManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] islandTreasureSpawners;
    [SerializeField]
    private Islands[] islandsMapping;

    private HashSet<Islands> islandsWithTreasure;
    private Dictionary<Islands, IslandTreasureSpawner> islandSpawnMap;

    private void Start()
    {
        islandsWithTreasure = new HashSet<Islands>();

        islandSpawnMap = new Dictionary<Islands, IslandTreasureSpawner>();
        for (int i = 0; i < islandTreasureSpawners.Length; i++)
        {
            IslandTreasureSpawner spawner = islandTreasureSpawners[i].GetComponent<IslandTreasureSpawner>();
            islandSpawnMap.Add(islandsMapping[i], spawner);
        }
    }

    public void SetIslandsWithTreasure(Islands[] islands)
    {
        if (islands != null)
        {
            islandsWithTreasure.Clear();
            foreach (Islands island in islands)
            {
                islandsWithTreasure.Add(island);
                islandSpawnMap[island].SpawnSpecialTreasure();
            }
        }
    }

    public HashSet<Islands> GetIslandsWithTreasure()
    {
        return islandsWithTreasure;
    }

    public void AddTreasure(Islands island)
    {
        if (!islandsWithTreasure.Contains(island))
        {
            islandsWithTreasure.Add(island);
            islandSpawnMap[island].SpawnSpecialTreasure();
        }
    }

    public void AddRandomTreasure(Islands excludeIsland)
    {
        bool foundLocation = false;
        var enumValues = System.Enum.GetValues(typeof(Islands));
        List<Islands> validIslands = new List<Islands>(islandsMapping);
        while (!foundLocation)
        {
            Islands randIsland = (Islands)enumValues.GetValue(Random.Range(0, enumValues.Length));
            if (!islandsWithTreasure.Contains(randIsland) &&
                validIslands.Contains(randIsland) &&
                randIsland != excludeIsland)
            {
                foundLocation = true;
                AddTreasure(randIsland);
            }
        }
    }

    public void RegisterRemovedTreasure(Islands island)
    {
        if (islandsWithTreasure.Contains(island))
            islandsWithTreasure.Remove(island);
    }
}
