using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public float RotationSpeed = 1;
    public Transform Target, Player;
    float mouseX, mouseY;

    public GameObject something;

    public ThirdPersonCharacterController thirdPersonCharacterController;

    bool lockedOnToEnemy;
    // Start is called before the first frame update
    void Start()
    {
        //Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void LateUpdate()
    {
        CamControl();
    }

    void CamControl()
    {
        mouseX += Input.GetAxis("Mouse X") * RotationSpeed;
        mouseY -= Input.GetAxis("Mouse Y") * RotationSpeed;
        mouseY = Mathf.Clamp(mouseY, -35, 60);

        transform.LookAt(cameraTarget());

        Target.rotation = Quaternion.Euler(mouseY, mouseX, 0);
        Player.rotation = Quaternion.Euler(0, mouseX, 0);

    }

    public Transform cameraTarget()
    {
        if (!thirdPersonCharacterController.lockedOnToEnemy)
            return Target;
        else
            return thirdPersonCharacterController.FindClosestEnemy().transform;
    }
}
