using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class QuantityIncrementor : MonoBehaviour, ISelectableQuantity
{
    [SerializeField]
    private GameObject decrementButton;
    [SerializeField]
    private GameObject incrementButton;
    [SerializeField]
    private GameObject displayText;

    private Button decBtn;
    private Button incBtn;
    private TextMeshProUGUI quantityText;

    private int quantity;
    private int min = 0;
    private int max = int.MaxValue;
    UnityEvent quantityChangedEvent;

    private void Awake()
    {
        decBtn = decrementButton.GetComponent<Button>();
        incBtn = incrementButton.GetComponent<Button>();
        quantityText = displayText.GetComponent<TextMeshProUGUI>();

        decBtn.onClick.AddListener(Decrement);
        incBtn.onClick.AddListener(Increment);
    }

    public void SetQuantity(int newQuantity)
    {
        quantity = Mathf.Max(0, newQuantity);
        UpdateText();
        InvokeQuantityChangeEvent();
    }

    public void SetQuantityQuiet(int newQuantity)
    {
        quantity = Mathf.Max(0, newQuantity);
        UpdateText();
    }

    public void SetLimits(int minInclusive, int maxInclusive)
    {
        min = minInclusive;
        max = maxInclusive;
        quantity = Mathf.Clamp(quantity, min, max);
        InvokeQuantityChangeEvent();
    }

    public int GetQuantity()
    {
        return quantity;
    }

    public void AddChangeListener(UnityAction callback)
    {
        if (quantityChangedEvent == null)
        {
            quantityChangedEvent = new UnityEvent();
        }
        quantityChangedEvent.AddListener(callback);
    }

    private void Increment()
    {
        if (quantity < max)
        {
            quantity++;
            UpdateText();
            InvokeQuantityChangeEvent();
        }
    }

    private void Decrement()
    {
        if (quantity > min)
        {
            quantity--;
            UpdateText();
            InvokeQuantityChangeEvent();
        }
    }

    private void UpdateText()
    {
        quantityText.SetText(quantity.ToString());
    }

    private void InvokeQuantityChangeEvent()
    {
        if (quantityChangedEvent == null)
        {
            quantityChangedEvent = new UnityEvent();
        }
        quantityChangedEvent.Invoke();
    }
}
