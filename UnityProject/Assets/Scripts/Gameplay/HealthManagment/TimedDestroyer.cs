using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedDestroyer : MonoBehaviour
{
    private static readonly string COUROTINE_CALL_NAME = "DestroyTimer";

    [SerializeField]
    private float lifeDuration = 10f;

    private IDestructable objectDestroyer;

    private void Awake()
    {
        objectDestroyer = gameObject.GetComponent<IDestructable>();
        StartCoroutine(COUROTINE_CALL_NAME);
    }

    private IEnumerator DestroyTimer()
    {
        yield return new WaitForSeconds(lifeDuration);

        if (objectDestroyer != null)
        {
            objectDestroyer.Destroy();
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
