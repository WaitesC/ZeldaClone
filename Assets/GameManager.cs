using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    Animator playerAnimator;

    public bool playerRetaliate;

    public bool moveTowardsEnemy;

    private float fixedDeltaTime;

    public float attackAnimSpeedMulti;

    public bool canMove;

    Image slowMotionVisual;

    void Awake()
    {
        // Make a copy of the fixedDeltaTime, it defaults to 0.02f, but it can be changed in the editor
        this.fixedDeltaTime = Time.fixedDeltaTime;
    }

    // Start is called before the first frame update
    void Start()
    {
        playerAnimator = GameObject.Find("Player").GetComponent<Animator>();

        playerRetaliate = false;
        slowMotionVisual = GameObject.Find("Slow Motion Visual").GetComponent<Image>();
        slowMotionVisual.enabled = !slowMotionVisual.enabled;

        //attackAnimSpeedMulti = 2.5f;
        playerAnimator.SetFloat("AttackAnimSpeedMulti", attackAnimSpeedMulti);
    }

    // Update is called once per frame
    void Update()
    {
        

        if (Time.timeScale == 1.0f)
        {
            playerAnimator.SetFloat("AttackAnimSpeedMulti", attackAnimSpeedMulti);
            slowMotionVisual.enabled = false;

        }

    }

    public void SlowDown()
    {

        if (Time.timeScale == 1.0f)
        {
            SlowDownStart();
        }
        else
            SlowDownEnd();
        // Adjust fixed delta time according to timescale
        // The fixed delta time will now be 0.02 frames per real-time second
        Time.fixedDeltaTime = this.fixedDeltaTime * Time.timeScale;
    }

    //resets time to normal after dodge is finished
    void ResetTime()
    {
        Time.timeScale = 1.0f;
        slowMotionVisual.enabled = false;

        playerRetaliate = false;
    }

    void SlowDownStart()
    {
        slowMotionVisual.enabled = true;
        playerRetaliate = true;
        //speed at which the game runs during slow mo
        Time.timeScale = 0.1f;

        //how long until the speed is reset
        Invoke("ResetTime", 0.2f);

        playerAnimator.SetFloat("AttackAnimSpeedMulti", 30.0f);
    }

    void SlowDownEnd()
    {
        playerAnimator.SetFloat("AttackAnimSpeedMulti", 2.5f);

        Time.timeScale = 1.0f;

        slowMotionVisual.enabled = !slowMotionVisual.enabled;

    }
}
