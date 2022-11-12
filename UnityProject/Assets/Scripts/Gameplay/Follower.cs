using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    [SerializeField]
    private GameObject target;

    [SerializeField]
    private bool x;
    [SerializeField]
    private bool y;
    [SerializeField]
    private bool z;

    private Vector3 mask;
    private Vector3 inverseMask;

    private void Start()
    {
        mask = Vector3.zero;
        inverseMask = Vector3.one;
        if(x)
        {
            mask.x = 1;
            inverseMask.x = 0;
        }
        if(y)
        {
            mask.y = 1;
            inverseMask.y = 0;
        }
        if(z)
        {
            mask.z = 1;
            inverseMask.z = 0;
        }
    }

    private void Update()
    {
        if (target != null)
            transform.position = Vector3.Scale(target.transform.position, mask) + 
                                 Vector3.Scale(transform.position, inverseMask);
    }

    public void SetTarget(GameObject newTarget)
    {
        target = newTarget;
    }
}
