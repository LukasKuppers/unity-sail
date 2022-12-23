using SimpleJSON;
using System;
using UnityEngine;

public class InventoryLoadModule : MonoBehaviour, ILoadModule
{
    private static readonly string JSON_KEY = "PlayerInventory_data";

    [SerializeField]
    private int initFoodAmount;
    [SerializeField]
    private int initWoodAmount;
    [SerializeField]
    private int initCannonballAmount;

    private PlayerInventory inventory;

    private void Start()
    {
        inventory = gameObject.GetComponent<PlayerInventory>();
    }

    public string GetJsonKey()
    {
        return JSON_KEY;
    }

    public void Load(string saveJson)
    {
        JSONNode json = JSON.Parse(saveJson);
        if (json == null)
        {
            InitInventory();
            return;
        }
        string objectData = json[JSON_KEY].Value;
        InventoryData data = JsonUtility.FromJson<InventoryData>(objectData);
        
        if (data == null)
        {
            InitInventory();
            return;
        }

        inventory.SetFoodAmount(data.foodAmount);
        inventory.SetTreasureAmount(data.treasureAmount);
        inventory.SetWoodAmount(data.woodAmount);
        inventory.SetCannonballAmount(data.cannonballAmount);
        inventory.SetCoinAmount(data.numCoins);
    }

    public string GetJsonString()
    {
        InventoryData savedData = new InventoryData
        {
            foodAmount = inventory.GetFoodAmount(),
            treasureAmount = inventory.GetTreasureAmount(),
            woodAmount = inventory.GetWoodAmount(),
            cannonballAmount = inventory.GetCannonballAmount(),
            numCoins = inventory.GetCoinAmount()
        };

        string jsonString = JsonUtility.ToJson(savedData);
        return jsonString;
    }

    private void InitInventory()
    {
        inventory.SetFoodAmount(initFoodAmount);
        inventory.SetWoodAmount(initWoodAmount);
        inventory.SetCannonballAmount(initCannonballAmount);
    }
}

[Serializable]
public class InventoryData
{
    public int foodAmount;

    public int treasureAmount;

    public int woodAmount;

    public int cannonballAmount;

    public int numCoins;
}
