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

    private IslandVisitEvent generalVisitEvent;

    private void Start()
    {
        individualVisitEvents = new Dictionary<Islands, UnityEvent>();

        foreach (GameObject colliderObj in islandDetectionColliders)
        {
            IslandVisitCollider col = colliderObj.GetComponent<IslandVisitCollider>();
            col.AddVisitListener(IslandListener);
        }
    }

    public void AddGeneralVisitListener(UnityAction<Islands> call)
    {
        if (generalVisitEvent == null)
            generalVisitEvent = new IslandVisitEvent();

        generalVisitEvent.AddListener(call);
    }

    public void AddSpecificVisitListener(Islands island, UnityAction call)
    {
        if (!individualVisitEvents.ContainsKey(island))
            individualVisitEvents[island] = new UnityEvent();

        individualVisitEvents[island].AddListener(call);
    }

    private void IslandListener(Islands island)
    {
        if (generalVisitEvent == null)
            generalVisitEvent = new IslandVisitEvent();

        if (!individualVisitEvents.ContainsKey(island))
            individualVisitEvents[island] = new UnityEvent();

        generalVisitEvent.Invoke(island);
        individualVisitEvents[island].Invoke();
    }
}
