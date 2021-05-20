using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInputManager : MonoBehaviour
{
    private Vector3 prevMousePos;
    private Vector3 currentMousePos;

    private void Start()
    {
        prevMousePos = Input.mousePosition;
        currentMousePos = prevMousePos;
    }

    private void Update()
    {
        prevMousePos = currentMousePos;
        currentMousePos = Input.mousePosition;
    }

    public Vector2 GetMouseDelta()
    {
        return new Vector2(currentMousePos.x - prevMousePos.x, currentMousePos.y - prevMousePos.y);
    }
}
