using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyWaveSpawner : MonoBehaviour
{
    public GameObject[] enemies;
    public GameObject[] enemies2;

    public int waveNum;

    public float x, y, z;

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
        Vector3[] spawnPositions = new[] { new Vector3(x, y, z), new Vector3(x + 1, y, z) };

        Quaternion spawnRotation = Quaternion.identity;
        for (int i = 1; i < 2; i++)
        {
            Instantiate(enemy, spawnPositions[i], spawnRotation);
        }
        
        GameObject enemy2 = enemies2[Random.Range(0, enemies2.Length)];
        Vector3[] spawnPositions2 = new[] { new Vector3(x, y, z + 1), new Vector3(x + 1, y, z + 1) };

        Quaternion spawnRotation2 = Quaternion.identity;
        for (int i = 1; i < 2; i++)
        {
            Instantiate(enemy2, spawnPositions2[i], spawnRotation2);
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
