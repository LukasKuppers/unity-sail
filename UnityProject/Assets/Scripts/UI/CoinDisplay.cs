using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinDisplay : MonoBehaviour
{
    [SerializeField]
    private GameObject inventoryObject;
    [SerializeField]
    private GameObject amountText;

    private PlayerInventory inventory;
    private TextMeshProUGUI text;

    private void Start()
    {
        inventory = inventoryObject.GetComponent<PlayerInventory>();
        text = amountText.GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        text.SetText(inventory.GetCoinAmount().ToString());
    }
}
