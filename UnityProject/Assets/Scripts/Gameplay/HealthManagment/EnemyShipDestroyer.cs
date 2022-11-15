using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

    private ObjectDestroyedEvent destroyedEvent;
    private bool generousDropMode = false;

    public void SetPickupGenerator(GameObject pickupGenerator)
    {
        this.pickupGenerator = pickupGenerator.GetComponent<PickupGenerator>();
    }

    public void AddDestructionListener(UnityAction<GameObject> call)
    {
        if (destroyedEvent == null)
            destroyedEvent = new ObjectDestroyedEvent();

        destroyedEvent.AddListener(call);
    }

    public void DropGenerousResources()
    {
        generousDropMode = true;
    }

    public void Destroy()
    {
        GameObject destroyedShip = Instantiate(destroyedPrefab, transform.position, transform.rotation);

        if (destroyedEvent != null)
            destroyedEvent.Invoke(destroyedShip);

        if (pickupGenerator != null)
        {
            if (!generousDropMode)
            {
                Item item = GetRandItem();
                int quantity = GetRandQuantity(item);
                pickupGenerator.SpawnPickup(item, quantity, transform.position);
            }
            else
                SpawnGenrousResourceDrop();
        }
        else
        {
            Debug.LogWarning("EnemyShipDestroyer:Destroy: Pickup generator has not been set");
        }

        Destroy(gameObject);
    }

    private void SpawnGenrousResourceDrop()
    {
        int foodQuantity = GetRandQuantity(Item.FOOD);
        int woodQuantity = GetRandQuantity(Item.WOOD);
        int cannonballQuantity = GetRandQuantity(Item.CANNONBALL);

        pickupGenerator.SpawnPickup(Item.FOOD, foodQuantity, transform.position + transform.forward * 5);
        pickupGenerator.SpawnPickup(Item.WOOD, woodQuantity, transform.position - transform.forward * 5);
        pickupGenerator.SpawnPickup(Item.CANNONBALL, cannonballQuantity, transform.position);
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
