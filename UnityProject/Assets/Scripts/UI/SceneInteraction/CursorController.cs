using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorController : MonoBehaviour
{
    [SerializeField]
    private Sprite aimSprite;
    [SerializeField]
    private Sprite uiSprite;
    [SerializeField]
    private bool lockCursorToUiMode = false;
    [SerializeField]
    private float maxCursorScale = 100f;
    [SerializeField]
    private float cursorUIScale = 50f;

    private Camera cam;
    private Image cursorImg;

    private void Start()
    {
        cam = Camera.main;
        cursorImg = gameObject.GetComponent<Image>();

        Cursor.visible = false;

        if (lockCursorToUiMode)
            cursorImg.sprite = uiSprite;
        else
            PlayerSceneInteraction.AddInteractionChangeListener(ControlSpriteAppearance);
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

    private void ControlSpriteAppearance(bool interactionEnabled)
    {
        Sprite appearance = interactionEnabled ? aimSprite : uiSprite;
        cursorImg.sprite = appearance;
    }
}
