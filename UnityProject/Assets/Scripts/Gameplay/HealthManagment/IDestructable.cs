using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IDestructable
{
    public void AddDestructionListener(UnityAction call);

    public void Destroy();
}
