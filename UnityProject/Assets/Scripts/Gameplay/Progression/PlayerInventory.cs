using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField]
    private GameObject shipPrefabManager;
    [SerializeField]
    private int foodAmount = 0;
    [SerializeField]
    private int woodAmount = 0;
    [SerializeField]
    private int treasureAmount = 0;
    [SerializeField]
    private int cannonballAmount = 0;
    [SerializeField]
    private int coinAmount = 0;

    [SerializeField]
    private int foodWeight = 1;
    [SerializeField]
    private int woodWeight = 1;
    [SerializeField]
    private int treasureWeight = 1;
    [SerializeField]
    private int cannonballWeight = 1;

    private ShipPrefabManager shipManager;
    private int maxCapacity = 100;

    private void Start()
    {
        shipManager = shipPrefabManager.GetComponent<ShipPrefabManager>();
        shipManager.AddSpawnListener(UpdateMaxCapacity);
    }

    private void UpdateMaxCapacity()
    {
        GameObject currentShip = shipManager.GetCurrentShip();
        InventoryCapacity capacityManager = currentShip.GetComponent<InventoryCapacity>();
        maxCapacity = capacityManager.GetCapacity();
    }

    public int GetFoodAmount() { return foodAmount; }
    public int GetWoodAmount() { return woodAmount; }
    public int GetTreasureAmount() { return treasureAmount; }
    public int GetCannonballAmount() { return cannonballAmount; }
    public int GetCoinAmount() { return coinAmount; }

    public int GetFoodWeight() { return foodWeight; }
    public int GetWoodWieght() { return woodWeight; }
    public int GetTreasureWeight() { return treasureWeight; }
    public int GetCannonballWeight() { return cannonballWeight; }

    public void SetFoodAmount(int amount) { foodAmount = amount; }
    public void SetWoodAmount(int amount) { woodAmount = amount; }
    public void SetTreasureAmount(int amount) { treasureAmount = amount; }
    public void SetCannonballAmount(int amount) { cannonballAmount = amount; }
    public void SetCoinAmount(int amount) { coinAmount = amount; }

    public int IncrementFood(int amount)
    {
        int maxInc = GetFreeCapacity() / foodWeight;
        int trueInc = Mathf.Clamp(amount, -foodAmount, maxInc);
        foodAmount += trueInc;
        return trueInc;
    }

    public int IncrementWood(int amount)
    {
        int maxInc = GetFreeCapacity() / woodWeight;
        int trueInc = Mathf.Clamp(amount, -woodAmount, maxInc);
        woodAmount += trueInc;
        return trueInc;
    }

    public int IncrementTreasure(int amount)
    {
        int maxInc = GetFreeCapacity() / treasureWeight;
        int trueInc = Mathf.Clamp(amount, -treasureAmount, maxInc);
        treasureAmount += trueInc;
        return trueInc;
    }

    public int IncrementCannonball(int amount)
    {
        int maxInc = GetFreeCapacity() / cannonballWeight;
        int trueInc = Mathf.Clamp(amount, -cannonballAmount, maxInc);
        cannonballAmount += trueInc;
        return trueInc;
    }

    public void IncrementCoin(int amount)
    {
        coinAmount = Mathf.Max(0, coinAmount + amount);
    }

    private int GetFreeCapacity()
    {
        int foodTotal = foodAmount * foodWeight;
        int woodtotal = woodAmount * woodWeight;
        int treasureTotal = treasureAmount * treasureWeight;
        int cannonballTotal = cannonballAmount * cannonballWeight;

        int totalWeight = foodTotal + woodtotal + treasureTotal + cannonballTotal;
        int freeWeight = maxCapacity - totalWeight;
        
        if (freeWeight < 0)
        {
            Debug.LogError("PlayerInventory:State Invalid: total weight exceeds max capacity");
        }

        return freeWeight;
    }
}
