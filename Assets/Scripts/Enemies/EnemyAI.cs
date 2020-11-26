using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

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
    private Vector3 playerPosition;
    private Vector3 currentPosition;

    [Header("States Variables")] 
    public bool PlayerInSightRange;
    public float SightRange;
    public int RayCastsCount;
    public float HeightMultiplier;
    public float FieldOfView;
    
    

    private void Awake()
    {
        Player = GameObject.Find("Player").transform;
        Agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        Sight();
        
        playerPosition = Player.position;

        if (!PlayerMovement2.IsPlayerInMove)
        {
            Agent.velocity = Vector3.zero;
            Agent.Stop();
            Agent.enabled = false;
            Agent.enabled = true;
        }
        else
        {
            if (!PlayerInSightRange)
                Patrol();
            else
                Attack();
        }
    }

    private void Sight()
    {
        RaycastHit hit;

        var origin = transform.position + Vector3.up * HeightMultiplier;
        var direction = (transform.forward - transform.right).normalized;
        var angleStep = Quaternion.AngleAxis(FieldOfView / RayCastsCount, Vector3.up);

        for (int i = 0; i < RayCastsCount; i++)
        {
            Debug.DrawRay(origin, direction * SightRange, Color.red);

            if (Physics.Raycast(origin, direction, out hit, SightRange))
            {
                if (hit.collider.name == "Player")
                    PlayerInSightRange = true;
            }

            direction = angleStep * direction;
        }
    }

    private void Patrol()
    {
        if (!walkPointSet)
            SearchWalkPoint();

        if (walkPointSet && !PlayerMovement2.IsPlayerInMove)
        {
            Agent.velocity = Vector3.zero;
            Agent.Stop();
            Agent.enabled = false;
            Agent.enabled = true;
        }

        if (walkPointSet && PlayerMovement2.IsPlayerInMove)
            Agent.SetDestination(WalkPoint);

        Vector3 distanceToWalkPoint = transform.position - WalkPoint;

        // Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }

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
        if (PlayerMovement2.IsPlayerInMove)
            Agent.SetDestination(playerPosition);
        if (!PlayerMovement2.IsPlayerInMove)
        {
            Agent.velocity = Vector3.zero;
            Agent.Stop();
            Agent.enabled = false;
            Agent.enabled = true;
        }
    }

    private void Attack()
    {
        // Make sure enemy doesn't move
        Agent.SetDestination(transform.position);
        transform.LookAt(Player);
        SceneManager.LoadScene("Level_01", LoadSceneMode.Single);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Harmful"))
            Destroy(this.gameObject);
    }
}
