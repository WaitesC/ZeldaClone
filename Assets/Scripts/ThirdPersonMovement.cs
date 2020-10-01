using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    //reference to char controller
    public CharacterController controller;
    //reference to enemy targeter controller
    public GameObject EnemyTargeter;
    //ref to cameraq
    public Transform cam;

    //speed stuff
    public float speed = 6f;

    float speedMultiplier = 1;

    //turning stuff
    public float smoothTurnTime = 0.1f;
    float smoothTurnVelocity;

    //gravity stuff
    Vector3 velocity;
    public float gravity = -9.81f;
    bool isGrounded;
    public float jumpHeight = 3f;

    //ground check stuff
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    public Transform target;


    void Update()
    {
        Movement();

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

        float hor = Input.GetAxisRaw("Horizontal");
        float ver = Input.GetAxisRaw("Vertical");
        Vector3 dir = new Vector3(hor, 0f, ver).normalized;

        if (dir.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float targetAngle2 = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg + target.position.y - transform.position.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref smoothTurnVelocity, smoothTurnTime);
            float angle2 = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle2, ref smoothTurnVelocity, smoothTurnTime);

            var step = speed * Time.deltaTime;

            //code for facing enemy, MIGHT NEED SOME EXTRA WORK
            //right now only faces towards enemy but 
            if (EnemyTargeter.GetComponent<EnemyTargeter>().lockedOnToEnemy == true)
            {


                transform.LookAt(target.transform);

                //transform.rotation = Quaternion.Euler(0f, angle, 0f);

            }
            else
                transform.rotation = Quaternion.Euler(0f, angle, 0f);


            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime * speedMultiplier);
        }

        SprintCheck();

        Jump();

        Gravity();
    }

    //checks for sprint input
    void SprintCheck()
    {
        if (Input.GetAxis("Sprint") != 0f && isGrounded)
        {
            speedMultiplier = 3;
        }

        if (Input.GetAxis("Sprint") != 1f)
        {
            speedMultiplier = 1;
        }
    }

    void Gravity()
    {
        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

    void Jump()
    {
        if(Input.GetAxis("Jump") !=0 && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    
}
