using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PickupDisplay : MonoBehaviour
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
    private float verticalOffset = 0f;

    private GameObject scenePickupObject;
    private Image itemIconImage;
    private TextMeshProUGUI amountText;

    private Camera mainCam;

    private void Awake()
    {
        itemIconImage = gameObject.GetComponent<Image>();
        amountText = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        mainCam = Camera.main;
    }

    private void Update()
    {
        Debug.Log("target name: " + scenePickupObject.name);
        transform.position = mainCam.WorldToScreenPoint(scenePickupObject.transform.position);
        transform.position += Vector3.up * verticalOffset;
    }

    public void SetInitParameters(Item itemType, int amount, GameObject target)
    {
        switch (itemType)
        {
            case Item.FOOD:
                itemIconImage.sprite = foodIcon;
                break;
            case Item.WOOD:
                itemIconImage.sprite = woodIcon;
                break;
            case Item.CANNONBALL:
                itemIconImage.sprite = cannonballIcon;
                break;
            case Item.TREASURE:
                itemIconImage.sprite = treasureIcon;
                break;
        }

        scenePickupObject = target;
        amountText.SetText(amount.ToString());
    }

    public void UpdateItemAmount(int newAmount)
    {
        amountText.SetText(newAmount.ToString());
    }
}
