using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour, IDamageable
{
    [SerializeField]
    private float maxHealth;

    private IDestructable destroyer;
    private float healthPoints;

    private void Awake()
    {
        destroyer = gameObject.GetComponent<IDestructable>();
        healthPoints = maxHealth;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public float GetHealth()
    {
        return healthPoints;
    }

    public void SetHealth(float health)
    {
        healthPoints = health;
    }

    public void ResetHealth()
    {
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
