using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface ISelectableQuantity
{
    public void SetQuantity(int newQuantity);

    public void SetLimits(int minInclusive, int maxInclusive);

    public int GetQuantity();

    public void AddChangeListener(UnityAction callback);
}
