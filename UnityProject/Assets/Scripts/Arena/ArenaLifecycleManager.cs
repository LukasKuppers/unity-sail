using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaLifecycleManager : MonoBehaviour
{
    [SerializeField]
    private GameObject shipSafetyManagerObject;

    private ShipSafetyManager safetyManager;

    private void Start()
    {
        safetyManager = shipSafetyManagerObject.GetComponent<ShipSafetyManager>();

        safetyManager.SetShipSafety(false);
    }
}
