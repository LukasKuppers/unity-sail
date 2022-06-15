using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIShipController : MonoBehaviour, IQueuableTask
{
    [SerializeField]
    private float aggressionDistance = 100f;
    [SerializeField]
    private float goalProximityDistance = 10f;
    [SerializeField]
    private float chaseTimeLimitSec = 180f;

    private GameObject shipPrefabManager;
    private GameObject shipSafetyManager;

    private ShipPrefabManager spawnedShipManager;
    private ShipSafetyManager safetyManager;
    private AIShipMode shipMode = AIShipMode.Anchored;
    private IAutomaticShip ship;
    private ShipPathfinder pathfinder;

    private GameObject targetObject;
    private Vector3 goalLocation;
    private float chaseDuration = 0f;

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

    public void RunTask(float deltaTime)
    {
        if (safetyManager.ShipIsSafe() || shipMode == AIShipMode.Anchored ||
            (shipMode == AIShipMode.Passive && goalLocation == null))
        {
            ship.DisableMovement();
            chaseDuration = 0f;
        }
        else
        {
            if (shipMode == AIShipMode.Passive)
            {
                chaseDuration = 0f;
                SeekGoal();
            }
            else
            {
                if (goalLocation == null)
                {
                    ship.EnableMovement();
                    Attack();
                    chaseDuration += deltaTime;
                }
                else
                {
                    float distToTarget = Vector3.Distance(targetObject.transform.position, transform.position);
                    float targetDistToGoal = Vector3.Distance(goalLocation, targetObject.transform.position);
                    if (distToTarget <= aggressionDistance && chaseDuration < chaseTimeLimitSec)
                    {
                        ship.EnableMovement();
                        Attack();
                        chaseDuration += deltaTime;
                    }
                    else
                    {
                        SeekGoal();
                    }
                    if (distToTarget > aggressionDistance)
                        chaseDuration = 0;
                    if (targetDistToGoal <= aggressionDistance)
                        chaseDuration = 0;
                }                
            }
        }
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

    private void SeekGoal()
    {
        if (goalLocation == null)
            return;

        float distToGoal = Vector3.Distance(goalLocation, transform.position);
        if (distToGoal <= goalProximityDistance)
            ship.DisableMovement();
        else
        {
            ship.EnableMovement();
            pathfinder.TravelToPoint(goalLocation);
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

        pathfinder.TravelToPoint(target);
    }
}

public enum AIShipMode
{
    Agressive, 
    Passive, 
    Anchored
}
