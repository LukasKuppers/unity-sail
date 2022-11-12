using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaBeastManager : MonoBehaviour
{
    private static readonly float MIN_ATTACK_INTERVAL = 45f;

    [SerializeField]
    private GameObject shipPrefabManagerObject;
    [SerializeField]
    private GameObject seaBeastAttackPrefab;
    [SerializeField]
    private GameObject seaBeastPassivePrefab;
    [SerializeField]
    private GameObject explosionPrefab;
    [SerializeField]
    private float attackTime = 4.68f;
    [SerializeField]
    private float damageAmount = 30f;
    [SerializeField]
    private float maxAttackInterval = 120f;

    private ShipPrefabManager shipPrefabManager;

    private bool isActive = false;

    private void Start()
    {
        shipPrefabManager = shipPrefabManagerObject.GetComponent<ShipPrefabManager>();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
            CirclePlayer();
    }

    public void EnableActiveAgression()
    {
        if (!isActive)
        {
            isActive = true;
            StartCoroutine(RunContinuousAttacks());
        }
    }

    public void DisableActiveAgression()
    {
        isActive = false;
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

    public void CirclePlayer()
    {
        CircleShip(shipPrefabManager.GetCurrentShip());
    }

    public void CircleShip(GameObject targetShip)
    {
        Quaternion randomRot = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
        Instantiate(seaBeastPassivePrefab, targetShip.transform.position, randomRot, targetShip.transform);
    }

    private IEnumerator AttackShipOnDelay(float delay, GameObject targetShip)
    {
        yield return new WaitForSecondsRealtime(delay);

        Instantiate(explosionPrefab, targetShip.transform.position, Quaternion.identity);
        IDamageable healthManager = targetShip.GetComponent<IDamageable>();

        if (healthManager != null)
            healthManager.Damage(damageAmount);
    }

    private IEnumerator RunContinuousAttacks()
    {
        while (isActive)
        {
            float waitTime = Random.Range(MIN_ATTACK_INTERVAL, maxAttackInterval);
            yield return new WaitForSeconds(waitTime);

            if (isActive)
                AttackPlayer();
        }
    }
}
