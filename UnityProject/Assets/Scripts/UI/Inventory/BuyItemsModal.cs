using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuyItemsModal : MonoBehaviour
{
    private static readonly string INTERACTION_LOCK_KEY = "Buy_items_modal_key";

    [SerializeField]
    private GameObject exitButton;
    [SerializeField]
    private GameObject buyButton;
    [SerializeField]
    private GameObject freeSpaceText;
    [SerializeField]
    private GameObject coinTotalText;
    [SerializeField]
    private GameObject foodWeightText;
    [SerializeField]
    private GameObject woodWeightText;
    [SerializeField]
    private GameObject cannonballWeightText;
    [SerializeField]
    private GameObject foodPriceText;
    [SerializeField]
    private GameObject woodPriceText;
    [SerializeField]
    private GameObject cannonballPriceText;
    [SerializeField]
    private GameObject foodIncrementor;
    [SerializeField]
    private GameObject woodIncrementor;
    [SerializeField]
    private GameObject cannonballIncrementor;

    private PlayerInventory inventory;
    private int foodPrice;
    private int woodPrice;
    private int cannonballPrice;

    private Button exitBtn;
    private Button buyBtn;
    private TextMeshProUGUI freeSpaceT;
    private TextMeshProUGUI coinTotalT;
    private QuantityIncrementor foodQuant;
    private QuantityIncrementor woodQuant;
    private QuantityIncrementor cannonballQuant;

    private void Awake()
    {
        exitBtn = exitButton.GetComponent<Button>();
        buyBtn = buyButton.GetComponent<Button>();
        freeSpaceT = freeSpaceText.GetComponent<TextMeshProUGUI>();
        coinTotalT = coinTotalText.GetComponent<TextMeshProUGUI>();
        foodQuant = foodIncrementor.GetComponent<QuantityIncrementor>();
        woodQuant = woodIncrementor.GetComponent<QuantityIncrementor>();
        cannonballQuant = cannonballIncrementor.GetComponent<QuantityIncrementor>();

        exitBtn.onClick.AddListener(CloseModal);
        buyBtn.onClick.AddListener(BuyItems);
        foodQuant.AddChangeListener(() => OnIncrement(Item.FOOD));
        woodQuant.AddChangeListener(() => OnIncrement(Item.WOOD));
        cannonballQuant.AddChangeListener(() => OnIncrement(Item.CANNONBALL));

        PlayerSceneInteraction.DisableInteraction(INTERACTION_LOCK_KEY);
    }

    public void InitParameters(GameObject inventoryObject, int foodPrice, int woodPrice, int cannonballPrice)
    {
        this.foodPrice = foodPrice;
        this.woodPrice = woodPrice;
        this.cannonballPrice = cannonballPrice;
        inventory = inventoryObject.GetComponent<PlayerInventory>();

        TextMeshProUGUI foodWeightT = foodWeightText.GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI woodWeightT = woodWeightText.GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI cannonballWeightT = cannonballWeightText.GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI foodPriceT = foodPriceText.GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI woodPriceT = woodPriceText.GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI cannonballPriceT = cannonballPriceText.GetComponent<TextMeshProUGUI>();

        foodWeightT.SetText(inventory.GetFoodWeight().ToString() + "kg");
        woodWeightT.SetText(inventory.GetWoodWieght().ToString() + "kg");
        cannonballWeightT.SetText(inventory.GetCannonballWeight().ToString() + "kg");
        foodPriceT.SetText(foodPrice.ToString());
        woodPriceT.SetText(woodPrice.ToString());
        cannonballPriceT.SetText(cannonballPrice.ToString());

        UpdateFreeSpaceText(inventory.GetFreeCapacity());
    }

    private void OnIncrement(Item itemType)
    {
        int foodAmount = foodQuant.GetQuantity();
        int woodAmount = woodQuant.GetQuantity();
        int cannonballAmount = cannonballQuant.GetQuantity();

        int spaceTaken = (foodAmount * inventory.GetFoodWeight()) +
                         (woodAmount * inventory.GetWoodWieght()) +
                         (cannonballAmount * inventory.GetCannonballWeight());

        if (spaceTaken > inventory.GetFreeCapacity())
        {
            switch (itemType)
            {
                case Item.FOOD:
                    foodQuant.SetQuantityQuiet(foodAmount - 1);
                    break;
                case Item.WOOD:
                    woodQuant.SetQuantityQuiet(woodAmount - 1);
                    break;
                case Item.CANNONBALL:
                    cannonballQuant.SetQuantityQuiet(cannonballAmount - 1);
                    break;
            }
            return;
        }

        int freeSpace = inventory.GetFreeCapacity() - spaceTaken;
        UpdateFreeSpaceText(freeSpace);
        int totalCost = (foodAmount * foodPrice) + (woodAmount * woodPrice) + (cannonballAmount * cannonballPrice);
        coinTotalT.SetText(totalCost.ToString());
        if (totalCost > inventory.GetCoinAmount())
        {
            coinTotalT.color = Color.red;
        }
        else
        {
            coinTotalT.color = Color.white;
        }
    }

    private void BuyItems()
    {
        int foodAmount = foodQuant.GetQuantity();
        int woodAmount = woodQuant.GetQuantity();
        int cannonballAmount = cannonballQuant.GetQuantity();
        int totalCost = (foodAmount * foodPrice) + (woodAmount * woodPrice) + (cannonballAmount * cannonballPrice);

        if (totalCost <= inventory.GetCoinAmount())
        {
            inventory.IncrementFood(foodAmount);
            inventory.IncrementWood(woodAmount);
            inventory.IncrementCannonball(cannonballAmount);
            inventory.IncrementCoin(-totalCost);

            CloseModal();
        }
    }

    private void CloseModal()
    {
        PlayerSceneInteraction.EnableInteraction(INTERACTION_LOCK_KEY);
        Destroy(gameObject);
    }

    private void UpdateFreeSpaceText(int freeSpace)
    {
        freeSpaceT.SetText($"Amount of inventory space remaining after purchase: {freeSpace}kg");
    }
}
