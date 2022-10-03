using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpecificCollisionListener : MonoBehaviour
{
    private Dictionary<GameObject, UnityAction> eventMap;

    private void Start()
    {
        eventMap = new Dictionary<GameObject, UnityAction>();
    }

    public void AddSpecificCollisionListener(GameObject collider, UnityAction call)
    {
        if (eventMap.ContainsKey(collider))
            eventMap[collider] = call;

        eventMap.Add(collider, call);
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject colObj = other.gameObject;

        if (eventMap.ContainsKey(colObj))
            eventMap[colObj].Invoke();
    }
}
