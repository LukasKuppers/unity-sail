using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorController : MonoBehaviour
{
    [SerializeField]
    private GameObject shipPrefabManager;
    [SerializeField]
    private Sprite aimSprite;
    [SerializeField]
    private Sprite uiSprite;
    [SerializeField]
    private bool lockCursorToUiMode = false;
    [SerializeField]
    private float maxCursorScale = 100f;
    [SerializeField]
    private float minCursorScale = 5f;
    [SerializeField]
    private float cursorUIScale = 50f;
    [SerializeField]
    private float animRotateSpeed = 1f;

    private Camera cam;
    private Image cursorImg;
    private PlayerCanonController cannonController;

    private int layerMask;

    private void Start()
    {
        cam = Camera.main;
        cursorImg = gameObject.GetComponent<Image>();

        layerMask = 1 << PlayerCanonController.AIM_RAY_LAYER_MASK;
        layerMask = ~layerMask;

        if (!lockCursorToUiMode)
        {
            ShipPrefabManager prefabManager = shipPrefabManager.GetComponent<ShipPrefabManager>();
            prefabManager.AddSpawnListener(() =>
            {
                cannonController = prefabManager.GetCurrentShip().GetComponent<PlayerCanonController>();
            });
        }

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
        ControlAnimation();
    }

    private void ControlPosition()
    {
        transform.position = Input.mousePosition;
    }

    private void ControlScale()
    {
        float scale = cursorUIScale;
        if (PlayerSceneInteraction.InteractionEnabled() && !lockCursorToUiMode)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 3000f, layerMask))
                scale = maxCursorScale / hit.distance;
            
        }
        scale = Mathf.Max(minCursorScale, scale);
        transform.localScale = Vector3.one * scale;
    }

    private void ControlAnimation()
    {
        if (PlayerSceneInteraction.InteractionEnabled() && !lockCursorToUiMode)
        {
            if (cannonController != null && cannonController.TargetInRange())
                transform.Rotate(Vector3.forward, animRotateSpeed);
        }
    }

    private void ControlSpriteAppearance(bool interactionEnabled)
    {
        Sprite appearance = interactionEnabled ? aimSprite : uiSprite;
        cursorImg.sprite = appearance;

        transform.rotation = Quaternion.identity;
    }
}
