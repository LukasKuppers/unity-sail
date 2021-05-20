using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveBird : MonoBehaviour
{
    [SerializeField]
    private GameObject centerPoint;
    [SerializeField]
    private float speed = 1f;
    [SerializeField]
    private float baseHeight = 20f;
    [SerializeField]
    private float horizontalRange = 20f;
    [SerializeField]
    private float verticalRange = 5f;
    [SerializeField]
    private float heightChangeFrequency = 1f;
    [SerializeField]
    private float turnSpeed = 1f;

    private void Update()
    {
        Translate();
        Rotate();
    }

    private void Translate()
    {
        transform.position += transform.forward * speed * Time.deltaTime;

        float perlinMultiplier = Mathf.PerlinNoise(transform.position.x * heightChangeFrequency, transform.position.z * heightChangeFrequency);
        float height = baseHeight + (perlinMultiplier * verticalRange);
        transform.position = new Vector3(transform.position.x, height, transform.position.z);
    }

    private void Rotate()
    {
        Vector3 diff = centerPoint.transform.position - transform.position;
        float targetAngle = Vector2.SignedAngle(new Vector2(transform.forward.x, transform.forward.z),
            new Vector2(diff.x, diff.z));

        float turnStrength = Vector3.Scale(diff, new Vector3(1, 0, 1)).magnitude / horizontalRange;
        float angle = turnStrength * targetAngle * turnSpeed * Time.deltaTime;
        transform.Rotate(new Vector3(0, -angle, 0), Space.World);
    }
}
