using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterMovement : MonoBehaviour
{
    public float speed = 5.0f;
    

    private CharacterController controller;
    private Vector3 moveDirection = Vector3.zero;
    public Animator anim;
    
    public static bool ZoomCamera = false;

    // Define a jump velocity that will be applied when the player lands on a jump pad
    public float jumpVelocity = 10.0f;

    // Keep track of whether the player is currently jumping
    private bool isJumping = false;

    // Define the speed at which the character rotates
    public float rotationSpeed = 10.0f;

    private bool IsJumpPadEnabled = true;


    private float JumpPadTimer = 0.0f;
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }
    void Update()
    {
        JumpPadTimer += Time.deltaTime;
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
       // moveDirection = new Vector3(horizontal, 0, vertical).normalized;
        // Only update the x and z components of moveDirection if the player is not jumping
        if (!isJumping)
        {
            moveDirection = new Vector3(horizontal, 0, vertical).normalized;
        }
        else
        {
            // Keep the x and z components of moveDirection, and only update the y component
            //moveDirection += Physics.gravity * Time.deltaTime;

            controller.Move(Vector3.down * Time.deltaTime * 9.81f);
            if (controller.isGrounded ||Math.Floor(moveDirection.y) <= 0)
            {
                isJumping = false;
            }
        }
        if (moveDirection.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0f, targetAngle, 0f);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            //controller.Move(moveDirection * speed * Time.deltaTime);
            if (GrabScript.grabScript.isGrabbing == true)
            {
                anim.SetBool("IsGrabbing", true);
                anim.SetBool("IsMoving", false);
            }
            else
            {
                anim.SetBool("IsGrabbing", false);
                anim.SetBool("IsMoving", true);
            }

        }
        else
        {
            anim.SetBool("IsMoving", false);
        }

        // Check for jump pads
        Collider[] colliders = Physics.OverlapSphere(transform.position, 0.01f);
        foreach (Collider collider in colliders)
        {
            
            if (collider.gameObject.CompareTag("JumpPad") && controller.isGrounded && IsJumpPadEnabled == true)
            {
                JumpCharacterScript.DubbleJump = 2; // don't make the character jump again until they've landed
                moveDirection.y = jumpVelocity;
                // Apply the jump velocity
                isJumping = true;
                IsJumpPadEnabled = false;
                JumpPadTimer = 0.0f;
            }

        }
        // If the jump pad is disabled, wait 5 seconds before enabling it again, this is to prevent the player from jumping multiple times in a row
        if (IsJumpPadEnabled == false && Math.Floor(JumpPadTimer) == 5 )
        {
            JumpPadTimer = 0.0f;
            IsJumpPadEnabled = true;
        }
        controller.Move(moveDirection * speed * Time.deltaTime);

       
    }

    void LateUpdate()
    {
        if (controller.transform.position.y <=-5)
        {
            controller.transform.position = new Vector3(0, 0, 0); // if the player falls off the map, reset their position to the origin
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("ZoomCamera"))
        {
            ZoomCamera = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("ZoomCamera"))
        {
            ZoomCamera = false;
        }
    }
}
