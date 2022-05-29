using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectDestroyer : MonoBehaviour, IDestructable
{
    [SerializeField]
    private GameObject destroyedPrefab;

    private UnityEvent destuctionEvent;

    public void AddDestructionListener(UnityAction call)
    {
        if (destuctionEvent == null)
            destuctionEvent = new UnityEvent();

        destuctionEvent.AddListener(call);
    }

    public void Destroy()
    {
        if (destuctionEvent != null)
            destuctionEvent.Invoke();

        Instantiate(destroyedPrefab, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
