using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Globalization;

public class IslandVisitNotifier : MonoBehaviour
{
    [SerializeField]
    private GameObject hudParent;
    [SerializeField]
    private GameObject islandVisitManager;

    [SerializeField]
    private GameObject islandVisitBannerPrefab;

    private IslandVisitManager visitManager;

    private void Start()
    {
        visitManager = islandVisitManager.GetComponent<IslandVisitManager>();

        visitManager.AddGeneralVisitListener(DisplayVisitBanner);
    }

    private void DisplayVisitBanner(Islands island)
    {
        if (island != Islands.EUREKA_TRADING_POST)
        {
            GameObject banner = Instantiate(islandVisitBannerPrefab, hudParent.transform);

            TextMeshProUGUI bannerText = banner.GetComponentInChildren<TextMeshProUGUI>();

            TextInfo textinfo = new CultureInfo("en-US", false).TextInfo;
            string islandName = island.ToString();
            islandName = islandName.Replace('_', ' ');
            islandName = textinfo.ToTitleCase(islandName.ToLower());
            bannerText.text = islandName;

            AudioManager.GetInstance().Play(SoundMap.WHISTLE);
        }
    }
}
