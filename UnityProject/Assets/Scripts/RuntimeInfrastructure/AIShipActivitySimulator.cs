using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AIShipActivitySimulator : IQueuableTask
{
    private ShipPrefabManager playerShipManager;
    private GameObject player;
    private GameObject ship;
    private AIShipController shipController;

    private float loadDistance;
    private float simVelocity;
    private bool isRouteShip;

    public AIShipActivitySimulator(GameObject shipObj, ShipPrefabManager playerShipManager,
                                   float loadDistance, float simVelocity)
    {
        this.playerShipManager = playerShipManager;
        this.playerShipManager.AddSpawnListener(UpdatePlayerShip);
        UpdatePlayerShip();

        ship = shipObj;
        this.loadDistance = loadDistance;
        this.simVelocity = simVelocity;
        shipController = ship.GetComponent<AIShipController>();

        isRouteShip = false;
    }

    public AIShipActivitySimulator(GameObject shipObj, ShipPrefabManager playerShipManager, 
                                   float loadDistance, float simVelocity, bool isTravellingRoute)
    {
        this.playerShipManager = playerShipManager;
        this.playerShipManager.AddSpawnListener(UpdatePlayerShip);
        UpdatePlayerShip();

        ship = shipObj;
        this.loadDistance = loadDistance;
        this.simVelocity = simVelocity;
        shipController = ship.GetComponent<AIShipController>();

        isRouteShip = isTravellingRoute;
    }

    public void RunTask(float deltaTime)
    {
        if (ship == null)
            return;

        if (ShipInActiveRange())
        {
            if (!ship.activeSelf)
                ship.SetActive(true);
        }
        else
        {
            ship.SetActive(false);

            Vector3 goal = shipController.GetGoalLocation();
            if (goal != null)
            {
                Vector3 atGoal = new Vector3(goal.x - ship.transform.position.x, 0, goal.z - ship.transform.position.z);
                if (atGoal.magnitude > simVelocity)
                {
                    Vector3 dir = Vector3.Normalize(atGoal);
                    ship.transform.position += dir * simVelocity * deltaTime;
                }
                else
                    if (isRouteShip)
                        ship.GetComponent<IDestructable>().Destroy();
            }
        }
    }

    private void UpdatePlayerShip()
    {
        player = playerShipManager.GetCurrentShip();
    }

    private bool ShipInActiveRange()
    {
        float dist = Vector3.Distance(ship.transform.position, player.transform.position);
        return dist < loadDistance;
    }
}
