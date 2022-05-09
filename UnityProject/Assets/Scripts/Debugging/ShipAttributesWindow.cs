using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShipAttributesWindow : MonoBehaviour
{
    [SerializeField]
    private GameObject shipSafetyManager;

    private GameObject ship;
    private ShipSafetyManager safetyManager;
    private TextMeshProUGUI text;
    private IShipController controller;
    private IDamageable health;

    private bool isEnabled = false;

    private void Start()
    {
        safetyManager = shipSafetyManager.GetComponent<ShipSafetyManager>();
        text = gameObject.GetComponent<TextMeshProUGUI>();

        PlayerInputManager inputManager = InputReference.GetInputManager();
        inputManager.AddInputListener(InputEvent.OPEN_DEBUG, ToggleDebugWindow);
        SetText();
    }

    private void ToggleDebugWindow()
    {
        isEnabled = !isEnabled;
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
        if (isEnabled)
        {
            float sailAngle = controller.GetSailAngle();
            float sailHeight = controller.GetSailHeight();
            float speed = controller.GetSpeed();
            float hp = health.GetHealth();
            bool isSafe = safetyManager.ShipIsSafe();

            string display = "Sail Angle: " + sailAngle + "\n" +
                "Sail Height: " + sailHeight + "\n" +
                "Speed: " + speed + "\n" +
                "Health: " + hp + "\n" +
                "Ship Is Safe: " + isSafe;

            text.text = display;
        }
        else
        {
            text.text = "";
        }
    }
}
