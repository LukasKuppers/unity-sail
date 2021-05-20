using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICanonController : MonoBehaviour
{
    [SerializeField]
    private GameObject targetObject;
    [SerializeField]
    private float maxRange = 50f;
    [SerializeField]
    private float maxYawAngle = 30f;

    private IProjectileShooter canon;
    private Rigidbody targetRb;

    public void SetTarget(GameObject target)
    {
        if (target != null)
        {
            targetObject = target;
            targetRb = targetObject.GetComponent<Rigidbody>();
        }
    }

    private void Start()
    {
        canon = gameObject.GetComponent<IProjectileShooter>();
        targetRb = targetObject.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        ControlFire();
    }

    private void ControlFire()
    {
        if (Vector3.Distance(transform.position, targetObject.transform.position) <= maxRange)
        {
            float aimAngle = Aim();
            if (Mathf.Abs(aimAngle) <= maxYawAngle)
            {
                canon.Shoot();
            }
        }
    }

    private float Aim()
    {
        Vector3 targetPos = targetObject.transform.position;
        if (targetRb != null)
        {
            targetPos += targetRb.velocity;
        }

        Vector2 diff = new Vector2(targetPos.x - transform.position.x,
            targetPos.z - transform.position.z);
        Vector2 forward = new Vector2(transform.forward.x, transform.forward.z);
        float aimAngle = Vector2.SignedAngle(forward, diff);

        Vector3 diff_3d = targetPos - transform.position;
        float range = Vector3.Scale(diff_3d, new Vector3(1, 0, 1)).magnitude;
        float height = transform.position.y - targetPos.y;

        canon.SetOrientation(-aimAngle, 0f);
        canon.SetPitch(range, height);

        return aimAngle;
    }
}
