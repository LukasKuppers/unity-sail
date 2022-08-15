using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProjectileShooter
{
    public void SetParentRigidbody(Rigidbody rb);

    public void SetRotationLimits(float yaw, float pitch);

    public bool SetOrientation(float yaw, float pitch);

    public bool SetPitch(float range, float height);

    public bool Shoot();

    public void DisplayAim();
}
