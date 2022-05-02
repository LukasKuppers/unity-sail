using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipTypeManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] shipTypePrefabs;
    [SerializeField]
    private int currentShipIndex = 1;

    public GameObject GetShipPrefab()
    {
        return shipTypePrefabs[currentShipIndex];
    }

    public int GetShipIndex()
    {
        return currentShipIndex;
    }

    public void SetShipType(int shipIndex)
    {
        currentShipIndex = shipIndex;
    }
}
