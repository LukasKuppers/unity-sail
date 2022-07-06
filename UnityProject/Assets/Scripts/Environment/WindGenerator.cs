using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindGenerator : MonoBehaviour
{
    private static readonly float COROUTINE_WAIT_TIME = 1.0f;

    [SerializeField]
    private float windChangeSpeed = 1f;

    private float direction;
    private float targetDirection;

    private void Start()
    {
        direction = Random.Range(0f, 360f);
        targetDirection = Random.Range(0f, 360f);

        StartCoroutine(WindMoveCoroutine());
    }

    public float GetWindDirection()
    {
        return direction;
    }

    private IEnumerator WindMoveCoroutine()
    {
        while (true)
        {
            if (Mathf.Abs(targetDirection - direction) < windChangeSpeed)
            {
                targetDirection = Random.Range(0f, 360f);
            }
            else
            {
                direction += Mathf.Sign(targetDirection - direction) * windChangeSpeed;
            }

            yield return new WaitForSeconds(COROUTINE_WAIT_TIME);
        }
    }
}
