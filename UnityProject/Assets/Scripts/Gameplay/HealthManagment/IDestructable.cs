using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IDestructable
{
    // returns the destroyed version of the object, if it exists
    public void AddDestructionListener(UnityAction<GameObject> call);

    public void Destroy();
}
