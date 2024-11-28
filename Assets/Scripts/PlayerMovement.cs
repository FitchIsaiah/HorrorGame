using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{


    public CharacterController controller;

    public float speed = 12f;
    public float sprintSpeed = 18f;
    public float crouchSpeed = 6f;
    public float gravity = -9.81f;

    public Transform cameraTransform;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public float jumpHeight = 3f;

    public float crouchHeight = 1f;
    public float standHeight = 2f;
    public float transitionSpeed = 5f;

    private float currentSpeed;
    private Vector3 velocity;
    private bool isGrounded;

    private void Start()
    {
        currentSpeed = speed;
    }



    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; 
        }

        HandleSprintingAndCrouching();
        


        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * currentSpeed * Time.deltaTime);

        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }


        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

    void HandleSprintingAndCrouching()
    {
        // Check if crouching
        if (Input.GetKey(KeyCode.C))
        {
            // Enter crouch state
            controller.height = Mathf.Lerp(controller.height, crouchHeight, Time.deltaTime * transitionSpeed);
            currentSpeed = crouchSpeed;

            // Adjust camera height
            cameraTransform.localPosition = Vector3.Lerp(
                cameraTransform.localPosition,
                new Vector3(cameraTransform.localPosition.x, crouchHeight / 2, cameraTransform.localPosition.z),
                Time.deltaTime * transitionSpeed
            );
        }
        else
        {
            // Standing height adjustment
            controller.height = Mathf.Lerp(controller.height, standHeight, Time.deltaTime * transitionSpeed);

            // Adjust camera back to standing position
            cameraTransform.localPosition = Vector3.Lerp(
                cameraTransform.localPosition,
                new Vector3(cameraTransform.localPosition.x, standHeight / 2, cameraTransform.localPosition.z),
                Time.deltaTime * transitionSpeed
            );

            // Sprinting logic (only when not crouching)
            if (Input.GetKey(KeyCode.LeftShift))
            {
                currentSpeed = sprintSpeed;
            }
            else
            {
                currentSpeed = speed;
            }
        }

        // Debugging speed to ensure correct logic
        Debug.Log("Current Speed: " + currentSpeed);
    }















}

/*void HandleSprinting()
{
    if (Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.C))

    {
        currentSpeed = sprintSpeed;
    }
    else
    {
        currentSpeed = speed;
    }
}
    void HandleCrouching()
    {
        if (Input.GetKey(KeyCode.C))
        {
            controller.height = Mathf.Lerp(controller.height, crouchHeight, Time.deltaTime * transitionSpeed);
            currentSpeed = crouchSpeed;

        //Smoothly lower the camera
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, new Vector3(cameraTransform.localPosition.x, crouchHeight / 2, cameraTransform.localPosition.z), Time.deltaTime * transitionSpeed);



        }
        else
        {
            controller.height = Mathf.Lerp(controller.height, standHeight, Time.deltaTime * transitionSpeed);
            currentSpeed = speed;

        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, new Vector3(cameraTransform.localPosition.x, standHeight / 2, cameraTransform.localPosition.z), Time.deltaTime * transitionSpeed);

    }


    }
*/

