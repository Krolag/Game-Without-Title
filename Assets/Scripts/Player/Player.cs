﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Throwable _objectToThrow;
    [SerializeField] private Transform _throwingPosition;
    [SerializeField] private float _throwStrenght;

    private void Awake()
    {

    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Throw();
        }

        //some test actions
        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray ray = new Ray(Camera.main.gameObject.transform.position, Camera.main.gameObject.transform.forward);
            RaycastHit[] hits = Physics.RaycastAll(ray, 4f);

            foreach (var hit in hits)
            {
                Interactable interactable;
                if (hit.collider.TryGetComponent<Interactable>(out interactable))
                {
                    interactable.Interact(GetComponent<Player>());
                }
            }
        }
    }
    public void PickUp(Throwable throwObject)
    {
        if (throwObject == null)
            return;
        Rigidbody rb = throwObject.GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.detectCollisions = false;
        _objectToThrow = throwObject;
        throwObject.transform.position = _throwingPosition.position;
        throwObject.transform.SetParent(_throwingPosition, false);
        throwObject.transform.localPosition = Vector3.zero;

    }

    public void Throw()
    {
        if (_objectToThrow == null)
            return;

        _objectToThrow.transform.SetParent(null);

        Rigidbody rb = _objectToThrow.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.detectCollisions = true;

        _objectToThrow.Throw(Camera.main.transform.forward, _throwStrenght);
        _objectToThrow = null;
    }
}
