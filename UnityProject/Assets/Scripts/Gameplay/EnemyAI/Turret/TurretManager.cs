using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretManager : MonoBehaviour
{
    [SerializeField]
    private GameObject playerIslandVisitManager;
    [SerializeField]
    private GameObject playerShipPrefabManager;
    [SerializeField]
    private Islands island;
    [SerializeField]
    private GameObject initialTurret;
    [SerializeField]
    private GameObject turretPrefab;

    private IslandVisitManager visitManager;

    private AICanonController currentCannon;
    private GameObject turret;
    private GameObject destroyedTurret;

    private void Start()
    {
        visitManager = playerIslandVisitManager.GetComponent<IslandVisitManager>();

        visitManager.AddSpecificVisitListener(island, OnPlayerVisit);
        visitManager.AddSpecificDepartureListener(island, OnPlayerDepart);

        visitManager.AddSpecificVisitListener(Islands.EUREKA_TRADING_POST, RespawnTurret);

        if (initialTurret != null)
        {
            turret = initialTurret;
            InitializeTurret();
        }
        else
            RespawnTurret();
    }

    private void OnPlayerVisit()
    {
        SetCannonActive(true);
    }

    private void OnPlayerDepart()
    {
        SetCannonActive(false);
    }

    private void SetCannonActive(bool isActive)
    {
        if (currentCannon != null && currentCannon.gameObject != null)
        {
            currentCannon.enabled = isActive;
        }
    }

    private void OnTurretDestroyed(GameObject destroyedTurret)
    {
        turret = null;
        currentCannon = null;
        this.destroyedTurret = destroyedTurret;
    }

    private void RespawnTurret()
    {
        if (turret == null)
        {
            if (destroyedTurret != null)
                Destroy(destroyedTurret);

            turret = Instantiate(turretPrefab, transform.position, Quaternion.identity, transform);
            InitializeTurret();
        }
    }

    private void InitializeTurret()
    {
        if (turret != null)
        {
            currentCannon = turret.GetComponentInChildren<AICanonController>();
            currentCannon.SetShipPrefabManager(playerShipPrefabManager);
            turret.GetComponent<IDestructable>().AddDestructionListener(OnTurretDestroyed);
        }
    }
}
