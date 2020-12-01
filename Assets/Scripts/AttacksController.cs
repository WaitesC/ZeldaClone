using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttacksController : MonoBehaviour
{
    //players animator
    public Animator playerAnimator;

    public GameObject attackParticle;

    //attack range and position
    public Transform attackPoint;
    public float attackRange = 0.5f;

    //attack damage
    public int attackPower;
    public int attackPower2;

    //enemy layers
    public LayerMask enemyLayers;

    //attack timings
    public float attackRate = 2f;
    public float attackRate2 = 2f;
    float nextAttackTime = 0f;
    public float attackDelay;
    public float attack2Delay;

    float speed = 1.0f;

    GameManager gameManager;
    ThirdPersonMovement thirdPersonMovement;
    EnemyTargeter enemyTargeter;

    //public float speed = 1.0f;

    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        thirdPersonMovement = GameObject.Find("Player").GetComponent<ThirdPersonMovement>();
        enemyTargeter = GetComponent<EnemyTargeter>();
        
    }

    void Update()
    {
        if (Time.time >= nextAttackTime)
        {
            gameManager.canMove = true;

            thirdPersonMovement.speed = 6;

        }


        if (gameManager.playerRetaliate)
        {
            if (Input.GetButtonDown("Attack"))
            {
                Attack();
            }
        }
            



        if (Time.time >= nextAttackTime)
        {
            if (Input.GetButtonDown("Attack"))
            {
                gameManager.canMove = false;
                
                thirdPersonMovement.speed = 0;
                Attack();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
        
        if (Time.time >= nextAttackTime)
        {
            if (Input.GetButtonDown("Attack2"))
            {
                gameManager.canMove = false;
                thirdPersonMovement.speed = 0;

                Attack2();
                nextAttackTime = 1.8f + Time.time + 1f / attackRate;
            }
        }

        //if(gameManager.dodging)
        //{
        //    float step = speed * Time.deltaTime; // calculate distance to move
        //    if (Input.GetButtonDown("Attack2"))
        //    {
        //        transform.position = Vector3.MoveTowards(transform.position, enemyTargeter.FindClosestEnemy().transform.position, step);
        //        Attack2();
        //    }
        
        //    if (Input.GetButtonDown("Attack"))
        //    {
        //        transform.position = Vector3.MoveTowards(transform.position, enemyTargeter.FindClosestEnemy().transform.position, step);
        //        Attack();
        //    }

        //}
    }

    //basic attack function
    void Attack()
    {
        //play attack anim
        playerAnimator.SetTrigger("Attack");

        FaceTarget();

        StartCoroutine(AttackDamage());
    }

    IEnumerator AttackDamage()
    {
        yield return new WaitForSeconds(attackDelay);
        //detect enemies in range
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayers);

        //damage enemies
        foreach (Collider enemy in hitEnemies)
        {
            enemy.GetComponent<UnitStats>().TakeDamage(attackPower);

            enemy.GetComponent<EnemyAI>().enemyAnimator.SetTrigger("Hurt");

            Instantiate(attackParticle, enemy.transform.position, Quaternion.identity);
        }

    }


    //heavy attack
    void Attack2()
    {
        //play attack anim
        playerAnimator.SetTrigger("Attack2");

        FaceTarget();

        StartCoroutine(Attack2Damage());
    }

    IEnumerator Attack2Damage()
    {
        yield return new WaitForSeconds(attack2Delay);
        //detect enemies in range
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayers);

        //damage enemies
        foreach (Collider enemy in hitEnemies)
        {
            enemy.GetComponent<UnitStats>().TakeDamage(attackPower2);

            enemy.GetComponent<EnemyAI>().enemyAnimator.SetTrigger("Hurt");

            Instantiate(attackParticle, enemy.transform.position, Quaternion.identity);

        }

    }

    void FaceTarget()
    {
        //float step = speed * Time.deltaTime; // calculate distance to move

        if (Vector3.Distance(enemyTargeter.FindClosestEnemy().transform.position, transform.position) <= attackRange)
        {
            transform.LookAt(enemyTargeter.FindClosestEnemy().transform);

            //transform.position = Vector3.MoveTowards(transform.position, enemyTargeter.FindClosestEnemy().transform.position, step);
        }

    }

}
