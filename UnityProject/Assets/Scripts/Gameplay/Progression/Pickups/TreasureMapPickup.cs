using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureMapPickup : MonoBehaviour, IClickableObject
{
    [SerializeField]
    private GameObject mapTreasureManager;

    private MapTreasureManager treasureManager;

    private void Start()
    {
        if (mapTreasureManager != null)
        {
            treasureManager = mapTreasureManager.GetComponent<MapTreasureManager>();
        }
    }

    public void InitParameters(GameObject mapTreasureManager)
    {
        this.mapTreasureManager = mapTreasureManager;
        treasureManager = mapTreasureManager.GetComponent<MapTreasureManager>();
    }

    public void Interact(string interactionLockKey)
    {
        treasureManager.AddRandomTreasure();
        PlayerAttackMode.EnableAttack(interactionLockKey);
        Destroy(gameObject);
    }
}
