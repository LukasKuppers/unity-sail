using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsAdvancedBuoyancy : MonoBehaviour
{
    public GameObject[] buoyancyPoints;

    [SerializeField]
    private GameObject waveManager;
    [SerializeField]
    private float strength = 1.0f;

    private WaveGenerator waves;
    private Rigidbody rb;
    private float gravity;

    private void Start()
    {
        waves = waveManager.GetComponent<WaveGenerator>();
        rb = gameObject.GetComponent<Rigidbody>();

        gravity = Physics.gravity.magnitude;
    }

    public void SetWaveManager(GameObject newWaveManager)
    {
        waveManager = newWaveManager;
        waves = newWaveManager.GetComponent<WaveGenerator>();
    }

    private void FixedUpdate()
    {
        ApplyBuoyancyForce();
    }

    private void ApplyBuoyancyForce()
    {
        foreach (GameObject point in buoyancyPoints)
        {
            rb.AddForceAtPosition(Vector3.down * gravity / buoyancyPoints.Length, point.transform.position, ForceMode.Acceleration);
            ApplyBuoyancyAtPoint(point.transform.position);
        }
    }

    private void ApplyBuoyancyAtPoint(Vector3 point)
    {
        float depth = waves.Height(point.x, point.z) - point.y;
        float magnitude = strength * gravity * depth / buoyancyPoints.Length;

        if (depth > 0)
        {
            rb.AddForceAtPosition(Vector3.up * magnitude, point, ForceMode.Acceleration);
        }
    }
}
