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

    public void AddVisitListener(UnityAction<Islands> call)
    {
        if (visitEvent == null)
            visitEvent = new IslandVisitEvent();

        visitEvent.AddListener(call);
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject colObj = collision.gameObject;
        if (colObj.CompareTag("Player"))
        {
            visitEvent.Invoke(island);
        }
    }
}
