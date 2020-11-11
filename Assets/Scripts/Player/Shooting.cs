using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Transform SpawnPoint;
    public GameObject Bullet;

    //Predkosc kul
    public float BulletSpeed = 20f;
    
    //Celowanie
    public Transform InitialPosition;
    public Transform AimPosition;
    private bool isAiming = false;

    public Transform ShotRotation;
    public Transform IdleRotation;

    void Start()
    {
        transform.position = InitialPosition.position;
        transform.rotation = IdleRotation.rotation;
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0)) 
            Shot();
        else if (Input.GetMouseButtonUp(0))
            transform.rotation = IdleRotation.rotation;
        
        if(Input.GetMouseButtonDown(1) && !isAiming)
            Aim();
        else if(Input.GetMouseButtonUp(1) && isAiming)
            UnAim();
    }

    private void Shot()
    {
        //Tworzenie kuli
        GameObject bullet = Instantiate(Bullet, SpawnPoint.position, Bullet.transform.rotation);
        Rigidbody rig = bullet.GetComponent<Rigidbody>();
        
        //Nadanie predkosci wylotowej
        rig.AddForce(SpawnPoint.forward * BulletSpeed, ForceMode.Impulse);
        transform.rotation = ShotRotation.rotation;
    }
    
    //Celowanie
    private void Aim()
    {
        transform.position = AimPosition.position;
        isAiming = true;
    }

    private void UnAim()
    {
        transform.position = InitialPosition.position;
        isAiming = false;
    }
}
