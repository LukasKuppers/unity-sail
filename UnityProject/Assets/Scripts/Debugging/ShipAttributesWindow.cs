using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShipAttributesWindow : MonoBehaviour
{
    [SerializeField]
    private GameObject ship;

    private TextMeshProUGUI text;
    private IShipController controller;

    private void Start()
    {
        text = gameObject.GetComponent<TextMeshProUGUI>();
        controller = ship.GetComponent<IShipController>();
    }

    private void Update()
    {
        SetText();
    }

    private void SetText()
    {
        float sailAngle = controller.GetSailAngle();
        float sailHeight = controller.GetSailHeight();
        float speed = controller.GetSpeed();

        string display = "Sail Angle: " + sailAngle + "\n" +
            "Sail Height: " + sailHeight + "\n" +
            "Speed: " + speed;

        text.text = display;
    }
}
