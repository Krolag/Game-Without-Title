using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardAwarenessBar : MonoBehaviour
{
    public Transform PlayerCamera;

    // LateUpdate is called after Update function
    void LateUpdate()
    {
        transform.LookAt(transform.position + PlayerCamera.forward);
    }
}
