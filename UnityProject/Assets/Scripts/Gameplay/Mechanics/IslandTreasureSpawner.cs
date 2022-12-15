using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandTreasureSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject inventoryObject;
    [SerializeField]
    private GameObject mapTreasureManager;
    [SerializeField]
    private GameObject treasurePrefab;
    [SerializeField]
    private GameObject specialTreasurePrefab;
    [SerializeField]
    private GameObject treasureMapPrefab;
    [SerializeField]
    private Islands island;
    [SerializeField]
    private GameObject[] spawnLocations;
    [SerializeField]
    private float treasureSpawnProbability = 1f;
    [SerializeField]
    private float mapSpawnProbability = 0f;

    public void SpawnTreasure()
    {
        if (spawnLocations.Length == 0)
            Debug.LogWarning("IslandTreasureSpawner: SpawnTreasure: no spawn locations assigned");
        foreach (GameObject location in spawnLocations)
        {
            if (location.transform.childCount == 0)
            {
                if (ShouldSpawnRand(treasureSpawnProbability))
                {
                    SpawnTreasure(location);
                }
                else if (ShouldSpawnRand(mapSpawnProbability))
                {
                    SpawnTreasureMap(location);
                }
            }
        }
    }

    public void SpawnSpecialTreasure()
    {
        if (spawnLocations.Length == 0)
        {
            Debug.LogWarning("IslandTreasureSpawner: SpawnSpecialTreaure: " +
                "no spawn locations assigned. Aborting.");
            return;
        }

        GameObject location = spawnLocations[Random.Range(0, spawnLocations.Length)];

        GameObject treasure = Instantiate(specialTreasurePrefab, location.transform.position,
                                          location.transform.rotation, location.transform);
        MapTreasurePickup data = treasure.GetComponent<MapTreasurePickup>();
        data.InitParameters(inventoryObject, mapTreasureManager, island);
    }

    private bool ShouldSpawnRand(float probability)
    {
        float rand = Random.Range(0f, 1f);
        return rand <= probability;
    }

    private void SpawnTreasure(GameObject location)
    {
        GameObject treasure = Instantiate(treasurePrefab, location.transform.position,
                                          location.transform.rotation, location.transform);
        IslandTreasurePickup data = treasure.GetComponent<IslandTreasurePickup>();
        data.SetInventory(inventoryObject);
    }

    private void SpawnTreasureMap(GameObject location)
    {
        GameObject map = Instantiate(treasureMapPrefab, location.transform.position,
                                     location.transform.rotation, location.transform);
        TreasureMapPickup data = map.GetComponent<TreasureMapPickup>();
        data.InitParameters(mapTreasureManager, island);
    }
}
