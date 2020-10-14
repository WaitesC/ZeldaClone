using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTargeter : MonoBehaviour
{
    GameObject closestEnemy;
    float closestEnemyDistance;

    public float lockOnDistance = 5;
    public bool lockedOnToEnemy;

    public GameObject Target;
    public GameObject Player;

    public GameObject lockOnIcon;
    public GameObject enemyHealthBarIcon;

    public EnemyHealthBar enemyHealthBar;

    //enemy stats
    float enemyMaxHealth;

    void Start()
    {
        
    }

    void Update()
    {
        LockOnToEnemy();

        //Debug.Log(EnemyCurrentHealth());
    }

    //finds closest enemy
    public GameObject FindClosestEnemy()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;

        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;

    }

    //finds distance between player and closest enemy
    public float DistancePlayerEnemy()
    {
        float enemyDistance = Vector3.Distance(FindClosestEnemy().transform.position, transform.position);


        return enemyDistance;
    }

    //find current enemy health
    public float EnemyCurrentHealth()
    {
        float enemyCurrentHealth = FindClosestEnemy().GetComponent<UnitStats>().currentHealth;

        return enemyCurrentHealth;

    }
    
    //find enemy max health
    public float EnemyMaxHealth()
    {
        float enemyMaxHealth = FindClosestEnemy().GetComponent<UnitStats>().maxHealth;

        return enemyMaxHealth;

    }

    //locks camera to follow enemy
    void LockOnToEnemy()
    {
        closestEnemy = FindClosestEnemy();
        closestEnemyDistance = DistancePlayerEnemy();

        //if distance is low enough player will lock onto enemy
        if (DistancePlayerEnemy() < lockOnDistance)
        {
            if (Input.GetAxis("Lock On") != 0f)
            {
                lockedOnToEnemy = true;
                StartLockOnFunction();
            }

            if (Input.GetAxis("Lock On") != 1f)
            {
                lockedOnToEnemy = false;
                EndLockOnFunction();
            }
        }

        //if player moves too far away lock on will cancel
        if (DistancePlayerEnemy() > lockOnDistance)
        {
            lockedOnToEnemy = false;
            EndLockOnFunction();
            
        }

        //moves camera target to enemy if locked on
        if (lockedOnToEnemy)
            Target.gameObject.transform.position = FindClosestEnemy().gameObject.transform.position;
        else
            Target.gameObject.transform.position = Player.gameObject.transform.position;

    }

    void StartLockOnFunction()
    {
        //FindClosestEnemy().gameObject.transform.localScale = new Vector3(1, 2, 1);

        lockOnIcon.SetActive(true);
        enemyHealthBarIcon.SetActive(true);


        SetEnemyHealthBar();

    }

    public void EndLockOnFunction()
    {
        //FindClosestEnemy().gameObject.transform.localScale = new Vector3(1, 1, 1);

        lockOnIcon.SetActive(false);
        enemyHealthBarIcon.SetActive(false);
        Target.gameObject.transform.position = Player.gameObject.transform.position;
        lockedOnToEnemy = false;

    }

    void SetEnemyHealthBar()
    {
        enemyHealthBar.SetCurrentHealth(EnemyCurrentHealth());
        enemyHealthBar.SetMaxHealth(EnemyMaxHealth());
    }
}
