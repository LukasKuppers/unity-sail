using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipHealing : MonoBehaviour
{
    private static readonly float COROUTINE_WAIT_TIME = 1.0f;
    private static readonly float COROUTINE_TIMEOUT_TIME = 10.0f;

    [SerializeField]
    private GameObject shipPrefabManager;
    [SerializeField]
    private GameObject inventoryObject;
    [SerializeField]
    private float healthPerWood = 1f;

    PlayerInventory inventory;
    ShipPrefabManager shipManager;
    private IDamageable healthManager;

    private bool coroutineRunning = false;

    private float lastRrecordedHealth = float.PositiveInfinity;

    private void Start()
    {
        inventory = inventoryObject.GetComponent<PlayerInventory>();
        shipManager = shipPrefabManager.GetComponent<ShipPrefabManager>();
        shipManager.AddSpawnListener(UpdateHealthManager);
    }

    private void UpdateHealthManager()
    {
        GameObject currentShip = shipManager.GetCurrentShip();
        healthManager = currentShip.GetComponent<IDamageable>();

        if (!coroutineRunning)
        {
            coroutineRunning = true;
            StartCoroutine(RegenerateHealth());
        }
    }

    private IEnumerator RegenerateHealth()
    {
        while (true)
        {
            if (healthManager.GetHealth() < lastRrecordedHealth || 
                healthManager.GetHealth() == healthManager.GetMaxHealth())
            {
                lastRrecordedHealth = healthManager.GetHealth();
                yield return new WaitForSeconds(COROUTINE_TIMEOUT_TIME);
            }
            else if (healthManager.GetHealth() < healthManager.GetMaxHealth() &&
                inventory.GetWoodAmount() > 0)
            {
                inventory.IncrementWood(-1);
                healthManager.Restore(healthPerWood);
            }

            lastRrecordedHealth = healthManager.GetHealth();
            yield return new WaitForSeconds(COROUTINE_WAIT_TIME);
        }
    }
}
