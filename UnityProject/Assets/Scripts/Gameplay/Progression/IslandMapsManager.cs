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
            Islands.EUREKA_TRADING_POST, 
            Islands.ROCKY_COVE, 
            Islands.PALM_ISLE, 
            Islands.BANNANA_CAY, 
            Islands.CRAB_REEF
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
}

public enum Islands
{
    EUREKA_TRADING_POST, 
    ROCKY_COVE, 
    PALM_ISLE, 
    BANNANA_CAY, 
    CRAB_REEF
}
