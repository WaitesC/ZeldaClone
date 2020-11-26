using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyWaveSpawner : MonoBehaviour
{
    public GameObject[] enemies;

    public int waveNum;
    

    Text WaveNumberText;
    Text WaveNumberTextSmall;
    //GameObject WaveNumberTextObject;
    public Color see;
    public Color unSee;

    Animator enemyAnimator;

    GameObject[] enemiesLeft;

    bool canCheckForEnemies;

    // Start is called before the first frame update
    void Start()
    {
        canCheckForEnemies = false;

        waveNum = 1;

        WaveNumberText = GameObject.Find("WaveNumberText").GetComponent<Text>();
        WaveNumberTextSmall = GameObject.Find("WaveNumberTextSmall").GetComponent<Text>();
        //WaveNumberTextObject = GameObject.Find("WaveNumberText");

        //NewWave();

        StartCoroutine(SpawnEnemies());

    }

    void NewWave()
    {
        WaveNumberText.color = see;
        WaveNumberTextSmall.color = unSee;

        //SpawnEnemies();

        //Invoke("SpawnEnemies", 2.0f);

        StartCoroutine(SpawnEnemies());

    }


    IEnumerator SpawnEnemies()
    {
        canCheckForEnemies = false;

        WaveNumberText.color = see;
        WaveNumberTextSmall.color = unSee;


        yield return new WaitForSeconds(2);

        canCheckForEnemies = true;

        WaveNumberText.color = unSee;
        WaveNumberTextSmall.color = see;


        GameObject enemy = enemies[Random.Range(0, enemies.Length)];
        Vector3[] spawnPositions = new[] { new Vector3(-15, 11, 13), new Vector3(-15, 11, 10), new Vector3(-13, 11, 10) };

        Quaternion spawnRotation = Quaternion.identity;
        for (int i = 1; i < 3; i++)
        {
            Instantiate(enemy, spawnPositions[i], spawnRotation);
        }

    }

    void CheckForEnemies()
    {
        //GameObject[] enemiesLeft = GameObject.FindGameObjectsWithTag("Enemy");

        if (enemiesLeft.Length == 1 && canCheckForEnemies == true)
        //if (enemiesLeft.Length == 1)
        {
            waveNum += 1;
            StartCoroutine(SpawnEnemies());
            //NewWave();

        }



    }

    

    // Update is called once per frame
    void Update()
    {

        enemiesLeft = GameObject.FindGameObjectsWithTag("Enemy");

        WaveNumberText.text = "Wave " + waveNum;
        WaveNumberTextSmall.text = "Wave " + waveNum;

        CheckForEnemies();
    }
}
