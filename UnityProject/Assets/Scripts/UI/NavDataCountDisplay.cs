using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NavDataCountDisplay : MonoBehaviour
{
    [SerializeField]
    private GameObject navDataManager;
    [SerializeField]
    private GameObject islandVisitManager;

    private NavigationDataManager navManager;
    private IslandVisitManager visitManager;
    private TextMeshProUGUI countText;

    private void Start()
    {
        navManager = navDataManager.GetComponent<NavigationDataManager>();
        countText = gameObject.GetComponent<TextMeshProUGUI>();

        visitManager = islandVisitManager.GetComponent<IslandVisitManager>();
        visitManager.AddGeneralVisitListener(UpdateText);

        countText.SetText("0");
    }

    private void UpdateText(Islands _island)
    {
        countText.SetText(navManager.GetNumNavigatedIslands().ToString());
    }
}
