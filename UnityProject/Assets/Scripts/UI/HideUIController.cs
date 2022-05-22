using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideUIController : MonoBehaviour
{
    PlayerInputManager inputManager;
    Canvas uiCanvas;


    private void Start()
    {
        inputManager = InputReference.GetInputManager();
        uiCanvas = gameObject.GetComponent<Canvas>();

        inputManager.AddInputListener(InputEvent.TOGGLE_UI, ToggleUI);
    }

    private void ToggleUI()
    {
        uiCanvas.enabled = !uiCanvas.enabled;
    }
}
