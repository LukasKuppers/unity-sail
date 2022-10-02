using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Globalization;
using System.Linq;

public class IslandVisitNotifier : MonoBehaviour
{
    [SerializeField]
    private GameObject hudParent;
    [SerializeField]
    private GameObject islandVisitManager;

    [SerializeField]
    private GameObject islandVisitBannerPrefab;

    private IslandVisitManager visitManager;

    private readonly Islands[] noNotifyIslands = {
        Islands.EUREKA_TRADING_POST, 
        Islands.PIRATE_OUTPOST_1, 
        Islands.PIRATE_OUTPOST_2,
        Islands.PIRATE_OUTPOST_3, 
        Islands.NAVY_OUTPOST_1,
        Islands.NAVY_OUTPOST_2,
        Islands.NAVY_OUTPOST_3
    };

    private void Start()
    {
        visitManager = islandVisitManager.GetComponent<IslandVisitManager>();

        visitManager.AddGeneralVisitListener(DisplayVisitBanner);
    }

    private void DisplayVisitBanner(Islands island)
    {
        bool shouldNotify = !noNotifyIslands.Contains(island);
        if (shouldNotify)
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
