using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryCapacity : MonoBehaviour
{
    [SerializeField]
    private int capacity = 100;

    public int GetCapacity()
    {
        return capacity;
    }
}
