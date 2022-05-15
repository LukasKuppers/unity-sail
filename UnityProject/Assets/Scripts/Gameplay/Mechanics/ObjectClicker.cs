using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectClicker : MonoBehaviour
{
    private IClickableObject action;
    private bool mouseIsFocused = false;

    private void Start()
    {
        PlayerInputManager inputManager = InputReference.GetInputManager();
        inputManager.AddInputListener(InputEvent.MOUSE_LEFT, InteractWithObject);

        action = gameObject.GetComponent<IClickableObject>();
    }

    private void InteractWithObject()
    {
        if (mouseIsFocused && PlayerSceneInteraction.InteractionEnabled())
        {
            action.Interact(GetLockKey());
        }
    }

    private void OnMouseEnter()
    {
        mouseIsFocused = true;
        PlayerAttackMode.DisableAttack(GetLockKey());
    }

    private void OnMouseExit()
    {
        mouseIsFocused = false;
        PlayerAttackMode.EnableAttack(GetLockKey());
    }

    private string GetLockKey()
    {
        return $"Object_clicker_key_{gameObject.name}";
    }
}
