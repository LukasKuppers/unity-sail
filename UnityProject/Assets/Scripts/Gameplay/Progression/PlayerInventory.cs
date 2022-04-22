using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
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

    public int GetFoodAmount() { return foodAmount; }
    public int GetWoodAmount() { return woodAmount; }
    public int GetTreasureAmount() { return treasureAmount; }
    public int GetCannonballAmount() { return cannonballAmount; }
    public int GetCoinAmount() { return coinAmount; }

    public void SetFoodAmount(int amount) { foodAmount = amount; }
    public void SetWoodAmount(int amount) { woodAmount = amount; }
    public void SetTreasureAmount(int amount) { treasureAmount = amount; }
    public void SetCannonballAmount(int amount) { cannonballAmount = amount; }
    public void SetCoinAmount(int amount) { coinAmount = amount; }

    public void IncrementFood(int amount)
    {
        foodAmount = Mathf.Max(0, foodAmount + amount);
    }

    public void IncrementWood(int amount)
    {
        woodAmount = Mathf.Max(0, woodAmount + amount);
    }

    public void IncrementTreasure(int amount)
    {
        treasureAmount = Mathf.Max(0, treasureAmount + amount);
    }

    public void IncrementCannonball(int amount)
    {
        cannonballAmount = Mathf.Max(0, cannonballAmount + amount);
    }

    public void IncrementCoin(int amount)
    {
        coinAmount = Mathf.Max(0, coinAmount + amount);
    }
}
