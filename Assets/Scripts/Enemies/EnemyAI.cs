using System;
using UnityEngine;
using UnityEngine.AI;
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

    [Header("States Variables")] 
    public float SightRange, AttackRange;
    public bool PlayerInSightRange, PlayerInAttackRange;


    private void Awake()
    {
        Player = GameObject.Find("Player").transform;
        Agent = GetComponent<NavMeshAgent>();
    }
    
    private void Update()
    {
        // Check for sight and attack range
        PlayerInSightRange = Physics.CheckSphere(transform.position, SightRange, WhatIsPlayer);
        PlayerInAttackRange = Physics.CheckSphere(transform.position, AttackRange, WhatIsPlayer);

        if (PlayerMovement2.IsPlayerInMove)
        {
            if (!PlayerInSightRange && !PlayerInAttackRange)
                Patrol();
            if (PlayerInSightRange && !PlayerInAttackRange)
                Chase();
            if (PlayerInSightRange && PlayerInAttackRange)
                Attack();
        }
    }

    private void Patrol()
    {
        if (!walkPointSet)
            SearchWalkPoint();

        if (walkPointSet)
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
        Agent.SetDestination(Player.position);
    }

    private void Attack()
    {
        // Make sure enemy doesn't move
        Agent.SetDestination(transform.position);
        transform.LookAt(Player);
    }
}
