using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class IslandVisitCollider : MonoBehaviour
{
    [SerializeField]
    private Islands island;

    private IslandVisitEvent visitEvent;

    private void Start()
    {
        if (visitEvent == null)
            visitEvent = new IslandVisitEvent();
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

    private void OnTriggerEnter(Collider other)
    {
        GameObject colObj = other.gameObject;
        if (colObj.CompareTag("Player"))
        {
            visitEvent.Invoke(island);
        }
    }
}
