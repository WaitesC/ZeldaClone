using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using Random = UnityEngine.Random;


public class EnemyAI : MonoBehaviour
{
    //players location
    public Transform Player;

    //speed of enemy movement
    public float MoveSpeed = 4;
    public float MaxDist = 10;
    public float MinDist = 5;

    public float followDistance;


    //animator for enemy movement
    Animator enemyAnimator;

    //var states = [FollowPlayer, Idle];

    //state change stuff
    float stateTime = 0f;
    float attackTime = 0f;
    public float nextStateTime = 2f;
    public float nextAttackTime = 2f;
    int currentFollowState;
    int currentAttackState;
    int maxFollowStates = 2;
    int maxAttackStates = 2;

    //enemy AI bools
    bool idle;
    bool followPlayer;
    bool attackPlayer;

    //attack stuff
    public LayerMask playerLayers;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public int attackPower;


    void Start()
    {
        //setting relevant components
        enemyAnimator = gameObject.GetComponent<Animator>();



    }


    void Update()
    {
        if (Vector3.Distance(transform.position, Player.position) >= followDistance)
            FollowPlayerStatesChanger();
        else if (Vector3.Distance(transform.position, Player.position) < followDistance)
            AttackPlayerStatesChanger();
        else
            transform.LookAt(Player);


    }

    void FollowPlayerStatesChanger()
    {
        stateChecker();

        currentFollowState = Random.Range(0, maxFollowStates);

        if (Time.time >= stateTime)
        {
            cancelBools();

            stateTime = Time.time + 1f / nextStateTime;

            FollowPlayerStates();
        }
    }

    void AttackPlayerStatesChanger()
    {
        stateChecker();

        currentAttackState = Random.Range(0, maxAttackStates);

        if (Time.time >= stateTime)
        {
            cancelBools();

            stateTime = Time.time + 1f / nextStateTime;

            AttackPlayerStates();
        }

    }

    void FollowPlayer()
    {
        //play walking animation
        enemyAnimator.

        transform.LookAt(Player);

        if (Vector3.Distance(transform.position, Player.position) >= MinDist)
        {

            transform.position += transform.forward * MoveSpeed * Time.deltaTime;



            if (Vector3.Distance(transform.position, Player.position) <= MaxDist)
            {
                //Here Call any function U want Like Shoot at here or something
            }

        }
    }

    void FollowPlayerStates()
    {
        //different enemy AI states
        switch (currentFollowState)
        {
            case 0:
                idle = true;
                break;
            case 1:
                followPlayer = true;
                break;
            default: break;
        }
    }
    
    
    void AttackPlayerStates()
    {
        //different enemy AI states
        switch (currentAttackState)
        {
            case 0:
                idle = true;
                break;
            case 1:
                attackPlayer = true;
                break;
            default: break;
        }
    }

    void Idle()
    {
        transform.LookAt(Player);
    }

    void AttackPlayer()
    {
        transform.LookAt(Player);

        if (Time.time >= attackTime)
        {
            cancelBools();

            Collider[] hitPlayers = Physics.OverlapSphere(attackPoint.position, attackRange, playerLayers);

            foreach(Collider player in hitPlayers)
            {
                player.GetComponent<PlayerHealth>().TakeDamage(attackPower);
                Debug.Log("Attack Player");

            }

            attackTime = Time.time + 1f / nextAttackTime;

        }
    }

    void stateChecker()
    {
        if (idle == true)
            Idle();
        if (followPlayer == true)
            FollowPlayer();
        if (attackPlayer == true)
            AttackPlayer();
    }

    void cancelBools()
    {
        idle = false;
        followPlayer = false;
        attackPlayer = false;
    }
}
