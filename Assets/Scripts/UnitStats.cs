﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStats : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;
    public int attackPower;

    EnemyTargeter enemyTargeter;
    
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;

        enemyTargeter = GameObject.Find("Player").GetComponent<EnemyTargeter>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        //play hurt animation

        if(currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        //play die animation

        //GetComponent<Collider>().enabled = false;
        //this.enabled = false;

        Time.timeScale = 1.0f;

        enemyTargeter.EndLockOnFunction();

        Destroy(gameObject);
    }
}
