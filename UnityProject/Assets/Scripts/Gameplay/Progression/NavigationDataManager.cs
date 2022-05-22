using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NavigationDataManager : MonoBehaviour
{
    [SerializeField]
    private GameObject shipPrefabManager;
    [SerializeField]
    private GameObject islandVisitManager;
    [SerializeField]
    private GameObject islandMapsManager;

    private ShipPrefabManager shipManager;
    private IslandVisitManager visitManager;
    private IslandMapsManager mapsManager;

    private HashSet<Islands> navigatedIslands;
    private UnityEvent countChangedEvent;

    private void Start()
    {
        navigatedIslands = new HashSet<Islands>();

        shipManager = shipPrefabManager.GetComponent<ShipPrefabManager>();
        visitManager = islandVisitManager.GetComponent<IslandVisitManager>();
        mapsManager = islandMapsManager.GetComponent<IslandMapsManager>();

        shipManager.AddRespawnListener(ResetNavData);
        visitManager.AddGeneralVisitListener(HandleIslandVisit);
    }

    public void AddNavCountChangeListener(UnityAction call)
    {
        if (countChangedEvent == null)
            countChangedEvent = new UnityEvent();

        countChangedEvent.AddListener(call);
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

        if (countChangedEvent != null)
            countChangedEvent.Invoke();
    }

    public void ResetNavData()
    {
        navigatedIslands.Clear();
        if (countChangedEvent != null)
            countChangedEvent.Invoke();
    }

    private void HandleIslandVisit(Islands island)
    {
        if (!mapsManager.IslandIsDiscovered(island))
        {
            navigatedIslands.Add(island);
            if (countChangedEvent != null)
                countChangedEvent.Invoke();
        }
    }
}
