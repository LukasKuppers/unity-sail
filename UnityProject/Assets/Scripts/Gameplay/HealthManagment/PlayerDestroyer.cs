using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDestroyer : MonoBehaviour, IDestructable
{
    [SerializeField]
    private GameObject respawnPoint;
    [SerializeField]
    private GameObject playerInventory;

    [SerializeField]
    private int defaultFoodAmount = 0;
    [SerializeField]
    private int defaultWoodAmount = 0;
    [SerializeField]
    private int defaultCannonballAmount = 0;

    private PlayerInventory inventory;
    private HealthManager playerHealth;
    private IShipController shipController;

    private void Start()
    {
        inventory = playerInventory.GetComponent<PlayerInventory>();
        playerHealth = gameObject.GetComponent<HealthManager>();
        shipController = gameObject.GetComponent<IShipController>();
    }

    public void SetRespawnPoint(GameObject newRespawnPoint)
    {
        respawnPoint = newRespawnPoint;
    }

    public void SetPlayerInventory(GameObject newInventory)
    {
        playerInventory = newInventory;
        inventory = playerInventory.GetComponent<PlayerInventory>();
    }

    public void Destroy()
    {
        inventory.SetFoodAmount(defaultFoodAmount);
        inventory.SetWoodAmount(defaultWoodAmount);
        inventory.SetCannonballAmount(defaultCannonballAmount);
        inventory.SetTreasureAmount(0);

        playerHealth.ResetHealth();

        shipController.SetSailHeight(0f);
        transform.position = respawnPoint.transform.position;
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}
