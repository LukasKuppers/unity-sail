using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class IslandVisitEvent : UnityEvent<Islands>
{
}

public class IslandVisitManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] islandDetectionColliders;

    private Dictionary<Islands, UnityEvent> individualVisitEvents;
    private Dictionary<Islands, UnityEvent> individualDepartureEvents;

    private IslandVisitEvent generalVisitEvent;
    private IslandVisitEvent generalDepartureEvent;

    private void Start()
    {
        if (individualVisitEvents == null)
            individualVisitEvents = new Dictionary<Islands, UnityEvent>();

        if (individualDepartureEvents == null)
            individualDepartureEvents = new Dictionary<Islands, UnityEvent>();

        foreach (GameObject colliderObj in islandDetectionColliders)
        {
            IslandVisitCollider col = colliderObj.GetComponent<IslandVisitCollider>();
            col.AddVisitListener(IslandVisitListener);
            col.AddDepartureListener(IslandDepartureListener);
        }
    }

    public void AddGeneralVisitListener(UnityAction<Islands> call)
    {
        if (generalVisitEvent == null)
            generalVisitEvent = new IslandVisitEvent();

        generalVisitEvent.AddListener(call);
    }

    public void AddGeneralDepartureListener(UnityAction<Islands> call)
    {
        if (generalDepartureEvent == null)
            generalDepartureEvent = new IslandVisitEvent();

        generalDepartureEvent.AddListener(call);
    }

    public void AddSpecificVisitListener(Islands island, UnityAction call)
    {
        if (individualVisitEvents == null)
            individualVisitEvents = new Dictionary<Islands, UnityEvent>();

        if (!individualVisitEvents.ContainsKey(island))
            individualVisitEvents[island] = new UnityEvent();

        individualVisitEvents[island].AddListener(call);
    }

    public void AddSpecificDepartureListener(Islands island, UnityAction call)
    {
        if (individualDepartureEvents == null)
            individualDepartureEvents = new Dictionary<Islands, UnityEvent>();

        if (!individualDepartureEvents.ContainsKey(island))
            individualDepartureEvents[island] = new UnityEvent();

        individualDepartureEvents[island].AddListener(call);
    }

    private void IslandVisitListener(Islands island)
    {
        if (generalVisitEvent == null)
            generalVisitEvent = new IslandVisitEvent();

        if (!individualVisitEvents.ContainsKey(island))
            individualVisitEvents[island] = new UnityEvent();

        generalVisitEvent.Invoke(island);
        individualVisitEvents[island].Invoke();
    }

    private void IslandDepartureListener(Islands island)
    {
        if (generalDepartureEvent == null)
            generalDepartureEvent = new IslandVisitEvent();

        if (!individualDepartureEvents.ContainsKey(island))
            individualDepartureEvents[island] = new UnityEvent();

        generalDepartureEvent.Invoke(island);
        individualDepartureEvents[island].Invoke();
    }
}
