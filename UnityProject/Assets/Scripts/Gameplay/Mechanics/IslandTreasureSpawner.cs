using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandTreasureSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject inventoryObject;
    [SerializeField]
    private GameObject treasurePrefab;
    [SerializeField]
    private GameObject[] spawnLocations;
    [SerializeField]
    private float spawnProbability = 1f;

    public void SpawnTreasure()
    {
        foreach (GameObject location in spawnLocations)
        {
            if (ShouldSpawnRand())
            {
                Spawn(location);
            }
        }
    }

    private bool ShouldSpawnRand()
    {
        float rand = Random.Range(0f, 1f);
        return rand <= spawnProbability;
    }

    private void Spawn(GameObject location)
    {
        GameObject treasure = Instantiate(treasurePrefab, location.transform.position,
                                          location.transform.rotation, location.transform);
        IslandTreasurePickup data = treasure.GetComponent<IslandTreasurePickup>();
        data.SetInventory(inventoryObject);
    }
}
