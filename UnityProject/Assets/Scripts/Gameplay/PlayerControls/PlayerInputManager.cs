using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInputManager : MonoBehaviour
{
    [SerializeField]
    private float holdDuration = 0.5f;

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
        { KeyCode.Q, InputEvent.OPEN_NOTES },
        { KeyCode.F1, InputEvent.TAKE_SCREENSHOT },
        { KeyCode.F2, InputEvent.TOGGLE_UI }
    };

    private Dictionary<InputEvent, UnityEvent> eventMap;
    private Dictionary<KeyCode, InputEvent> keyBinding;

    private float[] mouseHoldTimes = new float[3] { 0, 0, 0 };
    private bool[] mouseHoldInvoked = new bool[3] { false, false, false };

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

        HandleMouseInput(0, InputEvent.HOLD_MOUSE_LEFT, InputEvent.MOUSE_LEFT);
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

    private void HandleMouseInput(int btnIndex, InputEvent holdEvent, InputEvent clickEvent)
    {
        if (Input.GetMouseButton(btnIndex))
        {
            mouseHoldTimes[btnIndex] += Time.deltaTime;
            if (mouseHoldTimes[btnIndex] > holdDuration && !mouseHoldInvoked[btnIndex])
            {
                InvokeEvent(holdEvent);
                mouseHoldInvoked[btnIndex] = true;
            }
        }

        if (Input.GetMouseButtonUp(btnIndex))
        {
            if (mouseHoldTimes[btnIndex] < holdDuration)
                InvokeEvent(clickEvent);
            mouseHoldTimes[btnIndex] = 0f;
            mouseHoldInvoked[btnIndex] = false;
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
    TAKE_SCREENSHOT,
    TOGGLE_UI,
    MOUSE_RIGHT, 
    MOUSE_LEFT, 
    HOLD_MOUSE_RIGHT, 
    HOLD_MOUSE_LEFT
}
