using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CartographerModal : MonoBehaviour
{
    private static readonly string INTERACTION_LOCK_KEY = "Cartographer_modal_key";

    [SerializeField]
    private GameObject exitButton;
    [SerializeField]
    private GameObject sellButton;
    [SerializeField]
    private GameObject contentText;
    [SerializeField]
    private GameObject resultsContainer;
    [SerializeField]
    private GameObject sellAmountText;
    [SerializeField]
    private int coinsPerIsland = 500;

    private Button exitBtn;
    private Button sellBtn;
    private TextMeshProUGUI contentT;
    private TextMeshProUGUI sellAmountT;

    private NavigationDataManager navManager;
    private IslandMapsManager mapManager;
    private PlayerInventory inventory;

    private void Awake()
    {
        exitBtn = exitButton.GetComponent<Button>();
        sellBtn = sellButton.GetComponent<Button>();
        contentT = contentText.GetComponent<TextMeshProUGUI>();
        sellAmountT = sellAmountText.GetComponent<TextMeshProUGUI>();

        exitBtn.onClick.AddListener(CloseModal);
        sellBtn.onClick.AddListener(SellNavData);

        PlayerSceneInteraction.DisableInteraction(INTERACTION_LOCK_KEY);
    }

    public void InitParameters(GameObject navDataManager, GameObject inventoryObject, GameObject islandMapsManager)
    {
        navManager = navDataManager.GetComponent<NavigationDataManager>();
        inventory = inventoryObject.GetComponent<PlayerInventory>();
        mapManager = islandMapsManager.GetComponent<IslandMapsManager>();

        SetContentText(navManager.GetNumNavigatedIslands());
    }

    private void CloseModal()
    {
        PlayerSceneInteraction.EnableInteraction(INTERACTION_LOCK_KEY);
        Destroy(gameObject);
    }

    private void SellNavData()
    {
        int numIslands = navManager.GetNumNavigatedIslands();
        if (numIslands > 0)
        {
            int sellAmount = coinsPerIsland * numIslands;

            inventory.IncrementCoin(sellAmount);

            Islands[] newIslands = navManager.GetNavigatedIslands();
            foreach (Islands island in newIslands)
            {
                mapManager.DiscoverIsland(island);
            }

            navManager.ResetNavData();
            
            resultsContainer.SetActive(true);
            sellAmountT.SetText($"+{sellAmount}");
            SetContentText(0);

            AudioManager.GetInstance().Play(SoundMap.COINS);
        }
    }

    private void SetContentText(int numNavData)
    {
        contentT.SetText($"You have discovered {numNavData} new locations." +
            $"You may sell the navigation data to add the locations to your map, " +
            $"and gain a coin reward.");
    }
}
