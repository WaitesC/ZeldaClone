using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttacksController : MonoBehaviour
{
    //players animator
    public Animator playerAnimator;

    //attack range and position
    public Transform attackPoint;
    public float attackRange = 0.5f;

    //enemy layers
    public LayerMask enemyLayers;

    //attack timings
    public float attackRate = 2f;
    float nextAttackTime = 0f;


    void Update()
    {
        if(Time.time >= nextAttackTime)
        {
            if (Input.GetButtonDown("Attack"))
            {
                Attack();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }

        
    }

    //basic attack function
    void Attack()
    {
        //play attack anim
        playerAnimator.SetTrigger("Attack");
        //detect enemies in range
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayers);

        //damage enemies
        foreach(Collider enemy in hitEnemies)
        {
            enemy.GetComponent<UnitStats>().TakeDamage(GetComponent<UnitStats>().attackPower);
            Debug.Log("Player Normal Attack");
        }
    }

    
}
