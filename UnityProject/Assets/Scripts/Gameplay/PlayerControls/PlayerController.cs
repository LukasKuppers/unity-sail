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

    public void SetWindManager(GameObject newWindManager)
    {
        windManager = newWindManager;
        wind = windManager.GetComponent<WindGenerator>();
    }

    private void Update()
    {
        SetSailParameters();

        controller.SetSailAngle(sailAngle);

        if (PlayerSceneInteraction.InteractionEnabled())
        {
            controller.SetSailHeight(sailHeight);
            controller.SetSteerAmount(0.1f * Input.GetAxis("Horizontal"));
        }
    }

    private void SetSailParameters()
    {
        sailHeight += 0.1f * Input.GetAxis("Vertical");
        sailHeight = Mathf.Clamp01(sailHeight);

        sailAngle = GetSailAngle(sailAngle);
    }

    private float GetSailAngle(float currentSailAngle)
    {
        float windAngle = wind.GetWindDirection();
        Vector2 windVector = Vector2Util.DegreeToVector2(windAngle);
        Vector2 shipVector = new Vector2(transform.forward.x, transform.forward.z);
        float localWindAngle = Vector2.SignedAngle(shipVector, windVector) * -1;  

        float sailDelta = (localWindAngle - currentSailAngle) / 100f;

        return currentSailAngle + sailDelta;
    }
}
