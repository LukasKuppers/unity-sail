using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureMapPickup : MonoBehaviour, IClickableObject
{
    [SerializeField]
    private GameObject mapTreasureManager;
    [SerializeField]
    private Islands island;

    private MapTreasureManager treasureManager;

    private void Start()
    {
        if (mapTreasureManager != null)
        {
            treasureManager = mapTreasureManager.GetComponent<MapTreasureManager>();
        }
    }

    public void InitParameters(GameObject mapTreasureManager, Islands island)
    {
        this.mapTreasureManager = mapTreasureManager;
        treasureManager = mapTreasureManager.GetComponent<MapTreasureManager>();

        this.island = island;
    }

    public void Interact(string interactionLockKey)
    {
        treasureManager.AddRandomTreasure(island);
        PlayerAttackMode.EnableAttack(interactionLockKey);
        Destroy(gameObject);
    }
}
