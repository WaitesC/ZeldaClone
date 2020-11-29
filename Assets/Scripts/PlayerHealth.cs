using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    UnitStats playerStats;
    EnemyHealthBar playerHealthBar;

    public GameObject gameOverScreen;

    bool deadMeat;

    // Start is called before the first frame update
    void Start()
    {
        playerStats = GameObject.Find("Player").GetComponent<UnitStats>();
        playerHealthBar = GameObject.Find("Player Health Bar Fill").GetComponent<EnemyHealthBar>();


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
        //DamageTester();

        if(playerStats.currentHealth <0)
        {
            deadMeat = true;
            gameOverScreen.SetActive(true);
            Time.timeScale = 0;
        }

        if(deadMeat && Input.GetButtonDown("Jump"))
        {
            //reload scene;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            Time.timeScale = 1;
        }
    }

    public void TakeDamage(int damage)
    {
        playerStats.currentHealth -= damage;

        playerHealthBar.SetCurrentHealth(playerStats.currentHealth);
    }

    

    void DamageTester()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(20);
        }
    }
}
