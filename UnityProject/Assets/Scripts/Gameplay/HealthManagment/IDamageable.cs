using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    public float GetHealth();

    public void SetHealth(float health);

    public void ResetHealth();

    public void Damage(float damageAmount);

    public void Restore(float restoreAmount);
}
