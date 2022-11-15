using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaLifecycleManager : MonoBehaviour
{
    [SerializeField]
    private GameObject shipPrefabManagerObject;
    [SerializeField]
    private GameObject shipSafetyManagerObject;

    private ShipPrefabManager shipPrefabManager;
    private ShipSafetyManager safetyManager;

    private void Start()
    {
        shipPrefabManager = shipPrefabManagerObject.GetComponent<ShipPrefabManager>();
        safetyManager = shipSafetyManagerObject.GetComponent<ShipSafetyManager>();

        shipPrefabManager.SpawnShip(0);
        safetyManager.SetShipSafety(false);
    }
}
