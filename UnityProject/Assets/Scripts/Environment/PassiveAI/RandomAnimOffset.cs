using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAnimOffset : MonoBehaviour
{
    [SerializeField]
    private string OffsetParameterName;

    private void Start()
    {
        Animator anim = gameObject.GetComponent<Animator>();

        float randOffset = Random.Range(0f, 1f);
        anim.SetFloat(OffsetParameterName, randOffset);
    }
}
