using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cartographer : MonoBehaviour, IClickableObject
{
    [SerializeField]
    private GameObject uIParent;
    [SerializeField]
    private GameObject cartographerModal;

    public void Interact(string _interactionLockKey)
    {
        Instantiate(cartographerModal, uIParent.transform);
    }
}
