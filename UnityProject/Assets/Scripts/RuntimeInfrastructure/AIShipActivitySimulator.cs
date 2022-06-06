using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIShipActivitySimulator : IQueuableTask
{
    private ShipPrefabManager playerShipManager;
    private GameObject player;
    private GameObject ship;
    private AIShipController shipController;

    private float loadDistance;
    private float simVelocity;

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
    }

    public void RunTask(float deltaTime)
    {
        if (ShipInActiveRange())
        {
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
