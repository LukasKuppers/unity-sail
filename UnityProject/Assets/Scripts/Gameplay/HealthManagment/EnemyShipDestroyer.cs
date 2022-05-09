using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShipDestroyer : MonoBehaviour, IDestructable
{
    [SerializeField]
    private GameObject destroyedPrefab;
    [SerializeField]
    private int maxFoodDrop = 0;
    [SerializeField]
    private int maxWoodDrop = 0;
    [SerializeField]
    private int maxCannonballDrop = 0;
    [SerializeField]
    private int maxTreasureDrop = 0;

    private PickupGenerator pickupGenerator;

    public void SetPickupGenerator(GameObject pickupGenerator)
    {
        this.pickupGenerator = pickupGenerator.GetComponent<PickupGenerator>();
    }

    public void Destroy()
    {
        Instantiate(destroyedPrefab, transform.position, transform.rotation);

        if (pickupGenerator != null)
        {
            Item item = GetRandItem();
            int quantity = GetRandQuantity(item);
            pickupGenerator.SpawnPickup(item, quantity, transform.position);
        }
        else
        {
            Debug.LogWarning("EnemyShipDestroyer:Destroy: Pickup generator has not been set");
        }

        Destroy(gameObject);
    }

    private Item GetRandItem()
    {
        int randItemIndex = Random.Range(0, 4);
        return (Item) randItemIndex;
    }

    private int GetRandQuantity(Item itemType)
    {
        int maxQuant = 0;
        switch (itemType)
        {
            case Item.FOOD:
                maxQuant = maxFoodDrop;
                break;
            case Item.WOOD:
                maxQuant = maxWoodDrop;
                break;
            case Item.CANNONBALL:
                maxQuant = maxCannonballDrop;
                break;
            case Item.TREASURE:
                maxQuant = maxTreasureDrop;
                break;
        }

        if (maxQuant == 0)
        {
            return 0;
        }
        return Random.Range(1, maxQuant + 1);
    }
}
