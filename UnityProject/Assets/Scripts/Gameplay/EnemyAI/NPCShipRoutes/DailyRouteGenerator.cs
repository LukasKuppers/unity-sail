using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DailyRouteGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject dayNightCycleManager;
    [SerializeField]
    private GameObject shipRoutesManager;
    [SerializeField]
    private int routesPerDay = 1;

    private DayNightCycle timeManager;
    private ShipRoutesManager routesManager;

    private void Start()
    {
        timeManager = dayNightCycleManager.GetComponent<DayNightCycle>();
        routesManager = shipRoutesManager.GetComponent<ShipRoutesManager>();

        timeManager.AddRepeatTimeListener(0f, GenerateNewRoutes);
    }

    private void GenerateNewRoutes()
    {
        for (int i = 0; i < routesPerDay; i++)
        {
            routesManager.CreateRandomRoute();
        }
    }
}
