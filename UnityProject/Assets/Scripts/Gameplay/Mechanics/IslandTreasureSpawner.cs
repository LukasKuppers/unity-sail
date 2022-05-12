using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandTreasureSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject islandVisitManager;

    private IslandVisitManager visitManager;

    private void Start()
    {
        visitManager = islandVisitManager.GetComponent<IslandVisitManager>();

        visitManager.AddGeneralVisitListener(PrintVisitedIsland);
    }

    private void PrintVisitedIsland(Islands island)
    {
        string islandName = island.ToString();
        Debug.Log($"Visted island: {islandName}!");
    }
}
