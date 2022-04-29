using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeZone : MonoBehaviour
{
    [SerializeField]
    private GameObject shipPrefabManager;
    [SerializeField]
    private GameObject shipSafetyManager;
    [SerializeField]
    private float zoneRadius = 1f;

    private ShipPrefabManager shipManager;
    private ShipSafetyManager safetyManager;

    private GameObject ship;

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, zoneRadius);
    }

    private void Start()
    {
        shipManager = shipPrefabManager.GetComponent<ShipPrefabManager>();
        safetyManager = shipSafetyManager.GetComponent<ShipSafetyManager>();

        shipManager.AddSpawnListener(UpdateShip);
    }

    private void UpdateShip()
    {
        ship = shipManager.GetCurrentShip();
    }

    private void Update()
    {
        if (ship != null)
        {
            float dist = Vector3.Distance(transform.position, ship.transform.position);
            if (dist <= zoneRadius)
            {
                safetyManager.SetShipSafety(true);
            }
            else
            {
                safetyManager.SetShipSafety(false);
            }
        }
    }
}
