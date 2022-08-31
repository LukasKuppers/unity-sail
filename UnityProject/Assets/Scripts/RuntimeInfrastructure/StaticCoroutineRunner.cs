using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StaticCoroutineRunner : MonoBehaviour
{
    private static StaticCoroutineRunner instance;

    public static StaticCoroutineRunner GetInstance()
    {
        return instance;
    }

    private void Awake()
    {
        instance = this;
    }

    public void CallNextFrame(UnityAction call)
    {
        StartCoroutine(_CallNextFrame(call));
    }

    private IEnumerator _CallNextFrame(UnityAction call)
    {
        yield return new WaitForEndOfFrame();

        call.Invoke();
    }
}
