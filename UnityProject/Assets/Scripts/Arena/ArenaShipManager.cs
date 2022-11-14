using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaShipManager : MonoBehaviour
{
    private static readonly int MAX_SHIP_INDEX = 3;

    [SerializeField]
    private GameObject shipPrefabManagerObject;

    private ShipPrefabManager shipPrefabManager;

    private void Start()
    {
        shipPrefabManager = shipPrefabManagerObject.GetComponent<ShipPrefabManager>();

        shipPrefabManager.SpawnShip(0);
    }

    public void UpgradeShip()
    {
        int currentShipIndex = shipPrefabManager.GetShipIndex();

        if (currentShipIndex >= MAX_SHIP_INDEX)
        {
            Debug.LogWarning("ArenaShipManager:UpgradeShip: max ship already unlocked");
            return;
        }

        shipPrefabManager.SpawnShip(currentShipIndex + 1);
        AudioManager.GetInstance().Play(SoundMap.TRUMPET);
    }
}
