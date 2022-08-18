using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipHunger : MonoBehaviour
{
    [SerializeField]
    private GameObject shipPrefabManager;
    [SerializeField]
    private GameObject shipSafetyManager;
    [SerializeField]
    private GameObject inventoryObject;
    [SerializeField]
    private float foodDecrementTickTime;
    [SerializeField]
    private float hungerTickTime;
    [SerializeField]
    private float hpPerTick;

    private ShipPrefabManager shipManager;
    private ShipSafetyManager safetyManager;
    private PlayerInventory inventory;
    private IDamageable healthManager;

    private bool coroutineRunning = false;

    private void Start()
    {
        shipManager = shipPrefabManager.GetComponent<ShipPrefabManager>();
        safetyManager = shipPrefabManager.GetComponent<ShipSafetyManager>();
        inventory = inventoryObject.GetComponent<PlayerInventory>();

        shipManager.AddSpawnListener(UpdateHealthManager);
    }

    private void UpdateHealthManager()
    {
        GameObject currentShip = shipManager.GetCurrentShip();
        healthManager = currentShip.GetComponent<IDamageable>();

        if (!coroutineRunning)
        {
            coroutineRunning = true;
            StartCoroutine(ApplyHunger());
        }
    }

    IEnumerator ApplyHunger()
    {
        while (true)
        {
            if (!safetyManager.ShipIsSafe())
            {
                if (inventory.GetFoodAmount() > 0)
                {
                    inventory.IncrementFood(-1);
                    yield return new WaitForSeconds(foodDecrementTickTime);
                }
                else
                {
                    healthManager.Damage(hpPerTick);
                    yield return new WaitForSeconds(hungerTickTime);
                }
            } 
            else
                yield return new WaitForSeconds(foodDecrementTickTime);
        }
    }
}
