using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IslandMapsManager : MonoBehaviour
{
    [SerializeField]
    private IslandData[] islands;

    private List<Islands> discoveredIslands;

    private void Start()
    {
        discoveredIslands = new List<Islands>
        {
            Islands.EUREKA_TRADING_POST
        };
    }

    public void DiscoverIsland(Islands island)
    {
        if (!discoveredIslands.Contains(island))
        {
            discoveredIslands.Add(island);
        }
    }

    public IslandData[] GetDiscoveredIslands()
    {
        List<IslandData> discoveredIslandsData = new List<IslandData>();

        foreach (IslandData islandData in islands)
        {
            if (discoveredIslands.Contains(islandData.island))
            {
                discoveredIslandsData.Add(islandData);
            }
        }
        return discoveredIslandsData.ToArray();
    }

    public IslandData GetIslandInfo(Islands island)
    {
        for (int i = 0; i < islands.Length; i++)
        {
            if (islands[i].island == island)
                return islands[i];
        }
        return null;
    }

    public bool IslandIsDiscovered(Islands island)
    {
        return discoveredIslands.Contains(island);
    }

    public void SetDiscoveredIslands(Islands[] islands)
    {
        discoveredIslands = new List<Islands>(islands);
    }
}

public enum Islands
{
    EUREKA_TRADING_POST, 
    ROCKY_COVE, 
    PALM_ISLE, 
    BANNANA_CAY, 
    CRAB_REEF, 
    BEACON_HILL, 
    CHIP_TOOTH_COVE, 
    CROOKS_COVE, 
    SKULL_ISLE, 
    NONE
}
