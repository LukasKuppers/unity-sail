using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DropItemsModal : MonoBehaviour
{
    private static readonly string INTERACTION_LOCK_KEY = "Drop_items_modal_key";

    [SerializeField]
    private GameObject exitButton;
    [SerializeField]
    private GameObject dropItemsButton;
    [SerializeField]
    private GameObject foodWeightText;
    [SerializeField]
    private GameObject woodWeightText;
    [SerializeField]
    private GameObject cannonballWeightText;
    [SerializeField]
    private GameObject treasureWeightText;
    [SerializeField]
    private GameObject freeSpaceText;

    [SerializeField]
    private GameObject foodSelector;
    [SerializeField]
    private GameObject woodSelector;
    [SerializeField]
    private GameObject cannonballSelector;
    [SerializeField]
    private GameObject treasureSelector;

    private PickupGenerator pickupGenerator;
    private PlayerInventory inventory;
    private ShipPrefabManager shipManager;
    private Button exitBtn;
    private Button dropItemsBtn;
    private TextMeshProUGUI spaceTxt;
    private ISelectableQuantity food;
    private ISelectableQuantity wood;
    private ISelectableQuantity cannonball;
    private ISelectableQuantity treasure;

    private int foodWeight;
    private int woodWeight;
    private int cannonballWeight;
    private int treasureWeight;

    private void Awake()
    {
        exitBtn = exitButton.GetComponent<Button>();
        dropItemsBtn = dropItemsButton.GetComponent<Button>();
        spaceTxt = freeSpaceText.GetComponent<TextMeshProUGUI>();

        food = foodSelector.GetComponent<ISelectableQuantity>();
        wood = woodSelector.GetComponent<ISelectableQuantity>();
        cannonball = cannonballSelector.GetComponent<ISelectableQuantity>();
        treasure = treasureSelector.GetComponent<ISelectableQuantity>();

        exitBtn.onClick.AddListener(ExitModal);
        dropItemsBtn.onClick.AddListener(DropItems);

        PlayerSceneInteraction.DisableInteraction(INTERACTION_LOCK_KEY);
        PlayerAttackMode.DisableAttack(INTERACTION_LOCK_KEY);
    }

    public void InitParameters(GameObject pickupSpawner, GameObject inventoryObject, GameObject prefabManager)
    {
        pickupGenerator = pickupSpawner.GetComponent<PickupGenerator>();
        inventory = inventoryObject.GetComponent<PlayerInventory>();
        shipManager = prefabManager.GetComponent<ShipPrefabManager>();

        foodWeight = inventory.GetFoodWeight();
        woodWeight = inventory.GetWoodWieght();
        cannonballWeight = inventory.GetCannonballWeight();
        treasureWeight = inventory.GetTreasureWeight();

        foodWeightText.GetComponent<TextMeshProUGUI>().SetText(foodWeight.ToString() + " kg");
        woodWeightText.GetComponent<TextMeshProUGUI>().SetText(woodWeight.ToString() + " kg");
        cannonballWeightText.GetComponent<TextMeshProUGUI>().SetText(cannonballWeight.ToString() + " kg");
        treasureWeightText.GetComponent<TextMeshProUGUI>().SetText(treasureWeight.ToString() + " kg");

        food.SetLimits(0, inventory.GetFoodAmount());
        wood.SetLimits(0, inventory.GetWoodAmount());
        cannonball.SetLimits(0, inventory.GetCannonballAmount());
        treasure.SetLimits(0, inventory.GetTreasureAmount());
        food.AddChangeListener(UpdateFreeSpaceText);
        wood.AddChangeListener(UpdateFreeSpaceText);
        cannonball.AddChangeListener(UpdateFreeSpaceText);
        treasure.AddChangeListener(UpdateFreeSpaceText);

        UpdateFreeSpaceText();
    }

    private void ExitModal()
    {
        PlayerSceneInteraction.EnableInteraction(INTERACTION_LOCK_KEY);
        PlayerAttackMode.EnableAttack(INTERACTION_LOCK_KEY);
        Destroy(gameObject);
    }

    private void DropItems()
    {
        DropItem(Item.FOOD, food.GetQuantity());
        DropItem(Item.WOOD, wood.GetQuantity());
        DropItem(Item.CANNONBALL, cannonball.GetQuantity());
        DropItem(Item.TREASURE, treasure.GetQuantity());

        ExitModal();
    }

    private void UpdateFreeSpaceText()
    {
        int foodDropW = food.GetQuantity() * foodWeight;
        int woodDropW = wood.GetQuantity() * woodWeight;
        int cannonballDropW = cannonball.GetQuantity() * cannonballWeight;
        int treasureDropW = treasure.GetQuantity() * treasureWeight;

        int extraSpace = foodDropW + woodDropW + cannonballDropW + treasureDropW;
        int freeSpaceTot = inventory.GetFreeCapacity() + extraSpace;

        string text = "Free inventory space: " + freeSpaceTot.ToString() + " kg";
        spaceTxt.SetText(text);
    }

    private void DropItem(Item itemType, int amount)
    {
        if (amount > 0)
        {
            GameObject ship = shipManager.GetCurrentShip();
            Vector3 shipDir = ship.transform.forward;
            Vector3 randOffset = new Vector3(Random.Range(-5f, 5f), 0, Random.Range(-5f, 5f));
            Vector3 dropPos = ship.transform.position - (shipDir * 20f) + randOffset;

            switch (itemType)
            {
                case Item.FOOD:
                    inventory.IncrementFood(-amount);
                    break;
                case Item.WOOD:
                    inventory.IncrementWood(-amount);
                    break;
                case Item.CANNONBALL:
                    inventory.IncrementCannonball(-amount);
                    break;
                case Item.TREASURE:
                    inventory.IncrementTreasure(-amount);
                    break;
            }
            pickupGenerator.SpawnPickup(itemType, amount, dropPos);
        }
    }
}
