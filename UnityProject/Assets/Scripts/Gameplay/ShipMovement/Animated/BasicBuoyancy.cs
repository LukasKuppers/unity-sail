using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicBuoyancy : MonoBehaviour
{
    [SerializeField]
    private GameObject waveManager;

    private WaveGenerator waveFunc;

    private void Start()
    {
        waveFunc = waveManager.GetComponent<WaveGenerator>();
    }

    public void SetWaveManager(GameObject newWaveManager)
    {
        waveManager = newWaveManager;
        waveFunc = waveManager.GetComponent<WaveGenerator>();
    }

    private void Update()
    {
        SetHeight();
    }

    private void SetHeight()
    {
        float height = waveFunc.Height(transform.position.x, transform.position.z);
        transform.position = new Vector3(transform.position.x, height, transform.position.z);
    }
}
