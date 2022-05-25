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

    private Queue<float> sailHeightsQueue;

    private bool sailCoroutineRunning = false;

    private void Awake()
    {
        shipController = gameObject.GetComponent<IShipController>();
        if (WindManager != null)
        {
            wind = WindManager.GetComponent<WindGenerator>();
        }
        sailHeightsQueue = new Queue<float>();
    }

    private void Update()
    {
        if (shipController.GetSailHeight() > 0f)
        {
            FollowTarget();
        }
    }

    public void SetWindManager(GameObject newWindManager)
    {
        WindManager = newWindManager;
        wind = WindManager.GetComponent<WindGenerator>();
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
        EnqueueSailChange(0f);
        shipController.SetSteerAmount(0);
    }

    public void EnableMovement()
    {
        EnqueueSailChange(1f);
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

    private void EnqueueSailChange(float height)
    {
        if (!sailCoroutineRunning)
        {
            StartCoroutine(SetSail(height));
            return;
        }
        sailHeightsQueue.Enqueue(height);
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
        sailCoroutineRunning = true;
        float current = shipController.GetSailHeight();

        for (float t = 0; t <= 1; t += Time.deltaTime)
        {
            float height = Mathf.SmoothStep(current, sailHeight, t);
            shipController.SetSailHeight(height);
            yield return null;
        }
        shipController.SetSailHeight(sailHeight);
        sailCoroutineRunning = false;

        if (sailHeightsQueue.Count > 0)
        {
            float nextHeight = sailHeightsQueue.Dequeue();
            StartCoroutine(SetSail(nextHeight));
        }
    }
}
