using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDestroyer : MonoBehaviour, IDestructable
{
    [SerializeField]
    private GameObject destroyedPrefab;

    public void Destroy()
    {
        Instantiate(destroyedPrefab, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
