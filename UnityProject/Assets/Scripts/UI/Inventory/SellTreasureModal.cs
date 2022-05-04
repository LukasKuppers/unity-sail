using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SellTreasureModal : MonoBehaviour
{
    private static readonly string INTERACTION_LOCK_KEY = "Sell_treasure_modal_key";

    [SerializeField]
    private GameObject exitButton;
    [SerializeField]
    private GameObject sellTreasureButton;
    [SerializeField]
    private GameObject treasureAmountText;
    [SerializeField]
    private GameObject resultsObj;
    [SerializeField]
    private GameObject resultsText;

    [SerializeField]
    private int minValue;
    [SerializeField]
    private int maxValue;

    private PlayerInventory inventory;
    private Button exitBtn;
    private Button sellTreasureBtn;
    private TextMeshProUGUI treasureAmount;
    private TextMeshProUGUI results;

    private void Awake()
    {
        exitBtn = exitButton.GetComponent<Button>();
        sellTreasureBtn = sellTreasureButton.GetComponent<Button>();
        treasureAmount = treasureAmountText.GetComponent<TextMeshProUGUI>();
        results = resultsText.GetComponent<TextMeshProUGUI>();

        exitBtn.onClick.AddListener(CloseModal);
        sellTreasureBtn.onClick.AddListener(SellTreasure);

        PlayerSceneInteraction.DisableInteraction(INTERACTION_LOCK_KEY);
    }

    public void SetInventory(GameObject inventoryObject)
    {
        inventory = inventoryObject.GetComponent<PlayerInventory>();

        int amount = inventory.GetTreasureAmount();
        string text = "You have " + amount.ToString() + " treasure on board.";
        treasureAmount.SetText(text);
    }

    private void CloseModal()
    {
        PlayerSceneInteraction.EnableInteraction(INTERACTION_LOCK_KEY);
        Destroy(gameObject);
    }

    private void SellTreasure()
    {
        if (inventory != null)
        {
            int amount = inventory.GetTreasureAmount();
            if (amount > 0)
            {
                int coins = 0;
                for (int i = 0; i < amount; i++)
                {
                    coins += Random.Range(minValue, maxValue);
                }

                inventory.SetTreasureAmount(0);
                inventory.IncrementCoin(coins);

                resultsObj.SetActive(true);
                results.SetText("+" + coins.ToString());
            }
        }
    }
}
