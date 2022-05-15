using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IClickableObject
{
    public void Interact(string interactionLockKey);
}
