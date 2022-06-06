using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCActivityManager : MonoBehaviour
{
    [SerializeField]
    private GameObject shipPrefabManager;
    [SerializeField]
    private float loadDistance = 500f;
    [SerializeField]
    private float simVelocity = 10f;

    private ShipPrefabManager shipManager;
    private DistributedTaskManager taskManager;

    private void Start()
    {
        shipManager = shipPrefabManager.GetComponent<ShipPrefabManager>();
        taskManager = DistributedTaskManager.GetInstance();
    }

    public void RegisterNewShip(GameObject ship)
    {
        IQueuableTask shipSim = new AIShipActivitySimulator(ship, shipManager, loadDistance, simVelocity);
        taskManager.AddStandaloneTask(shipSim);

        ship.GetComponent<IDestructable>().AddDestructionListener(
            () => KillSimTask(shipSim));
    }

    private void KillSimTask(IQueuableTask task)
    {
        taskManager.RemoveStandaloneTask(task);
    }
}
