using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyShipSpawner;
    [SerializeField]
    private string playerTag = "Player";
    [SerializeField]
    private GameObject[] shipSpawnLocations;

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
            foreach (GameObject location in shipSpawnLocations)
            {
                GameObject ship = spawner.SpawnRandomShip(location.transform.position, AIShipMode.Agressive);
                ship.GetComponent<AIShipController>().SetGoal(location.transform.position);
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
