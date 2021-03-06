﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    UnitStats playerStats;
    EnemyHealthBar playerHealthBar;

    public GameObject gameOverScreen;
    public GameObject winScreen;

    bool deadMeat;

    GameManager gameManager;
    ThirdPersonMovement thirdPersonMovement;


    // Start is called before the first frame update
    void Start()
    {
        playerStats = GameObject.Find("Player").GetComponent<UnitStats>();
        playerHealthBar = GameObject.Find("Player Health Bar Fill").GetComponent<EnemyHealthBar>();
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        thirdPersonMovement = GameObject.Find("Player").GetComponent<ThirdPersonMovement>();


        //sets value of current health
        playerStats.currentHealth = playerStats.maxHealth;
        //sets current health in health bar
        playerHealthBar.SetCurrentHealth(playerStats.currentHealth);
        //sets max health in health bar
        playerHealthBar.SetMaxHealth(playerStats.maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        //function to test if damage works correctly
        DamageTester();

        if (playerStats.currentHealth < 0)
        {
            deadMeat = true;
            gameOverScreen.SetActive(true);
            Time.timeScale = 0.0f;
            gameManager.canMove = false;
            gameManager.paused = true;
            gameManager.deadMeat = true;
            //thirdPersonMovement.targetAngle = 0;
            //thirdPersonMovement.targetAngle = 0;
            //transform.rotation = Quaternion.identity;


        }

        if (deadMeat && Input.GetButtonDown("Jump"))
        {
            //reload scene;
            //gameManager.paused = false;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            Time.timeScale = 1;
        }
    }

    public void Win()
    {
        deadMeat = true;
        winScreen.SetActive(true);
        Time.timeScale = 0.0f;
        gameManager.canMove = false;
        gameManager.paused = true;
        gameManager.deadMeat = true;
    }

    public void TakeDamage(int damage)
    {
        playerStats.currentHealth -= damage;

        playerHealthBar.SetCurrentHealth(playerStats.currentHealth);
    }

    

    void DamageTester()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TakeDamage(200);
        }
    }
}
