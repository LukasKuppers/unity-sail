using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeShipModal : MonoBehaviour
{
    private static readonly string INTERACTION_LOCK_KEY = "Upgrade_ship_modal_key";
    private static readonly string NO_UPGRADE_MSG = "You have unlocked the final ship";

    [SerializeField]
    private GameObject exitButton;
    [SerializeField]
    private GameObject upgradeButton;
    [SerializeField]
    private GameObject nextShipText;
    [SerializeField]
    private GameObject shipPriceText;

    private Button exitBtn;
    private Button upgradeBtn;
    private TextMeshProUGUI nextShipT;
    private TextMeshProUGUI shipPriceT;

    private PlayerInventory inventory;
    private ShipPrefabManager prefabManager;
    private string[] shipNames;
    private int[] shipPrices;

    private int currentIndex = 0;

    private void Awake()
    {
        exitBtn = exitButton.GetComponent<Button>();
        upgradeBtn = upgradeButton.GetComponent<Button>();
        nextShipT = nextShipText.GetComponent<TextMeshProUGUI>();
        shipPriceT = shipPriceText.GetComponent<TextMeshProUGUI>();

        exitBtn.onClick.AddListener(CloseModal);
        upgradeBtn.onClick.AddListener(UpgradeShip);

        PlayerSceneInteraction.DisableInteraction(INTERACTION_LOCK_KEY);
    }

    public void InitParameters(GameObject inventoryObject, GameObject prefabManagerObject, 
                               string[] shipNames, int[] shipPrices)
    {
        inventory = inventoryObject.GetComponent<PlayerInventory>();
        prefabManager = prefabManagerObject.GetComponent<ShipPrefabManager>();

        if (shipNames.Length != shipPrices.Length)
            Debug.LogWarning("UpgradeShipModal:Init: Ship names and prices should be of same length");
        this.shipNames = shipNames;
        this.shipPrices = shipPrices;
        currentIndex = prefabManager.GetShipIndex();

        SetText();
    }

    private void UpgradeShip()
    {
        if (currentIndex < shipNames.Length - 1)
        {
            int price = shipPrices[currentIndex + 1];
            if (inventory.GetCoinAmount() >= price)
            {
                prefabManager.SpawnShip(currentIndex + 1);
                inventory.IncrementCoin(-price);
                CloseModal();
            }
            else
                AudioManager.GetInstance().Play(SoundMap.ERROR);
        }    
    }

    private void CloseModal()
    {
        AudioManager.GetInstance().Play(SoundMap.DISCARD_PAGE);

        PlayerSceneInteraction.EnableInteraction(INTERACTION_LOCK_KEY);
        Destroy(gameObject);
    }

    private void SetText()
    {
        if (currentIndex == shipNames.Length - 1)
        {
            nextShipT.SetText(NO_UPGRADE_MSG);
            shipPriceT.SetText("0");
        }
        else
        {
            string nextShip = shipNames[currentIndex + 1];
            int shipPrice = shipPrices[currentIndex + 1];
            nextShipT.SetText($"Next ship to buy: {nextShip}");
            shipPriceT.SetText(shipPrice.ToString());

            if (inventory.GetCoinAmount() < shipPrice)
            {
                shipPriceT.color = Color.red;
            }
        }
    }
}
