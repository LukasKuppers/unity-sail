using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour, IShipController
{
    [SerializeField]
    private GameObject WindGenerator;
    [SerializeField]
    private float topSpeed = 1f;
    [SerializeField]
    private float agility = 0.1f;
    [SerializeField]
    private float keelInfluence = 0.5f;

    private WindGenerator wind;
    private AdvancedBuoyancy buoyancy;
    private ShipParameters parameters;

    private float sailHeight;
    private float sailAngle;
    private float steerAmount;

    private float rotation;
    private float speed;
    private float acceleration;
    

    private void Start()
    {
        wind = WindGenerator.GetComponent<WindGenerator>();
        buoyancy = gameObject.GetComponent<AdvancedBuoyancy>();

        parameters = new ShipParameters(keelInfluence);
    }

    private void Update()
    {
        CalculateSpeed();
        CalculateRotation();

        buoyancy.SetLookDirection(rotation);
        transform.position += gameObject.transform.forward * speed;
    }

    public float GetSailAngle()
    {
        return sailAngle;
    }

    public float GetSailHeight()
    {
        return sailHeight;
    }

    public float GetSpeed()
    {
        return speed;
    }

    public float GetSailMultiplier()
    {
        return parameters.GetSailMultiplier(rotation, sailAngle, wind.GetWindDirection());
    }

    public void SetSailHeight(float height)
    {
        sailHeight = Mathf.Clamp01(height);
    }

    public void SetSailAngle(float angle)
    {
        sailAngle = Mathf.Clamp(angle, -90, 90);
    }

    public void SetSteerAmount(float angle)
    {
        steerAmount = Mathf.Clamp(angle, -1, 1);
    }

    private void CalculateSpeed()
    {
        float potential = sailHeight * 
            parameters.GetSailMultiplier(rotation, sailAngle, wind.GetWindDirection()) * 
            parameters.GetKeelMultiplier(rotation, wind.GetWindDirection());
        float target = potential * topSpeed;
        float delta = target - speed;

        acceleration = delta * agility;
        speed += acceleration;
    }

    private void CalculateRotation()
    {
        rotation = (rotation + steerAmount) % 360f;
    }
}
