using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class IslandVisitCollider : MonoBehaviour
{
    private static readonly string PLAYER_TAG = "Player";

    [SerializeField]
    private Islands island;

    private IslandVisitEvent visitEvent;
    private IslandVisitEvent departureEvent;

    private void Start()
    {
        if (visitEvent == null)
            visitEvent = new IslandVisitEvent();
        if (departureEvent == null)
            departureEvent = new IslandVisitEvent();
    }

    public Islands GetIsland()
    {
        return island;
    }

    public void AddVisitListener(UnityAction<Islands> call)
    {
        if (visitEvent == null)
            visitEvent = new IslandVisitEvent();

        visitEvent.AddListener(call);
    }

    public void AddDepartureListener(UnityAction<Islands> call)
    {
        if (departureEvent == null)
            departureEvent = new IslandVisitEvent();

        departureEvent.AddListener(call);
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject colObj = other.gameObject;
        if (colObj.CompareTag(PLAYER_TAG))
        {
            visitEvent.Invoke(island);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        GameObject colObj = other.gameObject;
        if (colObj.CompareTag(PLAYER_TAG))
        {
            departureEvent.Invoke(island);
        }
    }
}
