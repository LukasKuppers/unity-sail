using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyIslandVisitManager : MonoBehaviour
{
    [SerializeField]
    private string islandColliderTag = "IslandProximityCol";

    private UnityEvent<Islands> generalVisitEvent;
    private Dictionary<Islands, UnityEvent> particularVisitEvents;

    public void AddGeneralVisitListener(UnityAction<Islands> call)
    {
        if (generalVisitEvent == null)
            generalVisitEvent = new UnityEvent<Islands>();

        generalVisitEvent.AddListener(call);
    }

    public void AddVisitListener(Islands island, UnityAction call)
    {
        if (particularVisitEvents == null)
            particularVisitEvents = new Dictionary<Islands, UnityEvent>();

        if (!particularVisitEvents.ContainsKey(island))
            particularVisitEvents.Add(island, new UnityEvent());

        particularVisitEvents[island].AddListener(call);
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject colObj = other.gameObject;
        if (colObj.CompareTag(islandColliderTag))
        {
            IslandVisitCollider islandCol = colObj.GetComponent<IslandVisitCollider>();
            Islands island = islandCol.GetIsland();

            if (generalVisitEvent != null)
                generalVisitEvent.Invoke(island);

            if (particularVisitEvents != null && particularVisitEvents.ContainsKey(island))
                particularVisitEvents[island].Invoke();
        }
    }
}
