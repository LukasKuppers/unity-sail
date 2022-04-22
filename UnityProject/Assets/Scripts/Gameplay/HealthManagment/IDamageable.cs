using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    public float GetHealth();

    public void ResetHealth();

    public void Damage(float damageAmount);

    public void Restore(float restoreAmount);
}
