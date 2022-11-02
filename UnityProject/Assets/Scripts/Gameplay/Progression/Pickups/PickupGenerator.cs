using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject shipPrefabManager;
    [SerializeField]
    private GameObject inventoryObject;
    [SerializeField]
    private GameObject pickupUIParent;
    [SerializeField]
    private GameObject waveManager;
    [SerializeField]
    private GameObject pickupPrefab;

    public GameObject SpawnPickup(Item item, int amount, Vector3 position)
    {
        if (amount <= 0)
            return null;

        GameObject pickupObject = Instantiate(pickupPrefab, position, GetRandRotation());

        ItemPickup pickup = pickupObject.GetComponent<ItemPickup>();
        pickup.SetParameters(
            shipPrefabManager,
            inventoryObject,
            pickupUIParent,
            item,
            amount);

        BasicBuoyancy buoyancy = pickupObject.GetComponent<BasicBuoyancy>();
        buoyancy.SetWaveManager(waveManager);

        return pickupObject;
    }

    private Quaternion GetRandRotation()
    {
        float xRot = Random.Range(0f, 360f);
        float yRot = Random.Range(0f, 360f);
        float zRot = Random.Range(0f, 360f);

        return Quaternion.Euler(xRot, yRot, zRot);
    }
}
