using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SailingAmbiance : MonoBehaviour
{
    [SerializeField]
    private float maximalVelocity = 10f;

    private AudioSource audioSource;
    private Rigidbody rb;

    private void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        rb = gameObject.GetComponent<Rigidbody>();

        audioSource.volume = 0;
    }

    private void Update()
    {
        SetAmbianceVolume();
    }

    private void SetAmbianceVolume()
    {
        Vector3 movingVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        float shipVelocity = movingVelocity.magnitude;

        float volume = shipVelocity / maximalVelocity;
        volume = Mathf.Clamp01(volume);

        audioSource.volume = volume;
    }
}
