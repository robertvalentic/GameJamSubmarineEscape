using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemProp : MonoBehaviour, IInteractable
{
    void IInteractable.Interact(GameObject instigator)
    {
        if (instigator.tag == "Player")
        {
            instigator.GetComponent<CharacterControl>().AttachToGrabPoint(this.gameObject);
        }
    }
}
