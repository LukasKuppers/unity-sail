using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIShipController : MonoBehaviour, IQueuableTask
{
    private GameObject shipPrefabManager;
    private GameObject shipSafetyManager;

    private ShipPrefabManager spawnedShipManager;
    private ShipSafetyManager safetyManager;
    private AIShipMode shipMode = AIShipMode.Anchored;
    private IAutomaticShip ship;
    private ShipPathfinder pathfinder;

    private GameObject targetObject;
    private Vector3 goalLocation;

    private void Awake()
    {
        ship = gameObject.GetComponent<IAutomaticShip>();
        pathfinder = gameObject.GetComponent<ShipPathfinder>();
    }

    private void UpdateTarget()
    {
        targetObject = spawnedShipManager.GetCurrentShip();
    }

    public Vector3 GetGoalLocation()
    {
        return goalLocation;
    }

    public void RunTask(float _deltaTime)
    {
        if (safetyManager.ShipIsSafe())
        {
            SetMode(AIShipMode.Passive);
        }

        if (shipMode == AIShipMode.Agressive && targetObject != null)
        {
            Attack();
        }
        else if (shipMode == AIShipMode.Passive && goalLocation != null)
            pathfinder.TravelToPoint(goalLocation);
    }

    public void SetShipPrefabManager(GameObject newShipPrefabManager)
    {
        shipPrefabManager = newShipPrefabManager;
        spawnedShipManager = shipPrefabManager.GetComponent<ShipPrefabManager>();
        spawnedShipManager.AddSpawnListener(UpdateTarget);
        UpdateTarget();
    }

    public void SetShipSafetyManager(GameObject newShipSafetyManager)
    {
        shipSafetyManager = newShipSafetyManager;
        safetyManager = shipSafetyManager.GetComponent<ShipSafetyManager>();
    }

    public void SetMode(AIShipMode newMode)
    {
        shipMode = newMode;
        if (shipMode == AIShipMode.Anchored || (shipMode == AIShipMode.Passive && goalLocation == null))
        {
            ship.DisableMovement();
        } else
        {
            ship.EnableMovement();
        }
    }

    public void SetGoal(Vector3 goal)
    {
        goalLocation = goal;
        if (shipMode == AIShipMode.Passive || shipMode == AIShipMode.Agressive)
            ship.EnableMovement();
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

        pathfinder.TravelToPoint(target);
    }
}

public enum AIShipMode
{
    Agressive, 
    Passive, 
    Anchored
}
