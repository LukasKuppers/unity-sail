using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private IShipController controller;

    private float sailHeight = 0f;
    private float sailAngle = 0f;

    private void Start()
    {
        controller = gameObject.GetComponent<IShipController>();
    }

    private void Update()
    {
        SetSailParameters();

        controller.SetSailHeight(sailHeight);
        controller.SetSailAngle(sailAngle);
        controller.SetSteerAmount(0.1f * Input.GetAxis("Horizontal"));
    }

    private void SetSailParameters()
    {
        sailHeight += 0.1f * Input.GetAxis("Vertical");
        sailHeight = Mathf.Clamp01(sailHeight);

        sailAngle += Input.GetAxis("SecondaryHorizontal");
        sailAngle = Mathf.Clamp(sailAngle, -90, 90);
    }
}
