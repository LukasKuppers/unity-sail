using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NavDataCountDisplay : MonoBehaviour
{
    [SerializeField]
    private GameObject navDataManager;

    private NavigationDataManager navManager;
    private TextMeshProUGUI countText;

    private void Start()
    {
        navManager = navDataManager.GetComponent<NavigationDataManager>();
        countText = gameObject.GetComponent<TextMeshProUGUI>();

        navManager.AddNavCountChangeListener(UpdateText);
        countText.SetText("0");
    }

    private void UpdateText()
    {
        countText.SetText(navManager.GetNumNavigatedIslands().ToString());
    }
}
