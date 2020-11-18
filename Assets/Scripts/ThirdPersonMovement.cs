using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    //reference to char controller
    public CharacterController controller;
    //reference to enemy targeter controller
    //public GameObject EnemyTargeter;
    EnemyTargeter enemyTargeter;
    //ref to cameraq
    public Transform cam;
    //ref to camera target
    public Transform target;

    public Vector3 dir;
    float hor, ver;
    //speed stuff
    public float speed = 6f;
    public float movMag;

    float speedMultiplier = 1;

    //turning stuff
    public float smoothTurnTime = 0.1f;
    float smoothTurnVelocity;

    //gravity stuff
    public Vector3 velocity;
    public float gravity = -9.81f;
    public bool isGrounded;
    public float jumpHeight = 3f;

    //ground check stuff
    public Transform groundCheck;
    public float groundDistance = 0.1f;
    public LayerMask groundMask;

    //Dodge stuff
    //https://medium.com/ironequal/unity-character-controller-vs-rigidbody-a1e243591483
    public Vector3 Drag;
    public float DashDistance = 5f;

    //player animation
    Animator animator;

    private float fixedDeltaTime;

    //rotation angle for player movement/ lock on
    float angle;
    float targetAngle;

    GameManager gameManager;

    void Update()
    {
        Movement();

    }

    void Awake()
    {
        //cam = GameObject.Find("Main Camera").GetComponent<Transform>();
        // Make a copy of the fixedDeltaTime, it defaults to 0.02f, but it can be changed in the editor
        this.fixedDeltaTime = Time.fixedDeltaTime;
    }

    void Start()
    {
        enemyTargeter = GetComponent<EnemyTargeter>();
        animator = GetComponent<Animator>();
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    //handles character movement
    void Movement()
    {
        

        //sets to is grounded when touching anything with ground mask
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if(isGrounded && velocity.y<0)
        {
            velocity.y = -2f;
        }

        if (gameManager.canMove)
        {
            hor = Input.GetAxisRaw("Horizontal");
            ver = Input.GetAxisRaw("Vertical");
        }
        else
            movMag = 0;

         

        movMag = new Vector3(hor, 0f, ver).sqrMagnitude;

        animator.SetFloat("NormalSpeed", movMag);

        dir = new Vector3(hor, 0f, ver).normalized;


        if (dir.magnitude >= 0.1f)
        {
            //run animation

            //angles for direction player faces
             targetAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
             angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref smoothTurnVelocity, smoothTurnTime);

            var step = speed * Time.deltaTime;


            //face enemy when locked on
            if (enemyTargeter.lockedOnToEnemy == true)
            {


                transform.LookAt(target.transform);

                //transform.rotation = Quaternion.Euler(0f, angle, 0f);

            }
            else
                transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);
            

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime * speedMultiplier);
        }

        SprintCheck();

        Jump();

        Gravity();

        Dodge();

        velocity.x /= 1 + Drag.x * Time.deltaTime;
        velocity.y /= 1 + Drag.y * Time.deltaTime;
        velocity.z /= 1 + Drag.z * Time.deltaTime;
    }

    //checks for sprint input
    void SprintCheck()
    {
        if (Input.GetAxis("Sprint") != 0f && isGrounded)
        {
            speedMultiplier = 3;

            animator.SetBool("Sprint", true);
        }

        if (Input.GetAxis("Sprint") != 1f)
        {
            speedMultiplier = 1;

            animator.SetBool("Sprint", false);
        }
    }

    void Gravity()
    {
        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

    void Jump()
    {
        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            //do jump animation
            animator.SetTrigger("Jump");
            
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    void Dodge()
    {
        

        if (Input.GetButtonDown("Dodge") && isGrounded)
        {
            animator.SetTrigger("Dodge");
            //dodge direction based off player direction when lcoked on
            if (enemyTargeter.lockedOnToEnemy == true)
            {

                velocity += Vector3.Scale(dir, DashDistance * new Vector3((Mathf.Log(1f / (Time.deltaTime * Drag.x + 1)) / -Time.deltaTime), 0, (Mathf.Log(1f / (Time.deltaTime * Drag.z + 1)) / -Time.deltaTime)));
            }
            else
                velocity += Vector3.Scale(transform.forward, DashDistance * new Vector3((Mathf.Log(1f / (Time.deltaTime * Drag.x + 1)) / -Time.deltaTime), 0, (Mathf.Log(1f / (Time.deltaTime * Drag.z + 1)) / -Time.deltaTime)));
            
            
        }
    }
}
