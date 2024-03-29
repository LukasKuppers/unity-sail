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

    private void OnDisable()
    {
        StopCoroutine(SetSail());
        sailCoroutineRunning = false;
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
        sailHeightsQueue.Clear();
        sailHeightsQueue.Enqueue(height);

        if (!sailCoroutineRunning)
        {
            StartCoroutine(SetSail());
            return;
        }
    }

    private void ControlSailAngle()
    {
        float currentAngle = shipController.GetSailAngle();
        float windAngle = wind.GetWindDirection();

        Vector2 windVector = Vector2Util.DegreeToVector2(windAngle);
        Vector2 shipVector = new Vector2(transform.forward.x, transform.forward.z);
        float localWindAngle = Vector2.SignedAngle(shipVector, windVector) * -1;

        float sailDelta = (localWindAngle - currentAngle) / 100f;

        shipController.SetSailAngle(currentAngle + sailDelta);
    }

    private IEnumerator SetSail()
    {
        sailCoroutineRunning = true;

        while (sailHeightsQueue.Count > 0)
        {
            float targetHeight = sailHeightsQueue.Dequeue();
            float currentHeight = shipController.GetSailHeight();

            if (targetHeight == currentHeight)
            {
                yield return null;
                continue;
            }

            for (float t = 0; t <= 1; t += Time.deltaTime)
            {
                float height = Mathf.SmoothStep(currentHeight, targetHeight, t);
                shipController.SetSailHeight(height);
                yield return null;
            }
            shipController.SetSailHeight(targetHeight);
            yield return null;
        }
        sailCoroutineRunning = false;
    }
}
