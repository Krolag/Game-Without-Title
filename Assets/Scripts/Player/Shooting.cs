using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Transform SpawnPoint;
    public GameObject Bullet;

    //Predkosc kul
    public float BulletSpeed = 20f;
    
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
            Shot();
    }

    private void Shot()
    {
        //Tworzenie kuli
        GameObject bullet = Instantiate(Bullet, SpawnPoint.position, Bullet.transform.rotation);
        Rigidbody rig = bullet.GetComponent<Rigidbody>();
        
        //Nadanie predkosci wylotowej
        rig.AddForce(SpawnPoint.forward * BulletSpeed, ForceMode.Impulse);
    }
}
