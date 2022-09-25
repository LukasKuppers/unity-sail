using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyPatrolSpawnLocation
{
    public EnemyType shipType;

    public GameObject shipSpawnLocation;
}

public class EnemyPatrol : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyShipSpawner;
    [SerializeField]
    private string playerTag = "Player";
    [SerializeField]
    private EnemyPatrolSpawnLocation[] spawnLocations;

    private EnemyShipSpawner spawner;

    private List<GameObject> spawnedShips;
    private bool shipsAreLoaded = false;

    private void Start()
    {
        spawner = enemyShipSpawner.GetComponent<EnemyShipSpawner>();
        spawnedShips = new List<GameObject>();
    }

    private void SpawnShips()
    {
        if (!shipsAreLoaded)
        {
            foreach (EnemyPatrolSpawnLocation location in spawnLocations)
            {
                GameObject ship = spawner.SpawnShip(location.shipType, location.shipSpawnLocation.transform.position, AIShipMode.Agressive);
                ship.GetComponent<AIShipController>().SetGoal(location.shipSpawnLocation.transform.position);
                spawnedShips.Add(ship);
            }
            shipsAreLoaded = true;
        }
    }

    private void DestroyShips()
    {
        foreach (GameObject ship in spawnedShips)
        {
            if (ship != null)
                Destroy(ship);
        }
        spawnedShips.Clear();
        shipsAreLoaded = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject colObj = other.gameObject;
        if (colObj.CompareTag(playerTag))
        {
            SpawnShips();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        GameObject colObj = other.gameObject;
        if (colObj.CompareTag(playerTag))
        {
            DestroyShips();
        }
    }
}
