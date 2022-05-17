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
    private Islands island;
    [SerializeField]
    private GameObject[] spawnLocations;
    [SerializeField]
    private float spawnProbability = 1f;

    public void SpawnTreasure()
    {
        foreach (GameObject location in spawnLocations)
        {
            if (location.transform.childCount == 0)
            {
                if (ShouldSpawnRand())
                {
                    Spawn(treasurePrefab, location);
                }
            }
        }
    }

    public void SpawnSpecialTreasure()
    {
        GameObject location = spawnLocations[Random.Range(0, spawnLocations.Length)];

        GameObject treasure = Instantiate(specialTreasurePrefab, location.transform.position,
                                          location.transform.rotation, location.transform);
        MapTreasurePickup data = treasure.GetComponent<MapTreasurePickup>();
        data.InitParameters(inventoryObject, mapTreasureManager, island);
    }

    private bool ShouldSpawnRand()
    {
        float rand = Random.Range(0f, 1f);
        return rand <= spawnProbability;
    }

    private void Spawn(GameObject treasurePrefab, GameObject location)
    {
        GameObject treasure = Instantiate(treasurePrefab, location.transform.position,
                                          location.transform.rotation, location.transform);
        IslandTreasurePickup data = treasure.GetComponent<IslandTreasurePickup>();
        data.SetInventory(inventoryObject);
    }
}
