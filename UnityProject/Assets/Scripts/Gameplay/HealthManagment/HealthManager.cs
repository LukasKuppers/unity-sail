using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour, IDamageable
{
    [SerializeField]
    private float maxHealth;

    private IDestructable destroyer;
    private float healthPoints;

    private void Start()
    {
        destroyer = gameObject.GetComponent<IDestructable>();
        healthPoints = maxHealth;
    }

    public void Damage(float damageAmount)
    {
        if (damageAmount <= 0)
        {
            Debug.LogError("HealthManager: damageAmount must be positive");
            damageAmount *= -1f;
        }

        healthPoints -= damageAmount;

        if (healthPoints <= 0)
        {
            destroyer.Destroy();
        }
    }

    public void Restore(float restoreAmount)
    {
        if (restoreAmount <= 0)
        {
            Debug.LogError("HealthManager: restore amount must be positive");
        }

        healthPoints = Mathf.Clamp(healthPoints + restoreAmount, 0, maxHealth);
    }
}
