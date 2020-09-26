using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;

    public float speed = 6f;

    float speedMultiplier = 1;

    public float smoothTurnTime = 0.1f;

    float smoothTurnVelocity;

    void Update()
    {
        Movement();

        SprintCheck();
    }

    //handles character movement
    void Movement()
    {
        float hor = Input.GetAxisRaw("Horizontal");
        float ver = Input.GetAxisRaw("Vertical");
        Vector3 dir = new Vector3(hor, 0f, ver).normalized;

        if (dir.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref smoothTurnVelocity, smoothTurnTime);
            //change the following thingy for rotating towards enemy
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime * speedMultiplier);
        }
    }

    //checks for sprint input
    void SprintCheck()
    {
        if (Input.GetAxis("Sprint") != 0f)
        {
            speedMultiplier = 3;
        }

        if (Input.GetAxis("Sprint") != 1f)
        {
            speedMultiplier = 1;
        }
    }
}
