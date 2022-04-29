using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIShipController : MonoBehaviour
{
    [SerializeField]
    private GameObject shipPrefabManager;
    [SerializeField]
    private GameObject shipSafetyManager;

    private ShipPrefabManager spawnedShipManager;
    private ShipSafetyManager safetyManager;
    private GameObject targetObject;
    private AIShipMode shipMode = AIShipMode.Anchored;
    private IAutomaticShip ship;

    private void Start()
    {
        ship = gameObject.GetComponent<IAutomaticShip>();
        safetyManager = shipSafetyManager.GetComponent<ShipSafetyManager>();
        spawnedShipManager = shipPrefabManager.GetComponent<ShipPrefabManager>();
        spawnedShipManager.AddSpawnListener(UpdateTarget);
    }

    private void UpdateTarget()
    {
        targetObject = spawnedShipManager.GetCurrentShip();
    }

    private void Update()
    {
        if (safetyManager.ShipIsSafe())
        {
            SetMode(AIShipMode.Passive);
        }
        else
        {
            SetMode(AIShipMode.Agressive);
        }

        if (shipMode == AIShipMode.Agressive && targetObject != null)
        {
            Attack();
        }
    }

    public void SetMode(AIShipMode newMode)
    {
        shipMode = newMode;
        if (shipMode == AIShipMode.Anchored || shipMode == AIShipMode.Passive)
        {
            ship.DisableMovement();
        } else
        {
            ship.EnableMovement();
        }
    }

    private void Attack()
    {
        Vector3 stern = targetObject.transform.position - transform.position;
        Vector3 broadside = Vector3.Cross(stern, Vector3.up);

        float angle = Vector3.SignedAngle(transform.forward, stern, Vector3.up);
        float sign = Mathf.Sign(angle);

        broadside *= -sign;

        float alpha = Mathf.Clamp01(stern.magnitude / 100f);
        Vector3 target = transform.position + (alpha * stern.normalized) + ((alpha - 1f) * broadside.normalized);

        ship.SetTarget(target);
    }
}

public enum AIShipMode
{
    Agressive, 
    Passive, 
    Anchored
}
