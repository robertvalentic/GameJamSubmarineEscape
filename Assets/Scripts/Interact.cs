using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interact : MonoBehaviour
{
    [SerializeField] private Camera _camera = null;
    [SerializeField] private float _distance = 3.0f;
    private RaycastHit _hit;
    private bool _didHit;
    private bool _holdingItem = false;
    private GameObject _heldItem = null;

    void FixedUpdate()
    {
        if (_holdingItem == false)
        {
            // Shoots a raycast forward a certain amount every frame to check if what it collides with can be picked up or activated
            _didHit = Physics.Raycast(_camera.transform.position, _camera.transform.forward, out _hit, _distance);
        }
    }

    private void Update()
    {
        IInteractable item;

        if (Input.GetButtonDown("Interact") && _hit.collider.transform.tag == "Interactable" && _holdingItem == false)
        {
            item = _hit.collider.GetComponent<IInteractable>();

            if (item != null)
            {
                item.Interact(this.gameObject);
                _holdingItem = true;
                _heldItem = _hit.collider.gameObject;
            }
        }
        else if (Input.GetButtonDown("Interact") && _holdingItem == true)
        {
            DropHeldItem();
        }
    }

    private void DropHeldItem()
    {
        _heldItem.transform.parent = null;
        Rigidbody rb = _heldItem.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
        }
        _heldItem = null;
        _holdingItem = false;
    }
}
