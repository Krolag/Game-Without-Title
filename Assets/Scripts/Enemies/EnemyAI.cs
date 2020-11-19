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
    [Header("Initialize enemy")] public NavMeshAgent Agent;
    public Transform Player;
    public LayerMask WhatIsGround, WhatIsPlayer;

    [Header("Patroling variables")] public Vector3 WalkPoint;
    public float WalkPointRange;
    private bool walkPointSet;
    private Vector3 playerPosition;
    private Vector3 currentPosition;

    [Header("States Variables")] public float SightRange, AttackRange;
    public bool PlayerInSightRange, PlayerInAttackRange = true;
    public float FovAngle;

    private void Awake()
    {
        Player = GameObject.Find("Player").transform;
        Agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        // Check for sight and attack range
        // Check for sight and attack range
        RaycastHit hit;

        float angle = FovAngle;
        float step = (FovAngle * 2) / 30;
        for (int i = 0; i < 30; i++)
        {

            Debug.DrawRay(transform.position,
                transform.TransformDirection(Quaternion.AngleAxis(angle, transform.up) * transform.forward) *
                SightRange, Color.red);

            if (Physics.Raycast(transform.position,
                transform.TransformDirection(Quaternion.AngleAxis(angle, transform.up) * transform.forward), out hit,
                SightRange))
            {
                if (hit.collider.name == "Player")
                    PlayerInSightRange = true;
            }

            angle -= step;

        }

        // PlayerInSightRange = Physics.CheckSphere(transform.position, SightRange, WhatIsPlayer);
        // PlayerInAttackRange = Physics.CheckSphere(transform.position, AttackRange, WhatIsPlayer);
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
            if (!PlayerInSightRange && !PlayerInAttackRange)
                Patrol();
            // if (PlayerInSightRange && !PlayerInAttackRange)
            //     Chase();
            if (PlayerInSightRange && PlayerInAttackRange)
                Attack();
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

        StartCoroutine(CheckIfStuck());
    }

    IEnumerator CheckIfStuck()
    {
        currentPosition = transform.position;
        yield return new WaitForSeconds(2);
        // Debug.Log(Vector3.Distance(currentPosition, transform.position));
        if (Vector3.Distance(currentPosition, transform.position) <= 0.1)
           SearchWalkPoint();
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
