using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedDestroyer : MonoBehaviour
{
    private static readonly string COUROTINE_CALL_NAME = "DestroyTimer";

    [SerializeField]
    private float lifeDuration = 10f;

    private void Awake()
    {
        StartCoroutine(COUROTINE_CALL_NAME);
    }

    private IEnumerator DestroyTimer()
    {
        yield return new WaitForSeconds(lifeDuration);

        Destroy(gameObject);
    }
}
