using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsBasicBuoyancy : MonoBehaviour
{
    [SerializeField]
    private GameObject waveManager;

    public float strength;

    private WaveGenerator waves;
    private Rigidbody rb;

    private void Start()
    {
        waves = waveManager.GetComponent<WaveGenerator>();
        rb = gameObject.GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        ApplyBuoyancyForce();
    }

    private void ApplyBuoyancyForce()
    {
        float depth = waves.Height(transform.position.x, transform.position.z) - transform.position.y;
        float netForce = strength * depth * Physics.gravity.magnitude;

        if (netForce > 0)
        {
            rb.AddForce(netForce * Vector3.up, ForceMode.Acceleration);
        }
    }
}
