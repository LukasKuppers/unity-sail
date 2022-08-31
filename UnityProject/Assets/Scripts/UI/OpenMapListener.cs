using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenMapListener : MonoBehaviour
{
    [SerializeField]
    private GameObject shipPrefabManager;
    [SerializeField]
    private GameObject islandMapManager;
    [SerializeField]
    private GameObject mapTreasureManager;
    [SerializeField]
    private GameObject uIParent;
    [SerializeField]
    private GameObject mapPrefab;

    private ShipPrefabManager shipManager;
    private GameObject playerShip;

    private GameObject mapInstance;

    private void Start()
    {
        shipManager = shipPrefabManager.GetComponent<ShipPrefabManager>();
        shipManager.AddSpawnListener(UpdateShip);

        PlayerInputManager inputManager = InputReference.GetInputManager();
        inputManager.AddInputListener(InputEvent.TOGGLE_MAP, ToggleMap);
    }

    private void UpdateShip()
    {
        playerShip = shipManager.GetCurrentShip();
    }

    private void ToggleMap()
    {
        if (mapInstance == null)
        {
            if (PlayerSceneInteraction.InteractionEnabled())
            {
                mapInstance = Instantiate(mapPrefab, uIParent.transform);
                MapManager mapData = mapInstance.GetComponent<MapManager>();
                mapData.InitParameters(playerShip, islandMapManager, mapTreasureManager);

                AudioManager.GetInstance().Play(SoundMap.TURN_PAGE);
            }
        }
        else
        {
            Destroy(mapInstance);
        }
    }
}
