using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShipController
{
    public float GetSailAngle();

    public float GetSailHeight();

    public float GetSpeed();

    public float GetSailMultiplier();

    public void SetSailHeight(float sailHeight);

    public void SetSailAngle(float sailAngle);

    public void SetSteerAmount(float steerAmount);
}
