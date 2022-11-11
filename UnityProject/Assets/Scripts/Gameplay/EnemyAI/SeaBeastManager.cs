using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaBeastManager : MonoBehaviour
{
    [SerializeField]
    private GameObject shipPrefabManagerObject;
    [SerializeField]
    private GameObject seaBeastAttackPrefab;
    [SerializeField]
    private GameObject explosionPrefab;
    [SerializeField]
    private float attackTime = 4.68f;
    [SerializeField]
    private float damageAmount = 30f;

    private ShipPrefabManager shipPrefabManager;

    private void Start()
    {
        shipPrefabManager = shipPrefabManagerObject.GetComponent<ShipPrefabManager>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
            AttackPlayer();
    }

    public void AttackPlayer()
    {
        AttackShip(shipPrefabManager.GetCurrentShip());
    }

    public void AttackShip(GameObject targetShip)
    {
        Quaternion randomRot = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
        Instantiate(seaBeastAttackPrefab, targetShip.transform.position, randomRot, targetShip.transform);

        StartCoroutine(AttackShipOnDelay(attackTime, targetShip));
    }

    private IEnumerator AttackShipOnDelay(float delay, GameObject targetShip)
    {
        yield return new WaitForSecondsRealtime(delay);

        Instantiate(explosionPrefab, targetShip.transform.position, Quaternion.identity);
        IDamageable healthManager = targetShip.GetComponent<IDamageable>();

        if (healthManager != null)
            healthManager.Damage(damageAmount);
    }
}
