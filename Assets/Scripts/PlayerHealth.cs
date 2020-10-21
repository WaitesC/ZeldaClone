using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public UnitStats playerStats;
    public EnemyHealthBar playerHealthBar;


    // Start is called before the first frame update
    void Start()
    {
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
