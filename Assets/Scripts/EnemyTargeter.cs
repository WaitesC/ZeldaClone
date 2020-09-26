using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTargeter : MonoBehaviour
{
    public GameObject closestEnemy;
    public float closestEnemyDistance;

    public float lockOnDistance = 5;
    public bool lockedOnToEnemy;


    void Start()
    {
        
    }

    void Update()
    {
        closestEnemy = FindClosestEnemy();
        closestEnemyDistance = DistancePlayerEnemy();

        if(DistancePlayerEnemy() < lockOnDistance)
        {
            if (Input.GetAxis("Lock On") != 0f)
            {
                FindClosestEnemy().gameObject.transform.localScale = new Vector3(1, 2, 1);
                lockedOnToEnemy = true;
            }

            if (Input.GetAxis("Lock On") != 1f)
            {
                FindClosestEnemy().gameObject.transform.localScale = new Vector3(1, 1, 1);
                lockedOnToEnemy = false;
            }
        }

        if (DistancePlayerEnemy() > lockOnDistance)
        {

            FindClosestEnemy().gameObject.transform.localScale = new Vector3(1, 1, 1);
            lockedOnToEnemy = false;
            //
        }
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

}
