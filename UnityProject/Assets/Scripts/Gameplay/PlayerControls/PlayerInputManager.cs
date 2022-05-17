using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInputManager : MonoBehaviour
{
    private Dictionary<KeyCode, InputEvent> DEFAULT_BINDING = new Dictionary<KeyCode, InputEvent>()
    {
        { KeyCode.W, InputEvent.ACCELERATE_SHIP },
        { KeyCode.S, InputEvent.DECELERATE_SHIP },
        { KeyCode.A, InputEvent.STEER_LEFT },
        { KeyCode.D, InputEvent.STEER_RIGHT },
        { KeyCode.E, InputEvent.DROP_ITEMS },
        { KeyCode.F3, InputEvent.OPEN_DEBUG },
        { KeyCode.Escape, InputEvent.OPEN_MENU },
        { KeyCode.Tab, InputEvent.TOGGLE_MAP },
        { KeyCode.Q, InputEvent.OPEN_NOTES }
    };

    private Dictionary<InputEvent, UnityEvent> eventMap;
    private Dictionary<KeyCode, InputEvent> keyBinding;

    private void Start()
    {
        eventMap = new Dictionary<InputEvent, UnityEvent>();
        keyBinding = DEFAULT_BINDING;
    }

    private void Update()
    {
        foreach (KeyCode key in keyBinding.Keys)
        {
            if (Input.GetKeyDown(key))
            {
                InputEvent inputEvent = keyBinding[key];
                InvokeEvent(inputEvent);
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            InvokeEvent(InputEvent.MOUSE_LEFT);
        }
        if (Input.GetMouseButtonDown(1))
        {
            InvokeEvent(InputEvent.MOUSE_RIGHT);
        }
    }

    private void InvokeEvent(InputEvent inputEvent)
    {
        if (eventMap.ContainsKey(inputEvent))
        {
            eventMap[inputEvent].Invoke();
        }
    }

    public void AddInputListener(InputEvent eventType, UnityAction call)
    {
        if (!eventMap.ContainsKey(eventType))
        {
            UnityEvent newInputEvent = new UnityEvent();
            eventMap.Add(eventType, newInputEvent);
        }
        eventMap[eventType].AddListener(call);
    }

    public void RemoveInputListener(InputEvent eventType, UnityAction call)
    {
        if (eventMap.ContainsKey(eventType))
        {
            eventMap[eventType].RemoveListener(call);
        }
    }
}

public enum InputEvent
{
    ACCELERATE_SHIP, 
    DECELERATE_SHIP, 
    STEER_LEFT, 
    STEER_RIGHT, 
    DROP_ITEMS, 
    OPEN_DEBUG,
    OPEN_MENU, 
    TOGGLE_MAP, 
    OPEN_NOTES, 
    MOUSE_RIGHT, 
    MOUSE_LEFT
}
