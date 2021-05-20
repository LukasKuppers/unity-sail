using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    public void Damage(float damageAmount);

    public void Restore(float restoreAmount);
}
