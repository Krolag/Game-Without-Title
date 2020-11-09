using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBullet : MonoBehaviour
{
    void Update()
    {
        //jesli wyleci poza okreslona wartosc w osi XYZ to ma byc zniszczone (Destroy(gameObject)
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
