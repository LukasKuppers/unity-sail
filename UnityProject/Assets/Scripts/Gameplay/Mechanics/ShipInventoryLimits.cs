using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipInventoryLimits : MonoBehaviour
{
    [SerializeField]
    private int MaxWeightCapacity = 0;

    public int GetMaximumCapacity()
    {
        return MaxWeightCapacity;
    }
}
