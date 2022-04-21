using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private GameObject windManager;

    private IShipController controller;

    private WindGenerator wind;
    private float sailHeight = 0f;
    private float sailAngle = 0f;

    private void Start()
    {
        wind = windManager.GetComponent<WindGenerator>();
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

        sailAngle = GetSailAngle(sailAngle);
    }

    private float GetSailAngle(float currentSailAngle)
    {
        float shipAngle = transform.localEulerAngles.y > 180f ? transform.localEulerAngles.y - 360f : transform.localEulerAngles.y;
        float currentGlobal = currentSailAngle + shipAngle;
        float targetAngle = wind.GetWindDirection();

        return currentSailAngle + (targetAngle - currentGlobal) / 100f;
    }
}
