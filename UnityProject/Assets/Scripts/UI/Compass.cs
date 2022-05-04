using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Compass : MonoBehaviour
{
    [SerializeField]
    private GameObject shipPrefabManager;
    [SerializeField]
    private GameObject needle;

    private ShipPrefabManager shipManager;
    private GameObject ship;

    private void Start()
    {
        shipManager = shipPrefabManager.GetComponent<ShipPrefabManager>();

        shipManager.AddSpawnListener(UpdateShip);
    }

    private void Update()
    {
        if (ship != null)
        {
            float shipRot = ship.transform.rotation.eulerAngles.y;
            float needleRot = -shipRot + 90f;
            needle.transform.rotation = Quaternion.Euler(0, 0, needleRot);
        }
    }

    private void UpdateShip()
    {
        ship = shipManager.GetCurrentShip();
    }
}
