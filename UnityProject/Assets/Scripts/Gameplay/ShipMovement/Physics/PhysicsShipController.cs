using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsShipController : MonoBehaviour, IShipController
{
    private static readonly float MIN_UP_Y_COMPONENT = -0.7071f;

    [SerializeField]
    private GameObject windManager;
    [SerializeField]
    private float keelInfluence;
    [SerializeField]
    private float topSpeed;
    [SerializeField]
    private float agility;
    [SerializeField]
    private float torque;
    [SerializeField]
    private bool intoWindBonus = false;

    private WindGenerator wind;
    private Rigidbody rb;
    private ShipParameters paramteters;

    private float sailHeight;
    private float sailAngle;
    private float steerAmount;

    private void Start()
    {
        wind = windManager.GetComponent<WindGenerator>();
        rb = gameObject.GetComponent<Rigidbody>();
        paramteters = new ShipParameters(keelInfluence);

        sailHeight = 0f;
        sailAngle = 0f;
        steerAmount = 0f;
    }

    public void SetWindManager(GameObject newWindManager)
    {
        windManager = newWindManager;
        wind = windManager.GetComponent<WindGenerator>();
    }

    private void FixedUpdate()
    {
        float magnitude = CalculateForce();

        rb.AddForce(transform.forward * magnitude, ForceMode.Acceleration);
        rb.AddRelativeTorque(Vector3.up * steerAmount * torque);

        PerformCapsizeCheck();
    }

    public float GetSailHeight()
    {
        return sailHeight;
    }

    public float GetSailAngle()
    {
        return sailAngle;
    }

    public float GetSpeed()
    {
        return (rb.velocity - (Vector3.up * rb.velocity.y)).magnitude;
    }

    public float GetSailMultiplier()
    {
        float multiplier = paramteters.GetSailMultiplier(transform.forward, sailAngle, wind.GetWindDirection());

        multiplier = intoWindBonus ? Mathf.Clamp(multiplier, 0.5f, 1.0f) : multiplier;

        return multiplier;
    }

    public void SetSailHeight(float height)
    {
        sailHeight = Mathf.Clamp01(height);
    }

    public void SetSailAngle(float angle)
    {
        sailAngle = Mathf.Clamp(angle, -90f, 90f);
    }

    public void SetSteerAmount(float amount)
    {
        steerAmount = Mathf.Clamp(amount, -1f, 1f);
    }

    private float CalculateForce()
    {
        float potential = sailHeight *
            GetSailMultiplier() *
            paramteters.GetKeelMultiplier(transform.forward, wind.GetWindDirection());

        float target = potential * topSpeed;
        float delta = target - GetSpeed();

        return delta * agility;
    }

    private void PerformCapsizeCheck()
    {
        Vector3 localUp = transform.up;
        if (localUp.y <= MIN_UP_Y_COMPONENT)
        {
            // destroy ship
            IDestructable shipDestroyer = gameObject.GetComponent<IDestructable>();
            if (shipDestroyer != null)
                shipDestroyer.Destroy();
            else
                Destroy(gameObject);
        }
    }
}
