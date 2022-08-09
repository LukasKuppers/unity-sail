using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryIncreaseDisplay : MonoBehaviour
{
    [SerializeField]
    private Sprite foodIcon;
    [SerializeField]
    private Sprite woodIcon;
    [SerializeField]
    private Sprite cannonballIcon;
    [SerializeField]
    private Sprite treasureIcon;

    [SerializeField]
    private GameObject amountTextObject;
    [SerializeField]
    private GameObject iconObject;
    [SerializeField]
    private float animationDuration = 1.0f;

    private TextMeshProUGUI amountText;
    private Image icon;

    private void Start()
    {
        if (icon == null || amountText == null)
            InitObjects();

        float numFrames = animationDuration / Time.smoothDeltaTime;
        StartCoroutine(PickupAnimation((int)numFrames));
    }

    public void Init(Item type, int amount)
    {

        if (icon == null || amountText == null)
            InitObjects();

        switch (type)
        {
            case Item.FOOD:
                icon.sprite = foodIcon;
                break;
            case Item.WOOD:
                icon.sprite = woodIcon;
                break;
            case Item.CANNONBALL:
                icon.sprite = cannonballIcon;
                break;
            case Item.TREASURE:
                icon.sprite = treasureIcon;
                break;
        }

        amountText.text = amount.ToString();
    }

    private void InitObjects()
    {
        amountText = amountTextObject.GetComponent<TextMeshProUGUI>();
        icon = iconObject.GetComponent<Image>();
    }

    private IEnumerator PickupAnimation(int numFrames)
    {
        for (int i = 0; i < numFrames; i++)
        {
            float animPercent = (float)i / numFrames;

            transform.position += 0.5f * Vector3.up;

            float alpha = 1.0f - animPercent;
            Color col = icon.color;
            col.a = alpha;
            icon.color = col;

            yield return null;
        }

        Destroy(gameObject);
    }
}
