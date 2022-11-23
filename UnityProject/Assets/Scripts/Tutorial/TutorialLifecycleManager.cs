using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialLifecycleManager : MonoBehaviour
{
    [SerializeField]
    private GameObject shipPrefabManagerObject;

    private ShipPrefabManager shipPrefabManager;

    private void Start()
    {
        shipPrefabManager = shipPrefabManagerObject.GetComponent<ShipPrefabManager>();

        shipPrefabManager.SpawnShip(0);
    }
}
