using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ObjectDestroyedEvent : UnityEvent<GameObject>
{
}

public class ObjectDestroyer : MonoBehaviour, IDestructable
{
    [SerializeField]
    private GameObject destroyedPrefab;

    private ObjectDestroyedEvent destuctionEvent;

    public void AddDestructionListener(UnityAction<GameObject> call)
    {
        if (destuctionEvent == null)
            destuctionEvent = new ObjectDestroyedEvent();

        destuctionEvent.AddListener(call);
    }

    public void Destroy()
    {
        GameObject destroyedObj = Instantiate(destroyedPrefab, transform.position, transform.rotation);

        if (destuctionEvent != null)
            destuctionEvent.Invoke(destroyedObj);

        Destroy(gameObject);
    }
}
