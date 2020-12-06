using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Cryptography;
using UnityEngine;
using Random = UnityEngine.Random;


public class EnemyAI : MonoBehaviour
{
    //players info
    Transform Player;
    public float engageDist;
    float currentDist;
    ThirdPersonMovement thirdPersonMovement;
    PlayerHealth playerHealth;
    public AudioClip playerHurtSound;

    public GameObject attackParticle;



    //speed of enemy movement
    public float MoveSpeed = 4;
    public float MaxDist = 10;
    public float MinDist = 5;

    public float followDistance;

    GameManager gameManager;

    //animator for enemy movement
    public Animator enemyAnimator;

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
    public float attackDelay;

    //dodge stuff
    bool dodgeable;
    private float fixedDeltaTime;

    //ground check stuff
    public Transform groundCheck;
    public float groundDistance = 0.1f;
    public LayerMask groundMask;

    //gravity stuff
    Vector3 velocity;
    public float gravity = -9.81f;
    public bool isGrounded;
    public float jumpHeight = 3f;


    void Awake()
    {
        // Make a copy of the fixedDeltaTime, it defaults to 0.02f, but it can be changed in the editor
        this.fixedDeltaTime = Time.fixedDeltaTime;
    }

    



    void Start()
    {
        playerHealth = GameObject.Find("Player").GetComponent<PlayerHealth>();
        //ref to game manager
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        //setting relevant components
        enemyAnimator = gameObject.GetComponent<Animator>();

        Player = GameObject.Find("Player").GetComponent<Transform>();
        thirdPersonMovement = GameObject.Find("Player").GetComponent<ThirdPersonMovement>();

        engageDist = Vector3.Distance(Player.position, transform.position);

        enemyAnimator.Rebind();
        //enemyAnimator.enable = true;
        //enemyAnimator.Play("Enemy_Idle", 0, 0f);


    }


    void Update()
    {
        //Movement();
        //transform.Rotate(0, transform.rotation.y, transform.rotation.z, Space.World);

        currentDist = Vector3.Distance(Player.position, transform.position);

        if (currentDist < engageDist)
        {
            if (Vector3.Distance(transform.position, Player.position) >= followDistance)
                FollowPlayerStatesChanger();
            else if (Vector3.Distance(transform.position, Player.position) < followDistance)
                AttackPlayerStatesChanger();
            else
                LookAtPlayer();
        }

        DodgeCheck();

    }

    void Movement()
    {
        //sets to is grounded when touching anything with ground mask
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        Gravity();
    }

    void Gravity()
    {
        velocity.y += gravity * Time.deltaTime;
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
        //enemyAnimator.

        LookAtPlayer();

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
        LookAtPlayer();
    }

    void AttackPlayer()
    {
        LookAtPlayer();

        if (Time.time >= attackTime)
        {

            cancelBools();

            enemyAnimator.Play("Enemy_Attack1");

            dodgeable = true;

            Debug.Log("Now Dodgeable");

            Invoke("CounterWindow", 1f);


            StartCoroutine(AttackDamage());


            

            attackTime = Time.time + 1f / nextAttackTime;
        }
    }

    IEnumerator AttackDamage()
    {
        yield return new WaitForSeconds(attackDelay);

        Collider[] hitPlayers = Physics.OverlapSphere(attackPoint.position, attackRange, playerLayers);

        foreach (Collider player in hitPlayers)
        {
            //enemyAnimator.SetTrigger("Attack");

            player.GetComponent<Animator>().Play("Hurt");
            player.GetComponent<AudioSource>().PlayOneShot(playerHurtSound, 0.3f);
            Instantiate(attackParticle, player.transform.position, Quaternion.identity);

            playerHealth.TakeDamage(attackPower);
            //Debug.Log("Attack Player");

            

        }



    }

    private void CounterWindow()
    {
        dodgeable = false;
        Debug.Log("Not Dodgeable");

    }

    void DodgeCheck()
    {
        if (Input.GetButtonDown("Dodge") && thirdPersonMovement.isGrounded && dodgeable)
            gameManager.SlowDown();

    }

    

    void stateChecker()
    {
        if (idle == true)
            Idle();

        if (followPlayer == true)
        {
            idle = false;
            FollowPlayer();
        }

        if (attackPlayer == true)
        {
            idle = false;
            AttackPlayer();
        }
    }

    void cancelBools()
    {
        followPlayer = false;
        attackPlayer = false;
        idle = true;
    }

    //funciton to rotate towards player
    void LookAtPlayer()
    {
        transform.LookAt(new Vector3(Player.position.x, transform.position.y, Player.position.z));

    }
}
