using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIShipController : MonoBehaviour
{
    [SerializeField]
    private GameObject targetObject;

    private AIShipMode shipMode = AIShipMode.Anchored;
    private IAutomaticShip ship;

    private void Start()
    {
        ship = gameObject.GetComponent<IAutomaticShip>();
    }

    private void Update()
    {
        if (shipMode != AIShipMode.Agressive)
        {
            SetMode(AIShipMode.Agressive);
        }

        if (shipMode == AIShipMode.Agressive)
        {
            Attack();
        }
    }

    public void SetMode(AIShipMode newMode)
    {
        shipMode = newMode;
        if (shipMode == AIShipMode.Anchored)
        {
            ship.DisableMovement();
        } else
        {
            ship.EnableMovement();
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

        ship.SetTarget(target);
    }
}

public enum AIShipMode
{
    Agressive, 
    Passive, 
    Anchored
}
