using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    CharacterController controller;

    [SerializeField]
    public float gravity = 20;
    public float speed = 5, jumpSpeed = 10, rotationSpeed = 5, runSpeed = 10;

    Transform centerCamera, BaseDirection;
    Camera cam;

    float FallSpeed = 0, angleCamera = 0;

    bool isRunning;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        BaseDirection = transform.Find("BaseDirection");
        centerCamera = BaseDirection.Find("CenterCamera");
        cam = centerCamera.Find("Camera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        LayerMask newMask = ~(~9);
        isRunning = Input.GetKey(KeyCode.LeftShift);

        Vector3 move;
        if (!isRunning)
            move = (BaseDirection.forward * Input.GetAxis("Vertical") +
                       BaseDirection.right * Input.GetAxis("Horizontal")).normalized * speed * Time.deltaTime;
        else
            move = (BaseDirection.forward * Input.GetAxis("Vertical") +
                       BaseDirection.right * Input.GetAxis("Horizontal")).normalized * runSpeed * Time.deltaTime;
        //Saut
        if (controller.isGrounded)//Si au sol
        {
            
                if (Input.GetButton("Jump"))
                {
                    FallSpeed = -jumpSpeed;
                }
                else
                {
                    if (isRunning)
                        FallSpeed = runSpeed;
                    else
                        FallSpeed = speed;
                }
           

        }
        else
        {
            FallSpeed += gravity * Time.deltaTime;

        }

        move += Vector3.down * FallSpeed * Time.deltaTime;
        BaseDirection.localEulerAngles += Vector3.up * rotationSpeed * Input.GetAxis("Mouse X");

        angleCamera += rotationSpeed * Input.GetAxis("Mouse Y");
        if (angleCamera < -90) angleCamera = -90;
        if (angleCamera > 90) angleCamera = 90;
        centerCamera.localEulerAngles = Vector3.left * angleCamera + Vector3.up * centerCamera.localEulerAngles.y;

        RaycastHit hit;
        //Debug.DrawRay(centerCamera.position, -centerCamera.forward * 5, Color.green);
        if (Physics.Raycast(centerCamera.position, -centerCamera.forward, out hit, 5 + cam.nearClipPlane, newMask))
        {
            cam.transform.localPosition = Vector3.back * (hit.distance - cam.nearClipPlane);

            //Debug.DrawRay(centerCamera.position, hit.point - centerCamera.position, Color.red);
        }
        else
        {
            cam.transform.localPosition = Vector3.back * (5 + cam.nearClipPlane);
        }

        controller.Move(move);
    }
}
