using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProjectileShooter
{
    public void SetParentRigidbody(Rigidbody rb);

    public void SetRotationLimits(float yaw, float pitch);

    public void SetOrientation(float yaw, float pitch);

    public void SetPitch(float range, float height);

    public void Shoot();

    public void DisplayAim();
}
