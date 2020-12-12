using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Killable))]
public class EnemyAI : MonoBehaviour
{
    [Header("Initialize enemy")]
    public NavMeshAgent Agent;
    public Transform Player;
    public LayerMask WhatIsGround, WhatIsPlayer;

    [Header("Patroling variables")]
    public Vector3 WalkPoint;
    public float WalkPointRange;
    private bool walkPointSet;
    // private Vector3 playerPosition;
    private Vector3 currentPosition;

    [Header("States Variables")]
    public bool PlayerInSightRange;
    public float SightRange;
    public int RayCastsCount;
    public float HeightMultiplier;
    public float FieldOfView;

    // Zmienne od Pana Ignacego
    public float Awareness = 0f; // Zmienna definiujaca to jak bardzo dany enemy jest "swiadomy" naszej obecnosci
    public float AwarenessTime = 5f;
    public PlayerMovement2 PlayerObject;
    
    // Zmienne od Pana Jakuba
    public AwarenessBar AwarenessBar;
    
    //public int EnemyCounter;
    
    // private void Awake()
    // {
    //     Player = GameObject.Find("Player").transform;//dont search by name 
    //     Agent = GetComponent<NavMeshAgent>();
    // }

    private void Start()
    {
        AwarenessBar.SetMaxAwareness(100);
    }

    private void OnEnable()
    {
        GetComponent<Killable>().Death += OnDeath;
    }

    private void OnDisable()
    {
        GetComponent<Killable>().Death -= OnDeath;
    }

    private void Update()
    {
        // Sight();

        // playerPosition = Player.position;

        // if (!PlayerMovement2.IsPlayerInMove)
        // {
        //     Agent.velocity = Vector3.zero;
        //     Agent.Stop();
        //     Agent.enabled = false;
        //     Agent.enabled = true;
        // }
        // else
        // {
            if (!PlayerInSightRange) // wander state
            {
                // Patrol();
                if (Awareness > 20 && Awareness < 80)
                {
                    Awareness -= 1.5f * Time.deltaTime;
                    if (Awareness < 25)
                    {
                        SightRange = 30f;
                        RayCastsCount = 30;
                    }
                }
            }
            
            else if (PlayerInSightRange) // investigation state
                // Jesli przeciwnik raz nas zauwazy to caly czas nas widzi ??DLAczEEgooo?? ;_;
            {
                EnemyAwareness();
                PlayerInSightRange = !PlayerInSightRange; // Tymczasowo
            }
        // }
    }

    // private void AwarnessDecrease()
    // { 
    //     float startTime = Time.time;
    //     while (Time.time < startTime + AwarenessTime + 1)
    //         if (Time.time > startTime + AwarenessTime)
    //         {
    //             Awareness = 20f;
    //             SightRange = 30f;
    //             RayCastsCount = 30;
    //             break;
    //         }
    // }
    
    private void EnemyAwareness()
    {
        if (PlayerMovement2.isCrouching == true)
        {
            Awareness += 25f * Time.deltaTime;
        }
        else if (PlayerMovement2.isSprinting == true)
        {
            Awareness += 50f * Time.deltaTime;
        }
        else
            Awareness += 40f * Time.deltaTime;
        
        if (Awareness >= 30f)
        {
            SightRange = 40f;
            RayCastsCount = 40;
            // Patrol();
        }
        if (Awareness >= 50f)
        {
            Agent.speed = 6f;
            Agent.acceleration = 10f;
            // Patrol();
        }
        if (Awareness >= 90f)
        {
            Agent.speed = 9f;
            Agent.acceleration = 12f;
            Chase();
        }
        if (Awareness >= 100f)
        {
            Awareness = 100f;
            //Zabija nas dopiero wtedy gdy nas dogoni
            if ((Math.Abs(Agent.transform.position.x - Player.transform.position.x) < 5.0f) && (Math.Abs(Agent.transform.position.y - Player.transform.position.y) < 5.0f) && (Math.Abs(Agent.transform.position.z - Player.transform.position.z) < 5.0f)) 
            {
                Attack();
            }
        }
        AwarenessBar.setAwareness(Awareness);
    }
    
    // private void Sight()
    // {
    //     RaycastHit hit;
    //
    //     var origin = transform.position + Vector3.up * HeightMultiplier;
    //     var direction = (transform.forward - transform.right).normalized;
    //     var angleStep = Quaternion.AngleAxis(FieldOfView / RayCastsCount, Vector3.up);
    //
    //     for (int i = 0; i < RayCastsCount; i++)
    //     {
    //         Debug.DrawRay(origin, direction * SightRange, Color.red);
    //
    //         if (Physics.Raycast(origin, direction, out hit, SightRange))
    //         {
    //             if (hit.collider.name == "Player") //dont search gameobjects by name
    //                 PlayerInSightRange = true; // Tutaj jest ustawiana wartosc PlayerInSightRange na true, ale pozniej po "wyjsciu" nie jest cofana na false :<
    //         }
    //
    //         direction = angleStep * direction;
    //     }
    // }

    // private void Patrol()
    // {
    //     if (!walkPointSet)
    //         SearchWalkPoint();
    //
    //     if (walkPointSet && !PlayerMovement2.IsPlayerInMove)
    //     {
    //         Agent.velocity = Vector3.zero;
    //         Agent.Stop();
    //         Agent.enabled = false;
    //         Agent.enabled = true;
    //     }
    //
    //     if (walkPointSet && PlayerMovement2.IsPlayerInMove)
    //         Agent.SetDestination(WalkPoint);
    //
    //     Vector3 distanceToWalkPoint = transform.position - WalkPoint;
    //
    //     // Walkpoint reached
    //     if (distanceToWalkPoint.magnitude < 1f)
    //         walkPointSet = false;
    // }

    private void SearchWalkPoint()
    {
        // Calculate random point in given range
        float randomZ = Random.Range(-WalkPointRange, WalkPointRange);
        float randomX = Random.Range(-WalkPointRange, WalkPointRange);

        WalkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(WalkPoint, -transform.up, 2f, WhatIsGround))
            walkPointSet = true;
    }

    private void Chase()
    {
        // if (PlayerMovement2.IsPlayerInMove)
        //     Agent.SetDestination(playerPosition);
        // if (!PlayerMovement2.IsPlayerInMove)
        // {
        //     Agent.velocity = Vector3.zero;
        //     Agent.Stop();
        //     Agent.enabled = false;
        //     Agent.enabled = true;
        // }
    }

    private void Attack()
    {
        // Make sure enemy doesn't move
        Agent.SetDestination(transform.position);
        transform.LookAt(Player);
        SceneManager.LoadScene("Level_01", LoadSceneMode.Single);
    }

    private void OnDeath()
    {
        Destroy(this.gameObject);
        temporary.EnemiesLeft--;//xd

    }
    /*private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Harmful"))
        {
            Destroy(other.gameObject);
        }
    }*/
}
