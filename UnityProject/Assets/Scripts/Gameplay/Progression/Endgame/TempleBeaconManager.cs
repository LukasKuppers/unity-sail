using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempleBeaconManager : MonoBehaviour
{
    [SerializeField]
    private GameObject beaconPrefab;
    [SerializeField]
    private GameObject destroyedBeaconPrefab;
    [SerializeField]
    private GameObject beacon;

    private GameObject destroyedBeacon;

    private void Start()
    {
        // beacon not destroyed by default
        if (beacon == null)
            RespawnBeacon();
        else
            beacon.GetComponent<IDestructable>().AddDestructionListener(OnBeaconDestroyed);

    }

    public GameObject GetCurrentBeaconObject()
    {
        if (beacon != null)
            return beacon;
        return destroyedBeacon;
    }

    public void RespawnBeacon()
    {
        if (beacon == null)
        {
            if (destroyedBeacon != null)
                Destroy(destroyedBeacon);

            beacon = Instantiate(beaconPrefab, transform.position, transform.rotation, gameObject.transform);
            beacon.GetComponent<IDestructable>().AddDestructionListener(OnBeaconDestroyed);
        }
    }

    public void SetBeaconDestroyed()
    {
        if (destroyedBeacon == null)
        {
            if (beacon != null)
                beacon.GetComponent<IDestructable>().Destroy();
            else
                destroyedBeacon = Instantiate(destroyedBeaconPrefab, transform.position, transform.rotation, gameObject.transform);
        }
    }

    private void OnBeaconDestroyed(GameObject newDestroyedObject)
    {
        beacon = null;
        destroyedBeacon = newDestroyedObject;
    }
}
