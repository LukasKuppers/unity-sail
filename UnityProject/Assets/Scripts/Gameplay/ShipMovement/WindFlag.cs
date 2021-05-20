using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindFlag : MonoBehaviour
{
    [SerializeField]
    private GameObject windGenerator;

    private WindGenerator wind;

    private void Start()
    {
        wind = windGenerator.GetComponent<WindGenerator>();
    }

    private void Update()
    {
        SetRotation();    
    }

    private void SetRotation()
    {
        Vector3 normal = transform.parent.transform.up;
        Vector3 lookDirection = Quaternion.AngleAxis(wind.GetWindDirection(), normal) * Vector3.forward;
        transform.rotation = Quaternion.LookRotation(lookDirection, normal);
    }
}
