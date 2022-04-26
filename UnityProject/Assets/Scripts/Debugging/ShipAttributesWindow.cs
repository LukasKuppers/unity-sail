using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShipAttributesWindow : MonoBehaviour
{
    private GameObject ship;
    private TextMeshProUGUI text;
    private IShipController controller;
    private IDamageable health;

    private void Start()
    {
        text = gameObject.GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (ship != null && ship.activeSelf)
        {
            SetText();
        }
    }

    public void SetShipObject(GameObject newShip)
    {
        ship = newShip;
        controller = ship.GetComponent<IShipController>();
        health = ship.GetComponent<IDamageable>();
    }

    private void SetText()
    {
        float sailAngle = controller.GetSailAngle();
        float sailHeight = controller.GetSailHeight();
        float speed = controller.GetSpeed();
        float hp = health.GetHealth();

        string display = "Sail Angle: " + sailAngle + "\n" +
            "Sail Height: " + sailHeight + "\n" +
            "Speed: " + speed + "\n" +
            "Health: " + hp;

        text.text = display;
    }
}
