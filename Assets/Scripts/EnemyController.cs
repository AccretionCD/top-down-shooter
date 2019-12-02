﻿using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] float health = 5f;
    bool dead;
    public event System.Action OnDeath;

    [SerializeField] float targetTrackDelay = 0.2f;
    NavMeshAgent navMeshAgent;
    PlayerController target;
    bool hasTarget;

    [SerializeField] float attackRange = 3f;
    [SerializeField] float attackSpeed = 2f;
    [SerializeField] float attackCooldown = 1f;
    float timeToAttack;

    Material material;
    Color materialColor;

    public enum State {Idle, Moving, Attacking}
    State currentState;

    void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        target = FindObjectOfType<PlayerController>();
        material = GetComponent<MeshRenderer>().material;
        materialColor = material.color;
    }

    void Start()
    {
        if (target != null)
        {
            hasTarget = true;
            currentState = State.Moving;

            target.OnDeath += OnTargetDeath;

            StartCoroutine(UpdatePath());
        } 
    }

    void OnTargetDeath()
    {
        hasTarget = false;
        currentState = State.Idle;
    }

    IEnumerator UpdatePath()
    {
        while (hasTarget)
        {
            if (currentState == State.Moving)
            { 
                Vector3 targetPosition = new Vector3(target.transform.position.x, 0, target.transform.position.z);
                navMeshAgent.SetDestination(targetPosition);
            }
            
            yield return new WaitForSeconds(targetTrackDelay);
        }
    }

    void Update()
    {
        Attack();
    }

    void Attack()
    {
        if (hasTarget)
            if (Time.time > timeToAttack)
                if (Vector3.Distance(target.transform.position, transform.position) <= attackRange)
                {
                    timeToAttack = Time.time + attackCooldown;
                    StartCoroutine(StartAttack());
                }
    }

    IEnumerator StartAttack()
    {
        material.color = Color.red;
        currentState = State.Attacking;
        navMeshAgent.enabled = false;

        Vector3 startingPosition = transform.position;
        Vector3 targetPosition = target.transform.position;

        float attackProgress = 0f;

        while (attackProgress <= 1f)
        {
            attackProgress += Time.deltaTime * attackSpeed;
            float interpolation = 4 * (-attackProgress * attackProgress + attackProgress);
            transform.position = Vector3.Lerp(startingPosition, targetPosition, interpolation);
            
            yield return null;
        }

        material.color = materialColor;
        currentState = State.Moving;
        navMeshAgent.enabled = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bullet"))
            health--;

        if (health <= 0)
        {
            dead = true;

            OnDeath?.Invoke();

            Destroy(gameObject);
        }  
    }
}
