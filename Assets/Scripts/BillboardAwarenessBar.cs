using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardAwarenessBar : MonoBehaviour
{
    public Transform PlayerCamera;

    private void Start()
    {
        PlayerCamera = Camera.main.transform;
    }

    // LateUpdate is called after Update function
    void LateUpdate()
    {
        transform.LookAt(transform.position + PlayerCamera.forward);
    }
}
