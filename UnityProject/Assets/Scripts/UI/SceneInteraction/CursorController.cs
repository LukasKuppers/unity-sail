using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    [SerializeField]
    private float maxCursorScale = 100f;
    [SerializeField]
    private float cursorUIScale = 50f;

    private Camera cam;
    private GameObject camObj;

    private void Start()
    {
        cam = Camera.main;
        camObj = cam.gameObject;

        //Cursor.visible = false;
    }

    private void Update()
    {
        ControlPosition();
        ControlScale();
    }

    private void ControlPosition()
    {
        transform.position = Input.mousePosition;
    }

    private void ControlScale()
    {
        float scale = cursorUIScale;
        if (PlayerSceneInteraction.InteractionEnabled())
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
                scale = maxCursorScale / hit.distance;
        }
        transform.localScale = Vector3.one * scale;
    }
}
