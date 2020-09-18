using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCharacterController : MonoBehaviour
{
    public float Speed;
    float speedMultiplier = 1;

    public float lockOnDistance;

    public bool lockedOnToEnemy;
    //public float enemyDistance = 5;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();

        SprintCheck();

        //FindClosestEnemies();

        //FindClosestEnemy().gameObject.transform.localScale += new Vector3(0, 2, 0);
        //Debug.Log(distancePlayerEnemy());


        //if player is within lock on distance to an enemy
        if (distancePlayerEnemy() < lockOnDistance)
        {

            if (Input.GetAxis("Fire2") != 0f)
            {
                FindClosestEnemy().gameObject.transform.localScale = new Vector3(1, 2, 1);
                lockedOnToEnemy = true;
            }

            if (Input.GetAxis("Fire2") != 1f)
            {
                FindClosestEnemy().gameObject.transform.localScale = new Vector3(1, 1, 1);
                lockedOnToEnemy = false;
            }
        }

        if (distancePlayerEnemy() >= lockOnDistance)
        {

            FindClosestEnemy().gameObject.transform.localScale = new Vector3(1, 1, 1);
            lockedOnToEnemy = false;
            //
        }

    }

    void PlayerMovement()
    {
        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");
        Vector3 playerMovement = new Vector3(hor, 0f, ver) * Speed * speedMultiplier * Time.deltaTime;
        transform.Translate(playerMovement, Space.Self);
    }

    void SprintCheck()
    {
        if (Input.GetAxis("Fire3") != 0f)
        {
            speedMultiplier = 5;
        }

        if (Input.GetAxis("Fire3") != 1f)
        {
            speedMultiplier = 1;
        }
    }

    //finds all game objects near player that have the tag "Enemy" within enemyDistance
    //void FindClosestEnemies()
    //{
    //    GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag("Enemy");

    //    for (int i = 0; i < taggedObjects.Length; i++)
    //    {
    //        if (Vector3.Distance(transform.position, taggedObjects[i].transform.position) <= enemyDistance)
    //        {
    //            //Debug.Log("1");
    //        }
    //    }
    //}


    //finds the game object of the closest enemy
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

    public float distancePlayerEnemy()
    {
        float enemyDistance = Vector3.Distance(FindClosestEnemy().transform.position, transform.position);


        return enemyDistance;
    }
}
