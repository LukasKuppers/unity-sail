using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationDataManager : MonoBehaviour
{
    [SerializeField]
    private GameObject shipPrefabManager;
    [SerializeField]
    private GameObject islandVisitManager;

    private ShipPrefabManager shipManager;
    private IslandVisitManager visitManager;

    private HashSet<Islands> navigatedIslands;

    private void Start()
    {
        navigatedIslands = new HashSet<Islands>();

        shipManager = shipPrefabManager.GetComponent<ShipPrefabManager>();
        visitManager = islandVisitManager.GetComponent<IslandVisitManager>();

        shipManager.AddRespawnListener(ResetNavData);
        visitManager.AddGeneralVisitListener(HandleIslandVisit);
    }

    public int GetNumNavigatedIslands()
    {
        return navigatedIslands.Count;
    }

    public Islands[] GetNavigatedIslands()
    {
        Islands[] islandsList = new Islands[navigatedIslands.Count];
        int i = 0;
        foreach (Islands island in navigatedIslands)
        {
            islandsList[i] = island;
            i++;
        }
        return islandsList;
    }

    public void SetNavigatedIslands(Islands[] navigatedIslands)
    {
        if (navigatedIslands != null)
            this.navigatedIslands = new HashSet<Islands>(navigatedIslands);
    }

    public void ResetNavData()
    {
        navigatedIslands.Clear();
    }

    private void HandleIslandVisit(Islands island)
    {
        navigatedIslands.Add(island);
    }
}
