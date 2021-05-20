using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancedBuoyancy : MonoBehaviour
{
    [SerializeField]
    private GameObject waveManager;

    private WaveGenerator waveFunc;

    private float lookDirection;

    public void SetLookDirection(float rotation)
    {
        lookDirection = rotation;
    }

    private void Start()
    {
        waveFunc = waveManager.GetComponent<WaveGenerator>();
        lookDirection = 0;
    }

    private void Update()
    {
        SetHeight();
        SetRotation();
    }

    private void SetHeight()
    {
        float height = waveFunc.Height(transform.position.x, transform.position.z);
        transform.position = new Vector3(transform.position.x, height, transform.position.z);
    }

    private void SetRotation()
    {
        Vector3 normal = waveFunc.Normal(transform.position.x, transform.position.z);
        Vector3 lookRotation = Quaternion.AngleAxis(lookDirection, normal) * Vector3.forward;
        transform.rotation = Quaternion.LookRotation(lookRotation, normal);
    }
}
