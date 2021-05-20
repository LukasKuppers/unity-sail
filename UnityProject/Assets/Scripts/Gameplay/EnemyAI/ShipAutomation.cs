using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipAutomation : MonoBehaviour, IAutomaticShip
{
    [SerializeField]
    private GameObject WindManager;

    [SerializeField]
    private float turnSpeed = 1.0f;

    private IShipController shipController;
    private WindGenerator wind;
    private Vector3 target;

    private void Start()
    {
        shipController = gameObject.GetComponent<IShipController>();
        wind = WindManager.GetComponent<WindGenerator>();
    }

    private void Update()
    {
        if (shipController.GetSailHeight() > 0f)
        {
            FollowTarget();
        }
    }

    public void SetTarget(Vector3 targetPosition)
    {
        if (targetPosition != null)
        {
            target = targetPosition;
        }
    }

    public void DisableMovement()
    {
        StartCoroutine(SetSail(0f));
    }

    public void EnableMovement()
    {
        StartCoroutine(SetSail(1f));
    }

    private void FollowTarget()
    {
        Steer();
        ControlSailAngle();
    }

    private void Steer()
    {
        Vector2 forward = new Vector2(transform.forward.x, transform.forward.z);
        Vector2 diff = new Vector2(target.x - transform.position.x, target.z - transform.position.z);
        float angleOffset = Vector2.SignedAngle(forward, diff);

        shipController.SetSteerAmount(-angleOffset * turnSpeed);
    }

    private void ControlSailAngle()
    {
        float currentAngle = shipController.GetSailAngle();

        float shipAngle = transform.localEulerAngles.y > 180f ? transform.localEulerAngles.y - 360f : transform.localEulerAngles.y;
        float currentGlobal = currentAngle + shipAngle;
        float targetAngle = wind.GetWindDirection();

        shipController.SetSailAngle(currentAngle + (targetAngle - currentGlobal) / 100f);
    }

    private IEnumerator SetSail(float sailHeight)
    {
        float current = shipController.GetSailHeight();

        for (float t = 0; t <= 1; t += Time.deltaTime)
        {
            float height = Mathf.SmoothStep(current, sailHeight, t);
            shipController.SetSailHeight(height);
            yield return null;
        }
        shipController.SetSailHeight(sailHeight);
    }
}
