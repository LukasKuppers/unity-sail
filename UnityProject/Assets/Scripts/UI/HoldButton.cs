using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class HoldButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    private float minHoldTime = 0.5f;

    private float holdTime = 0f;
    private bool singleCalled = false;
    private bool buttonDown = false;

    // called once when button is held for minHoldTime
    private UnityEvent onHoldSingle;
    // called every frame after button is held for minHoldTime
    private UnityEvent onHoldPersistent;

    public void AddOnHoldListener(UnityAction call)
    {
        if (onHoldSingle == null)
            onHoldSingle = new UnityEvent();

        onHoldSingle.AddListener(call);
    }

    public void AddOnHoldPersistenListener(UnityAction call)
    {
        if (onHoldPersistent == null)
            onHoldPersistent = new UnityEvent();

        onHoldPersistent.AddListener(call);
    }

    private void Update()
    {
        if (buttonDown)
        {
            holdTime += Time.deltaTime;

            if (holdTime >= minHoldTime)
            {
                if (!singleCalled && onHoldSingle != null)
                    onHoldSingle.Invoke();

                if (onHoldPersistent != null)
                    onHoldPersistent.Invoke();

                singleCalled = true;
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        buttonDown = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        buttonDown = false;
        holdTime = 0;
        singleCalled = false;
    }
}
