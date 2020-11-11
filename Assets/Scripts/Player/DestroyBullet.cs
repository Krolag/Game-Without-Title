using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBullet : MonoBehaviour
{
    public Transform BulletPosition;
    public float Range = 300f;
    void Start()
    {
        BulletPosition = GetComponent<Transform>();
    }
    void Update()
    {
        //Jesli kula wyleci dalej niz ustalony zasieg, w osi X/Y/Z to zostaje zniszczona
        if (BulletPosition.position.x > Range || BulletPosition.position.x < -Range || BulletPosition.position.y > Range ||
            BulletPosition.position.y < -Range || BulletPosition.position.z > Range || BulletPosition.position.z < -Range)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Enemy")
        {
            Debug.Log(collision.transform.name);
            Destroy(collision.gameObject);
        }
        Destroy(gameObject);
    }
}
