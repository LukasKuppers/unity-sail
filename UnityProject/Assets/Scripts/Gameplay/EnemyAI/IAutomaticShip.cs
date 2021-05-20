using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAutomaticShip 
{
    public void SetTarget(Vector3 targetPosition);

    public void DisableMovement();

    public void EnableMovement();
}
