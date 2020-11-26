﻿using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Interactable))]
public class Throwable : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private Interactable _interactable;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _interactable = GetComponent<Interactable>();
    }

    private void OnEnable()
    {
        _interactable.Interacted += PickUp;
    }
    private void OnDisable()
    {
        _interactable.Interacted -= PickUp;
    }
    public void Throw(Vector3 direction, float force)
    {
        _rigidbody.AddForce(direction * force);
    }
    
    public void PickUp(Player player)
    {
        player.PickUp(this);
    }
}